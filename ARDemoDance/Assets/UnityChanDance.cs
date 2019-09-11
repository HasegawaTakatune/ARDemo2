using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;

public class UnityChanDance : MonoBehaviour
{



    public Camera ARCamera;

    public GameObject DetectedPlanePrefab;

    public GameObject VerticalPanelObject;

    public GameObject HorizontalPanelObject;

    public GameObject PointObject;

    private const float modelRotation = 180.0f;

    private bool isQuitting = false;

    public void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {

    }

    void Update()
    {
        _UpdateApplicationLifecycle();

        Touch touch = Input.GetTouch(0);

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
                GameObject prefab;
                if (hit.Trackable is FeaturePoint)
                {
                    prefab = PointObject;
                }
                else if (hit.Trackable is DetectedPlane)
                {
                    DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                    if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                    {
                        prefab = VerticalPanelObject;
                    }
                    else
                    {
                        prefab = HorizontalPanelObject;
                    }
                }
                else
                {
                    prefab = HorizontalPanelObject;
                }

                GameObject obj = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);
                obj.transform.Rotate(0, modelRotation, 0, Space.Self);

                Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

                obj.transform.parent = anchor.transform;
            }
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
