using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GOE
{
    public partial class QualityMgr
    {
        private static readonly string KeyWords_OPEN_SHARPEN_USM = "_OPEN_SHARPEN_USM";
        private static QualityMgr _m_instance;

        public static QualityMgr instance
        {
            get
            {
                if(_m_instance == null)
                {
                    _m_instance = new QualityMgr();
                }
                return _m_instance;
            }
        }
        
        private bool _m_openInstancing = true;
        private bool _m_bOpenShadow = true;//是否开启阴影
        private bool _m_bOpenShowcaseShadow = true;//是否开启阴影
        private bool _m_bOpenPostProcess = false;//是否开启后效
        private bool _m_bOpenClothPhysics = true;//是否开启布料物理
        private int _m_iFurPassCount = 10;//皮毛Pass数量

        public bool isOpenShadow => _m_bOpenShadow;
        public bool isOpenShowcaseShadow => _m_bOpenShowcaseShadow;
        public bool isOpenPostProcess => _m_bOpenPostProcess;
        public bool isOpenClothPhysics => _m_bOpenClothPhysics;
        public int furPassCount => _m_iFurPassCount;

        public void init()
        {
            // MJUniversalRenderAPI.g_OpenPostProcessing = false;
            // MJUniversalRenderAPI.g_IsHdrEnabled = false;
            // MJUniversalRenderAPI.g_OpenPlanarReflect = false;
            // MJUniversalRenderAPI.g_OpenPlanarShadow = false;
            Shader.SetGlobalFloat(ShaderPropertyMgr.g_DarkScale, 1);
            CommonQualityMgr.instance.setRenderTextureDescriptor();
        }

        public void setQualityLevel(ENPGameQuality _level)
        {

            switch (_level)
            {
                case ENPGameQuality.VERY_LOW:
                    setQualityVeryLow();
                    break;
                case ENPGameQuality.LOW:
                    setQualityLow();
                    break;
                case ENPGameQuality.NORMAL:
                    setQualityMedium();
                    break;
                case ENPGameQuality.HIGH:
                    setQualityHigh();
                    break;
                case ENPGameQuality.ULTRA:
                    setQualityUltra();
                    break;
            }

            updateQualitySetting();
            _setGraphicSetting();
        }
        private void setQualityVeryLow()
        {
            _m_bOpenShadow = false;//是否开启阴影
            _m_bOpenShowcaseShadow = false;
            _m_bOpenPostProcess = false;//是否开启后效
            _m_bOpenClothPhysics = false;
            _m_iFurPassCount = 5;
            GraphicsSettings.renderPipelineAsset = Game.instance.mainCamera.urpSetting.veryLowRenderPipelineAsset;
            QualitySettings.renderPipeline = Game.instance.mainCamera.urpSetting.veryLowRenderPipelineAsset;
            _updateURPSetting();
            Shader.DisableKeyword(KeyWords_OPEN_SHARPEN_USM);
            Shader.globalMaximumLOD = 200;
        }
        private void setQualityLow()
        {
            _m_bOpenShadow = false;//是否开启阴影
            _m_bOpenShowcaseShadow = false;
            _m_bOpenPostProcess = false;//是否开启后效
            _m_bOpenClothPhysics = false;
            _m_iFurPassCount = 10;
            GraphicsSettings.renderPipelineAsset = Game.instance.mainCamera.urpSetting.lowRenderPipelineAsset;
            QualitySettings.renderPipeline = Game.instance.mainCamera.urpSetting.lowRenderPipelineAsset;
            _updateURPSetting();
            Shader.DisableKeyword(KeyWords_OPEN_SHARPEN_USM);
            Shader.globalMaximumLOD = 200;
        }
        private void setQualityMedium()
        {
            _m_bOpenShadow = true;//是否开启阴影
            _m_bOpenShowcaseShadow = true;
            _m_bOpenPostProcess = false;//是否开启后效
            _m_bOpenClothPhysics = true;
            _m_iFurPassCount = 10;
            GraphicsSettings.renderPipelineAsset = Game.instance.mainCamera.urpSetting.mediumRenderPipelineAsset;
            QualitySettings.renderPipeline = Game.instance.mainCamera.urpSetting.mediumRenderPipelineAsset;
            _updateURPSetting();
            Shader.EnableKeyword(KeyWords_OPEN_SHARPEN_USM);
            Shader.globalMaximumLOD = 300;
        }     
        private void setQualityHigh()
        {
            _m_bOpenShadow = true;//是否开启阴影
            _m_bOpenShowcaseShadow = true;
            _m_bOpenPostProcess = false;//是否开启后效
            _m_bOpenClothPhysics = true;
            _m_iFurPassCount = 15;
            GraphicsSettings.renderPipelineAsset = Game.instance.mainCamera.urpSetting.highRenderPipelineAsset;
            QualitySettings.renderPipeline = Game.instance.mainCamera.urpSetting.highRenderPipelineAsset;
            _updateURPSetting();
            Shader.EnableKeyword(KeyWords_OPEN_SHARPEN_USM);
            Shader.globalMaximumLOD = 600;
        }
        private void setQualityUltra()
        {
            _m_bOpenShadow = true;//是否开启阴影
            _m_bOpenShowcaseShadow = true;
            _m_bOpenPostProcess = false;//是否开启后效
            _m_bOpenClothPhysics = true;
            _m_iFurPassCount = 15;
            GraphicsSettings.renderPipelineAsset = Game.instance.mainCamera.urpSetting.ultraRenderPipelineAsset;
            QualitySettings.renderPipeline = Game.instance.mainCamera.urpSetting.ultraRenderPipelineAsset;
            _updateURPSetting();
            Shader.EnableKeyword(KeyWords_OPEN_SHARPEN_USM);
            Shader.globalMaximumLOD = 600;
        }

        /// <summary>
        /// 刷新更新urp设置后需要更新的设置
        /// </summary>
        private void _updateURPSetting()
        {
        }

        private void updateQualitySetting()
        {
            // MagicaPhysicsManager.Instance.enabled = _m_bOpenClothPhysics;
        }
        
        
        private void _setGraphicSetting()
        {
            bool supportInstance = SystemInfo.supportsInstancing;

            // 由于模拟器 给到的是否支持Instancing的数据是错的，
            // 另外由于ShaderModel 3.5的机器支持Instancing的比较少，所以这里直接判断shaderModel 3.5及以下的都不开启Instancing，确保显示正常
            if (SystemInfo.graphicsShaderLevel <= 35)
            {
                supportInstance = false;
            }

            _m_openInstancing = supportInstance;
        }

        public void openPostProcessing()
        {
            // MJUniversalRenderAPI.g_OpenPostProcessing = true;
            if (UniversalRenderPipeline.asset.volumeFrameworkUpdateMode == VolumeFrameworkUpdateMode.ViaScripting)
            {
                CameraController.instance.controlCamera.UpdateVolumeStack();
                RTMainCameraController.instance.controlCamera.UpdateVolumeStack();
            }
        }

        public void closePostProcessing()
        {
            // MJUniversalRenderAPI.g_OpenPostProcessing = false;
            CameraController.instance.controlCamera.UpdateVolumeStack();
            RTMainCameraController.instance.controlCamera.UpdateVolumeStack();
        }
    }
}