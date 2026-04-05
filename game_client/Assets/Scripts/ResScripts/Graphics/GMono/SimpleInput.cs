using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GOE;
using UnityEngine.Rendering.Universal;

public class SimpleInput : MonoBehaviour
{
    public bool openRuntimeTest = false;
   // Text m_debugTip;
    public bool canRotation_X = true;
    public bool canRotation_Y = true;
    public bool canScale = true;
    public bool scaleRotation_Y = true;
    public Range scaleRotYRange = new Range(1, 10);
    public WCGFloatRange subsurfaceWidthRange = new WCGFloatRange(1,1);
    public WCGFloatRange shadowClipXRange = new WCGFloatRange(0.5f,0.5f);
    public WCGFloatRange shadowClipYRange = new WCGFloatRange(0.2f,1.95f);
    public WCGFloatRange shadowClipZRange = new WCGFloatRange(0.18f,1.0f);

    #region Field and Property
    /// <summary>
    /// Around center.
    /// </summary>
    public Transform target;
    public Transform targetMax;
    public Transform actorTrans;
    public Range viewRange = new Range(1, 10);

    public Transform cameraTrans;
    public Camera camera;
    /// <summary>
    /// Settings of mouse button, pointer and scrollwheel.
    /// </summary>
    public MouseSettings mouseSettings = new MouseSettings(0, 10, 10, 10);
 
    /// <summary>
    /// Range limit of angle.
    /// </summary>
    public Range angleRange = new Range(-90, 90);
 
    /// <summary>
    /// Range limit of distance.
    /// </summary>
    public Range distanceRange = new Range(1, 10);

    public bool useFovToScale = false;
    public Range fovRange = new Range(4, 25);
    
    public Range yHighRange = new Range(1, 10);
    
    /// <summary>
    /// Damper for move and rotate.
    /// </summary>
    [Range(0, 10)]
    public float damper = 5;
 
    /// <summary>
    /// Camera current angls.
    /// </summary>
    public Vector2 CurrentAngles { protected set; get; }
 
    /// <summary>
    /// Current distance from camera to target.
    /// </summary>
    public float CurrentDistance { protected set; get; }
    public float CurrentFov { protected set; get; }

    public float YCurrentHigh { protected set; get; }
 
    /// <summary>
    /// Camera target angls.
    /// </summary>
    protected Vector2 targetAngles = new Vector2(0, 180);
 
 
    /// <summary>
    /// Target distance from camera to target.
    /// </summary>
    protected float targetDistance;
    protected float targetFov;

    protected float yTargetHigh = 1.75f;

    private Bounds curShadowClipBounds;
    private UniversalAdditionalCameraData m_cameraData;

    #endregion
 
    #region Protected Method
    protected virtual void Start()
    {
      //  m_debugTip = GameObject.Find("Text").GetComponent<Text>();
        CurrentAngles = targetAngles = actorTrans.eulerAngles;
        //CurrentDistance = targetDistance = Vector3.Distance(transform.position, target.position);
        CurrentDistance = targetDistance =  actorTrans.position.z -transform.position.z;
        targetFov = CurrentFov = camera.fieldOfView;
        YCurrentHigh = yTargetHigh = transform.position.y - actorTrans.position.y;
        m_cameraData = camera.GetComponent<UniversalAdditionalCameraData>();
    }
 
    protected virtual void LateUpdate()
    {
        if(Input.GetKey(KeyCode.Escape))
            Application.Quit();
#if UNITY_EDITOR
        AroundByMouseInput();
#elif UNITY_ANDROID || UNITY_IPHONE
 
        AroundByMobileInput();
#else
        AroundByMouseInput();
#endif
 
    }
 
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势  
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;
 
    private bool m_IsSingleFinger;
    /*
    private void ScaleCamera()
    {
        //计算出当前两点触摸点的位置  
        var tempPosition1 = Input.GetTouch(0).position;
        var tempPosition2 = Input.GetTouch(1).position;
        float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
        float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);
        //计算上次和这次双指触摸之间的距离差距  
        //然后去更改摄像机的距离  
        distance -= ( currentTouchDistance - lastTouchDistance ) * scaleFactor * Time.deltaTime;
        //把距离限制住在min和max之间  
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        //备份上一次触摸点的位置，用于对比  
        oldPosition1 = tempPosition1;
        oldPosition2 = tempPosition2;
    }
    */
 
