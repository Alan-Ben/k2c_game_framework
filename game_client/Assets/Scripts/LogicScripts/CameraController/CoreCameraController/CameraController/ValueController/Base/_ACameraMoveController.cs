using UnityEngine;

namespace GOE
{
    public abstract class _ACameraMoveController
    {
        private bool _m_isMovingDone;

        public bool isMovingDone { get { return _m_isMovingDone; } }

        public void start(Vector3 _cameraPos, Vector3 _focusPos, Vector3 _cameraPosBeforeLimitation, Vector3 _focusPosBeforeLimitation, _APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            _m_isMovingDone = false;
            onStart(_cameraPos, _focusPos, _cameraPosBeforeLimitation, _focusPosBeforeLimitation, _posLimiter, _focusPosLimiter);
        }
        
        public abstract Vector3 updateCameraPos(_APosLimiter _posLimiter, _APosLimiter _focusPosLimiter);
        
        protected abstract void onStart(Vector3 _cameraPos, Vector3 _focusPos, Vector3 _cameraPosBeforeLimitation, Vector3 _focusPosBeforeLimitation, _APosLimiter _posLimiter, _APosLimiter _focusPosLimiter);
        protected void setMovingDone()
        {
            _m_isMovingDone = true;
        }
    }
}