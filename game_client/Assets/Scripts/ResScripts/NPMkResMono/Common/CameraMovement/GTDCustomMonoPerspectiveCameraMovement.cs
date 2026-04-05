using ALPackage;

namespace GOE
{
    /// <summary>
    /// 透视相机的自定义移动
    /// </summary>
    public class GTDCustomMonoPerspectiveCameraMovement : GTDCustomMonoCameraMovement
#if NP_GAME
        , _ICameraFieldOfViewSynchronizeTarget
#endif
    {
        [ALHeader("相机的 FOV 尺寸")]
        public float fieldOfView = 60;
        
        
#if NP_GAME
        float _ICameraFieldOfViewSynchronizeTarget.fieldOfView { get { return fieldOfView; } }


        private bool _m_setOriginValueWhenExit;
        private float _m_originFieldOfView;
        private CameraFieldOfViewSynchronizeController _m_fieldOfViewController;
        
        protected override void _enableMovement()
        {
            _m_setOriginValueWhenExit = setOriginValueWhenExit;
            
            if (_m_setOriginValueWhenExit)
                _m_originFieldOfView = CameraController.instance.cameraFieldOfView;
            
            _m_fieldOfViewController = new CameraFieldOfViewSynchronizeController(this);
            CameraController.instance.setCameraFieldOfViewController(_m_fieldOfViewController);

            base._enableMovement();
        }
        protected override void _disableMovement()
        {
            if (_m_setOriginValueWhenExit)
            {
                if (CameraController.instance.isFieldOfViewControllerEnable(_m_fieldOfViewController))
                    CameraController.instance.cameraFieldOfView = _m_originFieldOfView;
            }
            else
                _m_fieldOfViewController?.setMovingDone();

            _m_fieldOfViewController = null;

            base._disableMovement();
        }
#endif
    }
}