    protected void AroundByMobileInput()
    {
        if (Input.touchCount == 1)
        {
 
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                targetAngles.y -= Input.GetAxis("Mouse X") * mouseSettings.pointerSensitivity * mouseSettings.pointerSensitivityMobileOffset;
                // targetAngles.x -= Input.GetAxis("Mouse Y") * mouseSettings.pointerSensitivity;
                yTargetHigh -= Input.GetAxis("Mouse Y") * mouseSettings.highMoveSensitivity * mouseSettings.highMoveSensitivityMobileOffset;

                //Range.
                targetAngles.x = Mathf.Clamp(targetAngles.x, angleRange.min, angleRange.max);
            }
            //Mouse pointer.
            m_IsSingleFinger = true;
        }
 
        //Mouse scrollwheel.
        if (canScale)
        {
            if (Input.touchCount > 1)
            {
                //计算出当前两点触摸点的位置  
                if (m_IsSingleFinger)
                {
                    oldPosition1 = Input.GetTouch(0).position;
                    oldPosition2 = Input.GetTouch(1).position;
                }
 
                if (Input.touches[0].phase == TouchPhase.Moved && Input.touches[1].phase == TouchPhase.Moved)
                {
                    var tempPosition1 = Input.GetTouch(0).position;
                    var tempPosition2 = Input.GetTouch(1).position;
 
 
                    float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
                    float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);
 
                    //计算上次和这次双指触摸之间的距离差距  
                    //然后去更改摄像机的距离  
                    targetFov -= ( currentTouchDistance - lastTouchDistance ) * Time.deltaTime * mouseSettings.wheelSensitivity * mouseSettings.wheelSensitivityMobileOffset;
                    targetDistance -= ( currentTouchDistance - lastTouchDistance ) * Time.deltaTime * mouseSettings.wheelSensitivity * mouseSettings.wheelSensitivityMobileOffset;
                    //  m_debugTip.text = ( currentTouchDistance - lastTouchDistance ).ToString() + " + " + targetDistance.ToString();
                    //把距离限制住在min和max之间  
                    //备份上一次触摸点的位置，用于对比  
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;
                    m_IsSingleFinger = false;
                }
            }
        }
        
        CommonInput();
    }
 
    /// <summary>
    /// Camera around target by mouse input.
    /// </summary>
    protected void AroundByMouseInput()
    {
        if (Input.GetMouseButton(mouseSettings.mouseButtonID))
        {
            //Mouse pointer.
            targetAngles.y -= Input.GetAxis("Mouse X") * mouseSettings.pointerSensitivity;
            // targetAngles.x -= Input.GetAxis("Mouse Y") * mouseSettings.pointerSensitivity;
            yTargetHigh -= Input.GetAxis("Mouse Y") * mouseSettings.highMoveSensitivity;
            //Range.
            targetAngles.x = Mathf.Clamp(targetAngles.x, angleRange.min, angleRange.max);
        }
 
        //Mouse scrollwheel.
        if (canScale)
        {
            targetDistance -= Input.GetAxis("Mouse ScrollWheel") * mouseSettings.wheelSensitivity;
            targetFov -= Input.GetAxis("Mouse ScrollWheel") * mouseSettings.wheelSensitivity;
        }
       // m_debugTip.text = Input.GetAxis("Mouse ScrollWheel").ToString() + " + " + targetDistance.ToString();
       
       CommonInput();
    }
    private Vector3[] _m_clipPoints = new Vector3[8];
    public void UpdateShadowBounds(float weight)
    {
        if (_m_clipPoints == null)
            _m_clipPoints = new Vector3[8];
        
        float x = Mathf.Lerp(shadowClipXRange.min, shadowClipXRange.max, weight);
        float y = Mathf.Lerp(shadowClipYRange.min, shadowClipYRange.max, weight);
        float z = Mathf.Lerp(shadowClipZRange.min, shadowClipZRange.max, weight);
        NPGameUtility.getIntersectWithLineAndPlane(cameraTrans.position, cameraTrans.forward, Vector3.forward, actorTrans.position, out Vector3 centerPos);
        DebugPlus.DrawBox(centerPos, 2 * new Vector3(x,y,z), Quaternion.identity, Color.red);

        _m_clipPoints[0] = centerPos + new Vector3(x, y, z);
        _m_clipPoints[1] = centerPos + new Vector3(x, -y, z);
        _m_clipPoints[2] = centerPos + new Vector3(x, y, -z);
        _m_clipPoints[3] = centerPos + new Vector3(x, -y, -z);
        _m_clipPoints[4] = centerPos + new Vector3(-x, y, z);
        _m_clipPoints[5] = centerPos + new Vector3(-x, -y, z);
        _m_clipPoints[6] = centerPos + new Vector3(-x, y, -z);
        _m_clipPoints[7] = centerPos + new Vector3(-x, -y, -z);

#if NP_GAME
        // if (m_cameraData is not null)
        // {
        //     m_cameraData.openShadowClipOverride = true;
        //     m_cameraData.shadowClipPoints = _m_clipPoints;
        // }
#endif
    }
    
    private void CommonInput()
    {
        yTargetHigh = Mathf.Clamp(yTargetHigh, yHighRange.min, yHighRange.max);
        
 
        CurrentAngles = Vector2.Lerp(CurrentAngles, targetAngles, damper * Time.deltaTime);
        
    
        YCurrentHigh = Mathf.Lerp(YCurrentHigh, yTargetHigh, damper * Time.deltaTime);
 
        if (!canRotation_X) targetAngles.y = 0;
        if (!canRotation_Y) targetAngles.x = 0;

        actorTrans.rotation =  Quaternion.Euler(0, CurrentAngles.y, 0);
        float normalizeCurDistance = 1;
        if (useFovToScale)
        {
            targetFov = Mathf.Clamp(targetFov, fovRange.min, fovRange.max);
            CurrentFov = Mathf.Lerp(CurrentFov, targetFov, damper * Time.deltaTime);

            // float y = actorTrans.position.y + YCurrentHigh;
            // Vector3 pos = cameraTrans.position;
            // pos.y = y;
            // cameraTrans.position = pos;
            cameraTrans.position = actorTrans.position + Vector3.up * YCurrentHigh - Vector3.forward * CurrentDistance;

            camera.farClipPlane = CurrentDistance + viewRange.max;
            camera.nearClipPlane = MathF.Max(CurrentDistance + viewRange.min,0.01f);
            camera.fieldOfView = CurrentFov;
            normalizeCurDistance = CurrentFov / (fovRange.max - fovRange.min);
        }
        else
        {
            targetDistance = Mathf.Clamp(targetDistance, distanceRange.min, distanceRange.max);
            CurrentDistance = Mathf.Lerp(CurrentDistance, targetDistance, damper * Time.deltaTime);
        
            cameraTrans.position = actorTrans.position + Vector3.up * YCurrentHigh - Vector3.forward * CurrentDistance;
            camera.farClipPlane = CurrentDistance + viewRange.max;
            camera.nearClipPlane = MathF.Max(CurrentDistance + viewRange.min,0.01f);
            normalizeCurDistance = CurrentDistance / (distanceRange.max - distanceRange.min);
        }

        
#if NP_GAME
        // MJUniversalRenderAPI.SubsurfaceWidthGlobal = Mathf.Lerp(subsurfaceWidthRange.min, subsurfaceWidthRange.max, normalizeCurDistance);
#endif
        
        UpdateShadowBounds(normalizeCurDistance);
    }
    #endregion

    private void OnEnable()
    {
        openRuntimeTestCamera();
    }

    private void OnDestroy()
    {
#if NP_GAME
        // MJUniversalRenderAPI.SubsurfaceWidthGlobal = 1;
#endif
    }

    private void openRuntimeTestCamera()
    {
        if (openRuntimeTest)
        {
#if NP_GAME
            if (Application.isPlaying && CameraController.instance.controlCamera)
            {
                if (camera != null && CameraController.instance.controlCamera != camera)
                    camera.enabled = false;
                camera = CameraController.instance.controlCamera;
                cameraTrans = CameraController.instance.controlCamera.transform;
            }
#endif
        }
    }

    private void OnValidate()
    {
        openRuntimeTestCamera();
    }
}
 
