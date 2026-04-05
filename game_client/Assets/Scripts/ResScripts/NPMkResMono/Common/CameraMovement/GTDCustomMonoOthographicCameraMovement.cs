using ALPackage;

namespace GOE
{
    /// <summary>
    /// 正交相机的自定义移动
    /// </summary>
    public class GTDCustomMonoOthographicCameraMovement : GTDCustomMonoCameraMovement
#if NP_GAME
        , _ICameraOrthographicSizeSynchronizeTarget
#endif
    {
        [ALHeader("正交相机的尺寸")]
        public float orthographicSize = 5;


#if NP_GAME
        float _ICameraOrthographicSizeSynchronizeTarget.orthographicSize { get { return orthographicSize; } }
        
        
        private bool _m_setOriginValueWhenExit;
        private float _m_originOrthographicSize;
        private CameraOrthographicSizeSynchronizeController _m_orthographicSizeController;
        
        protected override void _enableMovement()
        {
            _m_setOriginValueWhenExit = setOriginValueWhenExit;
            
            if (_m_setOriginValueWhenExit)
                _m_originOrthographicSize = CameraController.instance.cameraOrthographicSize;
            
            _m_orthographicSizeController = new CameraOrthographicSizeSynchronizeController(this);
            CameraController.instance.setCameraOrthographicSizeController(_m_orthographicSizeController);

            base._enableMovement();
        }
        
        protected override void _disableMovement()
        {
            if (_m_setOriginValueWhenExit)
            {
                if (CameraController.instance.isOrthographicSizeControllerEnable(_m_orthographicSizeController))
                    CameraController.instance.cameraOrthographicSize = _m_originOrthographicSize;
            }
            else
                _m_orthographicSizeController?.setMovingDone();
            
            _m_orthographicSizeController = null;
            
            base._disableMovement();
        }
#endif
    }
}