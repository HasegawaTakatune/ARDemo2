using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;

/// <summary>
/// オブジェクト生成・移動をする
/// </summary>
public class FurnitureController : MonoBehaviour
{

    /// <summary>
    /// 床家具生成リスト
    /// </summary>
    public ProductionDesign floorProduct;

    /// <summary>
    /// 床家具インデックス
    /// </summary>
    public int floorIndex;

    /// <summary>
    /// 天井家具生成リスト
    /// </summary>
    public ProductionDesign ceilingProduct;

    /// <summary>
    /// 床家具インデックス
    /// </summary>
    public int ceilingIndex;

    /// <summary>
    /// 壁家具生成リスト
    /// </summary>
    public ProductionDesign wallProduct;

    /// <summary>
    /// 壁家具インデックス
    /// </summary>
    public int wallIndex;

    /// <summary>
    /// ARカメラ
    /// </summary>
    public Camera ARCamera;

    /// <summary>
    /// 
    /// </summary>
    private bool isQuitting = false;

    /// <summary>
    /// 置く
    /// </summary>
    private const int PUT = 0;

    /// <summary>
    /// 移動
    /// </summary>
    private const int MOVE = 1;

    /// <summary>
    /// モード
    /// </summary>
    public int mode;

    /// <summary>
    /// 選択したオブジェクト
    /// </summary>
    private GameObject selectedObject;

    /// <summary>
    /// レイキャストのマスク
    /// </summary>
    [SerializeField] private LayerMask mask;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Awake()
    {
        Application.targetFrameRate = 60;

        selectedObject = null;
    }

    /// <summary>
    /// メインループ
    /// </summary>
    void Update()
    {
        _UpdateApplicationLifecycle();

        Touch touch = Input.GetTouch(0);

        switch (mode)
        {
            case PUT: PutObjects(touch); break;
            case MOVE: MoveObjects(touch); break;
        }
    }

    /// <summary>
    /// オブジェクトを置く
    /// </summary>
    /// <param name="touch">タッチ情報</param>
    private void PutObjects(Touch touch)
    {
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) return;

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        // 認識した面によって生成するオブジェクトを切り替える
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out TrackableHit hit))
        {
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(ARCamera.transform.position - hit.Pose.position,
                hit.Pose.rotation * Vector3.up) < 0) { }
            else
            {
                if (hit.Trackable is FeaturePoint)
                {
                    return;
                }
                else if (hit.Trackable is DetectedPlane)
                {
                    DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                    if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                    {
                        // 壁オブジェクト生成
                        return;
                    }
                    else if (detectedPlane.PlaneType == DetectedPlaneType.HorizontalDownwardFacing)
                    {
                        // 天井オブジェクト生成
                        GameObject obj = ceilingProduct.PutCeiling(ceilingIndex, hit.Pose.position, hit.Pose.rotation);

                        Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        obj.transform.parent = anchor.transform;
                    }
                    else if (detectedPlane.PlaneType == DetectedPlaneType.HorizontalUpwardFacing)
                    {
                        // 床オブジェクト生成
                        GameObject obj = floorProduct.PutFloor(floorIndex, hit.Pose.position, hit.Pose.rotation);

                        Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        obj.transform.parent = anchor.transform;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }

    /// <summary>
    /// オブジェクトを移動
    /// </summary>
    /// <param name="touch"></param>
    private void MoveObjects(Touch touch)
    {
        if (Input.touchCount < 1) { return; }

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { return; }

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (selectedObject == null)
                {
                    // 移動するオブジェクトを選択する
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, mask))
                    {
                        selectedObject = hit.collider.gameObject;
                        selectedObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (selectedObject != null)
                {
                    // ほかのオブジェクトの位置に移動した場合、上に乗っかるようにする
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit rHit, 10.0f, mask))
                    {
                        selectedObject.transform.position = rHit.point + (Vector3.up * rHit.collider.transform.localScale.y);
                        break;
                    }

                    // 認識した面を移動する
                    TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
                    if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out TrackableHit tHit))
                    {
                        selectedObject.transform.position = tHit.Pose.position + (Vector3.up * 0.1f);
                    }
                }
                break;

            default:
                selectedObject.layer = LayerMask.NameToLayer("Furniture");
                selectedObject = null;
                break;

        }        
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void _UpdateApplicationLifecycle()
    {
        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (isQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            isQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            isQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void _DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