[Serializable]
public struct MouseSettings
{
    /// <summary>
    /// ID of mouse button.
    /// </summary>
    public int mouseButtonID;
 
    /// <summary>
    /// Sensitivity of mouse pointer.
    /// </summary>
    public float pointerSensitivity;
    public float pointerSensitivityMobileOffset;

    public float highMoveSensitivity;
    public float highMoveSensitivityMobileOffset;

    /// <summary>
    /// Sensitivity of mouse ScrollWheel.
    /// </summary>
    public float wheelSensitivity;
    public float wheelSensitivityMobileOffset;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mouseButtonID">ID of mouse button.</param>
    /// <param name="pointerSensitivity">Sensitivity of mouse pointer.</param>
    /// <param name="wheelSensitivity">Sensitivity of mouse ScrollWheel.</param>
    public MouseSettings(int mouseButtonID, float pointerSensitivity, float wheelSensitivity, float highMoveSensitivity)
    {
        this.mouseButtonID = mouseButtonID;
        this.pointerSensitivity = pointerSensitivity;
        this.wheelSensitivity = wheelSensitivity;
        this.highMoveSensitivity = highMoveSensitivity;
        this.pointerSensitivityMobileOffset = 0.1f;
        this.wheelSensitivityMobileOffset = 0.1f;
        this.highMoveSensitivityMobileOffset = 0.1f;
    }
}
 
