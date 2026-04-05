using UnityEngine;

namespace GOE
{
    public abstract class _ACameraFocusPosController
    {
        private bool _m_isMovingDone;
        
        public bool isMovingDone { get { return _m_isMovingDone; } }
        
        public void start(Vector3 _cameraFocusPos, Vector3 _cameraFocusPosBeforeLimitation, _APosLimiter _focusPoseLimiter)
        {
            _m_isMovingDone = false;
            onStart(_cameraFocusPos, _cameraFocusPosBeforeLimitation, _focusPoseLimiter);
        }
        public abstract Vector3 updateFocusPos(_APosLimiter _focusPoseLimiter);
        
        protected abstract void onStart(Vector3 _cameraFocusPos, Vector3 _cameraFocusPosBeforeLimitation, _APosLimiter _focusPoseLimiter);
        protected void setMovingDone()
        {
            _m_isMovingDone = true;
        }
    }
}