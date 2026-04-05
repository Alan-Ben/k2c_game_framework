namespace GOE
{
    public abstract class _ACameraFieldOfViewController
    {
        private bool _m_isMovingDone;
        
        public bool isMovingDone { get { return _m_isMovingDone; } }
        
        public void start(float _cameraFieldOfView)
        {
            _m_isMovingDone = false;
            onStart(_cameraFieldOfView);
        }
        public abstract float updateFieldOfView();

        protected abstract void onStart(float _cameraFieldOfView);
        protected void setMovingDone()
        {
            _m_isMovingDone = true;
        }
    }
}