/// <summary>
/// Range form min to max.
/// </summary>
[Serializable]
public struct Range
{
    /// <summary>
    /// Min value of range.
    /// </summary>
    public float min;
 
    /// <summary>
    /// Max value of range.
    /// </summary>
    public float max;
 
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="min">Min value of range.</param>
    /// <param name="max">Max value of range.</param>
    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
 
/// <summary>
/// Rectangle area on plane.
/// </summary>
[Serializable]
public struct PlaneArea
{
    /// <summary>
    /// Center of area.
    /// </summary>
    public Transform center;
 
    /// <summary>
    /// Width of area.
    /// </summary>
    public float width;
 
    /// <summary>
    /// Length of area.
    /// </summary>
    public float length;
 
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="center">Center of area.</param>
    /// <param name="width">Width of area.</param>
    /// <param name="length">Length of area.</param>
    public PlaneArea(Transform center, float width, float length)
    {
        this.center = center;
        this.width = width;
        this.length = length;
    }
}
 
/// <summary>
/// Target of camera align.
/// </summary>
[Serializable]
public struct AlignTarget
{
    /// <summary>
    /// Center of align target.
    /// </summary>
    public Transform center;
 
    /// <summary>
    /// Angles of align.
    /// </summary>
    public Vector2 angles;
 
    /// <summary>
    /// Distance from camera to target center.
    /// </summary>
    public float distance;
 
    /// <summary>
    /// Range limit of angle.
    /// </summary>
    public Range angleRange;
 
    /// <summary>
    /// Range limit of distance.
    /// </summary>
    public Range distanceRange;
 
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="center">Center of align target.</param>
    /// <param name="angles">Angles of align.</param>
    /// <param name="distance">Distance from camera to target center.</param>
    /// <param name="angleRange">Range limit of angle.</param>
    /// <param name="distanceRange">Range limit of distance.</param>
    public AlignTarget(Transform center, Vector2 angles, float distance, Range angleRange, Range distanceRange)
    {
        this.center = center;
        this.angles = angles;
        this.distance = distance;
        this.angleRange = angleRange;
        this.distanceRange = distanceRange;
    }
}