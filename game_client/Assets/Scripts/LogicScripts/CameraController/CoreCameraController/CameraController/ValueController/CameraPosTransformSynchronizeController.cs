using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 使相机的 position 值和 targetTransform 的 position 值保持一致
    /// </summary>
    public class CameraPosTransformSynchronizeController : _ACameraPosController
    {
        private Transform _m_targetTransform;
        private Vector3 _m_lastCameraPos;


        public CameraPosTransformSynchronizeController(Transform _targetTransform)
        {
            _m_targetTransform = _targetTransform;
        }
        

        protected override void onStart(Vector3 _cameraPos, Vector3 _cameraPosBeforeLimitation, _APosLimiter _posLimiter)
        {
            _m_lastCameraPos = _cameraPosBeforeLimitation;
        }
        public override Vector3 updateCameraPos(_APosLimiter _posLimiter)
        {
            if (_m_targetTransform == null)
                return _m_lastCameraPos;

            _m_lastCameraPos = _m_targetTransform.position;
            return _m_lastCameraPos;
        }

        public new void setMovingDone()
        {
            base.setMovingDone();
        }
    }
}