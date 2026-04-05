namespace GOE
{
    public abstract class _ACameraOrthographicSizeController
    {
        private bool _m_isMovingDone;
        
        public bool isMovingDone { get { return _m_isMovingDone; } }

        public void start(float _cameraOrthographicSize)
        {
            _m_isMovingDone = false;
            onStart(_cameraOrthographicSize);
        }
        
        public abstract float updateOrthographicSize();

        protected abstract void onStart(float _cameraOrthographicSize);
        protected void setMovingDone()
        {
            _m_isMovingDone = true;
        }
    }
}