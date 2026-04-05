namespace GOE
{
    /// <summary>
    /// 使相机的 fieldOfView 值和 target 的 fieldOfView 值保持一致
    /// </summary>
    public class CameraFieldOfViewSynchronizeController : _ACameraFieldOfViewController
    {
        private _ICameraFieldOfViewSynchronizeTarget _m_target;
        private float _m_lastFieldOfView;
        
        
        public CameraFieldOfViewSynchronizeController(_ICameraFieldOfViewSynchronizeTarget _target)
        {
            _m_target = _target;
        }
        
        
        protected override void onStart(float _cameraFieldOfView)
        {
            _m_lastFieldOfView = _cameraFieldOfView;
        }
        
        
        public override float updateFieldOfView()
        {
            if (_m_target == null)
                return _m_lastFieldOfView;

            _m_lastFieldOfView = _m_target.fieldOfView;
            return _m_lastFieldOfView;
        }
        
        public new void setMovingDone()
        {
            base.setMovingDone();
        }
    }

    public interface _ICameraFieldOfViewSynchronizeTarget
    {
        float fieldOfView { get; }
    }
}