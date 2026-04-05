
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GOE
{
    /// <summary>
    /// 游戏主相机控制器
    /// </summary>
    public sealed partial class CameraController
    {
        [NotNull] public static CameraController instance { get { return _g_instance ??= new CameraController(); } }
        private static CameraController _g_instance;
        
        [NotNull] private readonly ALCameraController _m_alCameraController; // 内部含有 ALCameraController 把一些方法封闭起来，保证 CameraController 的调用稳定
        private int _m_cullingMask; // 主相机的 CullingMask
        [ItemNotNull] private Camera[] _m_childCameras; // 子相机
        private int[] _m_childCameraCullingMasks; // 子相机的 CullingMask
        private UniversalAdditionalCameraData _m_additionalCameraData; // 主相机的 UniversalAdditionalCameraData
        
        public UniversalAdditionalCameraData additionalCameraData { get { return _m_additionalCameraData; } } // 主相机的 UniversalAdditionalCameraData

        private CameraController()
        {
            _m_cameraIsOpenRender = true;
            _m_alCameraController = new ALCameraController(6);
            
            _m_posStaticController = new CameraPosStaticController();
            _m_focusPosStaticController = new CameraFocusPosStaticController();
            _m_fieldOfViewStaticController = new CameraFieldOfViewStaticController();
            _m_orthographicSizeStaticController = new CameraOrthographicSizeStaticController();
        }
        
        /// <summary>
        /// 游戏主相机
        /// </summary>
        public Camera controlCamera { get { return _m_alCameraController.controlCamera; } }

        /// <summary>
        /// 设置本对象控制的摄像头
        /// </summary>
        public void setControlCamera(Camera _camera)
        {
            if (_camera == null)
                return;
            
            // 先记录一下原始值
            Vector3 originCameraPos = cameraPos;
            Vector3 originCameraFocusPos = cameraFocusPos;
            float originFieldOfView = cameraFieldOfView;
            float originOrthographicSize = cameraOrthographicSize;
            
            _m_alCameraController.setControlCamera(_camera);

            // 等底层设置完再设回来
            cameraPos = originCameraPos;
            cameraFocusPos = originCameraFocusPos;
            cameraFieldOfView = originFieldOfView;
            cameraOrthographicSize = originOrthographicSize;

            if (controlCamera != null)
            {
                // 赋值当前的参数
                controlCamera.orthographic = _m_cameraIsOrthographic;
                controlCamera.nearClipPlane = _m_cameraNearClipPlane;
                controlCamera.farClipPlane = _m_cameraFarClipPlane;
                // 储存相机的 CullingMask，用于开关相机渲染用
                _m_cullingMask = controlCamera.cullingMask;
                if (!_m_cameraIsOpenRender)
                    controlCamera.cullingMask = 0;
                _m_childCameras = controlCamera.GetComponentsInChildren<Camera>();
                if (_m_childCameras != null)
                {
                    _m_childCameraCullingMasks = new int[_m_childCameras.Length];
                    for (int i = 0; i < _m_childCameras.Length; i++)
                    {
                        Camera childCamera = _m_childCameras[i];
                        if (childCamera == null)
                            continue;
                        
                        _m_childCameraCullingMasks[i] = childCamera.cullingMask;
                        // 给子相机赋值对应的参数
                        childCamera.orthographic = _m_cameraIsOrthographic;
                        childCamera.nearClipPlane = _m_cameraNearClipPlane;
                        childCamera.farClipPlane = _m_cameraFarClipPlane;
                        if (!_m_cameraIsOpenRender)
                            childCamera.cullingMask = 0;
                    }
                }
                else
                    _m_childCameraCullingMasks = null;
                
                _m_additionalCameraData = controlCamera.GetComponent<UniversalAdditionalCameraData>();
            }
            else
            {
                _m_childCameras = null;
                _m_childCameraCullingMasks = null;
                _m_additionalCameraData = null;
            }
        }
        /// <summary>
        /// 使用相机设置刷新
        /// </summary>
        public void refreshByCameraSetting(WCGCameraSettingInfo _cameraSettingInfo)
        {
            refreshByCameraSetting(_cameraSettingInfo, Vector3.zero);
        }
        /// <summary>
        /// 使用相机设置刷新
        /// </summary>
        public void refreshByCameraSetting(WCGCameraSettingInfo _cameraSettingInfo, Vector3 _cornerPos)
        {
            // 把配置中的参数赋值过来
            cameraIsOrthographic = _cameraSettingInfo.isOrthographic;
            cameraNearClipPlane = _cameraSettingInfo.clippingNear;
            cameraFarClipPlane = _cameraSettingInfo.clippingFar;
            cameraPos = _cameraSettingInfo.cameraPosition + _cornerPos;
            cameraFocusPos = _cameraSettingInfo.cameraFocusPosition + _cornerPos;
            cameraOrthographicSize = _cameraSettingInfo.orthographicSize;
            cameraFieldOfView = _cameraSettingInfo.cameraFieldOfView;
            _m_alCameraController.setDefaultOrthographicsSize(_cameraSettingInfo.originSize);
            // 应用一次各种数值
            frameCheck();
            // 刷新所有面朝相机的物体
            refreshFaceCameraObjs();
        }
        /// <summary>
        /// 刷新所有面朝相机的物体
        /// </summary>
        /// <remarks>
        /// 有一个叫做 <see cref="FaceCameraMgr"/> 的东西，这个方法会更新相机的位置
        /// </remarks>
        public void refreshFaceCameraObjs()
        {
            FaceCameraMgr.instance.setRotation(cameraRotation);
        }
        /// <summary>
        /// 根据屏幕坐标从相机发射一条射线 
        /// </summary>
        public Ray screenPosToRay(Vector2 _screenPos)
        {
            return viewportPosToRay(new Vector2(_screenPos.x / Screen.width, _screenPos.y / Screen.height));
        }
        /// <summary>
        /// 根据视口坐标从相机发射一条射线 
        /// </summary>
        public Ray viewportPosToRay(Vector2 _viewportPos)
        {
            Vector3 forward = (cameraFocusPos - cameraPos).normalized;
            if (_viewportPos is { x: 0.5f, y: 0.5f })
                return new Ray(cameraPos, forward);

            Vector3 cameraUp = cameraRotation * Vector3.up;
            Vector3 cameraRight = cameraRotation * Vector3.right;
            Rect cameraViewRect = controlCamera != null ? controlCamera.rect : Rect.zero; // todo: 把这个 rect 也放到设置里面
            if (_m_cameraIsOrthographic)
            {
                float planeHeight = cameraOrthographicSize * 2;
                float planeWidth = planeHeight / (Screen.height * cameraViewRect.height) * (Screen.width * cameraViewRect.width);

                Vector3 leftBottom = cameraPos + (-cameraRight) * planeWidth / 2 + (-cameraUp) * planeHeight / 2;
                Vector3 rayOrigin = leftBottom + cameraRight * Mathf.Lerp(0, planeWidth, _viewportPos.x) + cameraUp * Mathf.Lerp(0, planeHeight, _viewportPos.y);
                return new Ray(rayOrigin, forward);
            }
            else
            {
                // 计算垂直于相机视野方向，并且与相机相距100的视野平面的四个角
                float distance = 100;
                float planeHeight = Mathf.Tan(cameraFieldOfView / 2 * Mathf.Deg2Rad) * distance * 2;
                float planeWidth = planeHeight / (Screen.height * cameraViewRect.height) * (Screen.width * cameraViewRect.width);

                // 计算平面的四个角的坐标。先把相机的坐标推到平面的中点
                Vector3 center = forward * distance + cameraPos;
                Vector3 leftBottom = center + (-cameraRight) * planeWidth / 2 + (-cameraUp) * planeHeight / 2;
                Vector3 targetPos = leftBottom + cameraRight * Mathf.Lerp(0, planeWidth, _viewportPos.x) + cameraUp * Mathf.Lerp(0, planeHeight, _viewportPos.y);
                return new Ray(cameraPos, targetPos - cameraPos);
            }
        }

        public void regExtraController(_IALCameraExtraController _extraController)
        {
            _m_alCameraController.regExtraController(_extraController);
        }
        public void unregExtraController(_IALCameraExtraController _extraController)
        {
            _m_alCameraController.unregExtraController(_extraController);
        }

        public void openNormalRateWidthFit(Vector2 _normalRate, bool _openPerspectiveWidthFit, bool _openOrthographicWidthFit)
        {
            _m_alCameraController.openNormalRateWidthFit(_normalRate, _openPerspectiveWidthFit, _openOrthographicWidthFit);
        }

        public void frameCheck()
        {
            // 如果相机对象存在，先把值记下来，一会比对用
            Vector3 originCameraPos = Vector3.zero;
            Quaternion originQuaternion = Quaternion.identity;
            float originOrthographicSize = 0;
            float originFieldOfView = 0;
            if (controlCamera != null && _m_cameraMonitorList.Count > 0)
            {
                originCameraPos = controlCamera.transform.position;
                originQuaternion = controlCamera.transform.rotation;
                originOrthographicSize = controlCamera.orthographicSize;
                originFieldOfView = controlCamera.fieldOfView;
            }
            
            // 刷新底层要刷的东西
            _m_alCameraController.frameCheck();
            
            // 补充刷新底层没有刷的东西
            if (_m_childCameras != null)
            {
                foreach (Camera childCamera in _m_childCameras)
                {
                    // 这里不刷正交 size，底层刷了（= =d）
                    childCamera.fieldOfView = cameraFieldOfView;
                }
            }
            
            // 开始比对值的新旧
            if (controlCamera != null && _m_cameraMonitorList.Count > 0)
            {
                bool transformChg = false;
                if (originCameraPos != controlCamera.transform.position)
                {
                    transformChg = true;
                    foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
                    {
                        monitor.onCameraPosChg();
                    }
                }

                if (originQuaternion != controlCamera.transform.rotation)
                {
                    transformChg = true;
                    foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
                    {
                        monitor.onCameraFocusChg();
                    }
                }
                
                if (transformChg)
                {
                    foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
                    {
                        monitor.onCameraTransformChg();
                    }
                }

                bool viewScaleChg = false;
                if (originOrthographicSize != controlCamera.orthographicSize)
                {
                    viewScaleChg = true;
                    foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
                    {
                        monitor.onOrthographicSizeChg();
                    }
                }

                if (originFieldOfView != controlCamera.fieldOfView)
                {
                    viewScaleChg = true;
                    foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
                    {
                        monitor.onFieldOfViewChg();
                    }
                }

                if (viewScaleChg)
                {
                    foreach (_ICameraMonitor monitor in _m_cameraMonitorList)
                    {
                        monitor.onViewScaleChg();
                    }
                }
            }
        }

        /// <summary>
        ///  设置阴影裁剪边界，提高阴影精度
        /// </summary>
        /// <param name="_openShadowClipOverride">是否要开启重载阴影裁剪边界</param>
        /// <param name="points">围住目标组成的包围盒的8个点</param>
        public void setShadowClipBounds(bool _openShadowClipOverride, Vector3[] points)
        {
            if (_m_additionalCameraData == null)
            {
                Debug.LogError("AdditionalCameraData 为空，@Ben检查");
                return;
            }
            // if(_openShadowClipOverride)
            //     _m_additionalCameraData.shadowClipPoints = points;
            // else
            //     _m_additionalCameraData.shadowClipPoints = null;
        }
    }
}