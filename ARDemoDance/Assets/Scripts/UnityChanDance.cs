using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;

public class UnityChanDance : MonoBehaviour
{

    public ProductionDesign floorProduct;

    public int floorIndex;

    public ProductionDesign ceilingProduct;

    public int ceilingIndex;

    public ProductionDesign wallProduct;

    public int wallIndex;

    public Camera ARCamera;

    public GameObject DetectedPlanePrefab;

    public GameObject VerticalPanelObject;

    public GameObject HorizontalPanelObject;

    public GameObject PointObject;

    private const float modelRotation = 180.0f;

    private bool isQuitting = false;

    private const int PUT = 0;
    private const int MOVE = 1;
    public int mode;

    private GameObject selectedObject;

    [SerializeField] private LayerMask mask;

    public void Awake()
    {
        Application.targetFrameRate = 60;

        selectedObject = null;
    }

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

    private void PutObjects(Touch touch)
    {
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) return;

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

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
                        return;
                    }
                    else if (detectedPlane.PlaneType == DetectedPlaneType.HorizontalDownwardFacing)
                    {
                        GameObject obj = ceilingProduct.PutCeiling(ceilingIndex, hit.Pose.position, hit.Pose.rotation);

                        Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);
                        obj.transform.parent = anchor.transform;
                    }
                    else if (detectedPlane.PlaneType == DetectedPlaneType.HorizontalUpwardFacing)
                    {
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

    private void MoveObjects(Touch touch)
    {
        if (Input.touchCount < 1) { return; }

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { return; }

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (selectedObject == null)
                {
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
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit rHit, 10.0f, mask))
                    {
                        selectedObject.transform.position = rHit.point + (Vector3.up * rHit.collider.transform.localScale.y);
                        break;
                    }

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

        //if (selectedObject == null)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //    if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, mask))
        //    {
        //        selectedObject = hit.collider.gameObject;
        //        SelectedObject.Name = selectedObject.name;
        //    }
        //}
        //else
        //{
        //    TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
        //    if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out TrackableHit hit))
        //    {
        //        if ((hit.Trackable is DetectedPlane) &&
        //            Vector3.Dot(ARCamera.transform.position - hit.Pose.position,
        //            hit.Pose.rotation * Vector3.up) < 0) { }
        //        else
        //        {
        //            if (Physics.SphereCast(hit.Pose.position, 2.0f, Vector3.zero, out RaycastHit hit2, 0.0f))
        //            {
        //                selectedObject.transform.position = hit.Pose.position + (Vector3.up * hit2.transform.localScale.y);
        //            }
        //            else
        //            {
        //                selectedObject.transform.position = hit.Pose.position + (Vector3.up * 0.1f);
        //            }
        //        }
        //    }

        //    //// タップした座標が平面か判定する
        //    //TrackableHitFlags filter = TrackableHitFlags.PlaneWithinPolygon;
        //    //if (Frame.Raycast(touch.position.x, touch.position.y, filter, out TrackableHit hit))
        //    //{
        //    //    // 平面にヒットしたら位置・姿勢を指定
        //    //    selectedObject.transform.position = hit.Pose.position;
        //    //    selectedObject.transform.rotation = hit.Pose.rotation;
        //    //    selectedObject.transform.Rotate(0, 180, 0, Space.Self);

        //    //    // Anchorを設定
        //    //    Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);
        //    //    selectedObject.transform.parent = anchor.transform;
        //    //    // ※Anchor設定は指を離した時だけで十分。
        //    //}
        //}
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
