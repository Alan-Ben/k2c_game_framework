using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GOE
{
    public class URPCameraManager
    {
        private static URPCameraManager _g_instance = new URPCameraManager();
        [NotNull]
        public static URPCameraManager instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new URPCameraManager();
                return _g_instance;
            }
        }
        private bool _m_isMainCameraEnable = true;
        private bool _m_isUICameraEnable = true;
        private bool _m_isRTMainCameraEnable = true;
        
        [NotNull] private Camera _m_mainCamera;
        [NotNull] private Camera _m_uiCamera;
        [NotNull] private Camera _m_rtMainCamera;
        [NotNull] private UniversalAdditionalCameraData _m_mainCameraData;
        [NotNull] private UniversalAdditionalCameraData _m_uiCameraData;
        [NotNull] private UniversalAdditionalCameraData _m_rtMainCameraData;
        
        private bool _m_isInit = false;
        
        /// <summary>
        /// 初始化所有相机数据
        /// </summary>
        /// <param name="_mainCamera"></param>
        /// <param name="_uiCamera"></param>
        /// <param name="_rtMainCamera"></param>
        /// <param name="_mainCameraData"></param>
        /// <param name="_uiCameraData"></param>
        /// <param name="_rtMainCameraData"></param>
        public void init(Camera _mainCamera, Camera _uiCamera, Camera _rtMainCamera, UniversalAdditionalCameraData _mainCameraData, UniversalAdditionalCameraData _uiCameraData, UniversalAdditionalCameraData _rtMainCameraData)
        {
            _m_mainCamera = _mainCamera;
            _m_uiCamera = _uiCamera;
            _m_rtMainCamera = _rtMainCamera;
            _m_mainCameraData = _mainCameraData;
            _m_uiCameraData = _uiCameraData;
            _m_rtMainCameraData = _rtMainCameraData;
            _m_isMainCameraEnable = true;
            _m_isUICameraEnable = true;
            _m_isRTMainCameraEnable = false;
            if(_m_mainCamera == null || _m_uiCamera == null || _m_rtMainCamera == null || _m_mainCameraData == null || _m_uiCameraData == null || _m_rtMainCameraData == null)
            {
                Debug.LogError("URPCameraManager init failed");
                _m_isInit = false;
            }
            else
            {
                _m_isInit = true;
            }
        }

        /// <summary>
        /// 开关主相机
        /// </summary>
        /// <param name="isEnable"></param>
        public void setMainCameraEnable(bool isEnable)
        {
            _m_isMainCameraEnable = isEnable;
            update();
        }
        
        /// <summary>
        /// 开关RT主相机
        /// </summary>
        /// <param name="isEnable"></param>
        public void setRTMainCameraEnable(bool isEnable)
        {
            _m_isRTMainCameraEnable = isEnable;
            update();
        }

        /// <summary>
        /// 根据相机开关情况，设置相机的开关、overlay和base
        /// </summary>
        private void update()
        {
            if(!_m_isInit)
                return;
            if (_m_isRTMainCameraEnable)
            {
                _m_mainCamera.enabled = false;
                _m_mainCameraData.renderType = CameraRenderType.Base;
                _m_mainCameraData.cameraStack.Clear();
                _m_rtMainCamera.enabled = true;
                _m_uiCameraData.renderType = CameraRenderType.Overlay;
                _m_rtMainCameraData.renderType = CameraRenderType.Base;
                _m_rtMainCameraData.cameraStack.Clear();
                _m_rtMainCameraData.cameraStack.Add(_m_uiCamera);
                _m_rtMainCamera.backgroundColor = Color.clear;
            }
            else if (_m_isMainCameraEnable)
            {
                _m_rtMainCamera.enabled = false;
                _m_rtMainCameraData.renderType = CameraRenderType.Base;
                _m_rtMainCameraData.cameraStack.Clear();
                _m_mainCamera.enabled = true;
                _m_uiCameraData.renderType = CameraRenderType.Overlay;
                _m_mainCameraData.renderType = CameraRenderType.Base;
                _m_mainCameraData.cameraStack.Clear();
                _m_mainCameraData.cameraStack.Add(_m_uiCamera);
            }
            else
            {
                _m_rtMainCamera.enabled = false;
                _m_rtMainCameraData.renderType = CameraRenderType.Base;
                _m_rtMainCameraData.cameraStack.Clear();
                _m_mainCamera.enabled = false;
                _m_mainCameraData.renderType = CameraRenderType.Base;
                _m_mainCameraData.cameraStack.Clear();
                _m_uiCameraData.renderType = CameraRenderType.Base;
                _m_uiCamera.backgroundColor = Color.clear;
                _m_uiCamera.clearFlags = CameraClearFlags.SolidColor;
            }
        }
    }
}