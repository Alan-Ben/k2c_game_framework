
using UnityEngine;

namespace GOE
{
    public abstract class _ACameraPosController
    {
        private bool _m_isMovingDone;
        
        public bool isMovingDone { get { return _m_isMovingDone; } }

        public void start(Vector3 _cameraPos, Vector3 _cameraPosBeforeLimitation, _APosLimiter _posLimiter)
        {
            _m_isMovingDone = false;
            onStart(_cameraPos, _cameraPosBeforeLimitation, _posLimiter);
        }
        
        public abstract Vector3 updateCameraPos(_APosLimiter _posLimiter);

        protected abstract void onStart(Vector3 _cameraPos, Vector3 _cameraPosBeforeLimitation, _APosLimiter _posLimiter);
        protected void setMovingDone()
        {
            _m_isMovingDone = true;
        }
    }
}