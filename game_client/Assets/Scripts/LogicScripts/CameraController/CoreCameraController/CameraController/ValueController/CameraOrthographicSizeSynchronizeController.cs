namespace GOE
{
    /// <summary>
    /// 使相机的 orthographicSize 值和 target 的 orthographicSize 值保持一致
    /// </summary>
    public class CameraOrthographicSizeSynchronizeController : _ACameraOrthographicSizeController
    {
        private _ICameraOrthographicSizeSynchronizeTarget _m_target;
        private float _m_lastOrthographicSize;
        
        
        public CameraOrthographicSizeSynchronizeController(_ICameraOrthographicSizeSynchronizeTarget _target)
        {
            _m_target = _target;
        }


        public override float updateOrthographicSize()
        {
            if (_m_target == null)
                return _m_lastOrthographicSize;
            
            _m_lastOrthographicSize = _m_target.orthographicSize;
            return _m_lastOrthographicSize;
        }

        protected override void onStart(float _cameraOrthographicSize)
        {
            _m_lastOrthographicSize = _cameraOrthographicSize;
        }

        public new void setMovingDone()
        {
            base.setMovingDone();
        }
    }
    
    public interface _ICameraOrthographicSizeSynchronizeTarget
    {
        float orthographicSize { get; }
    }
}