
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 使相机的 rotation 值和 targetTransform 的 rotation 值保持一致
    /// </summary>
    public class CameraFocusPosTransformSynchronizeController : _ACameraFocusPosController
    {
        private Transform _m_targetTransform;
        private Vector3 _m_lastFocusPos;
        
        
        public CameraFocusPosTransformSynchronizeController(Transform _targetTransform)
        {
            _m_targetTransform = _targetTransform;
        }
        
        
        protected override void onStart(Vector3 _cameraFocusPos, Vector3 _cameraFocusPosBeforeLimitation, _APosLimiter _focusPoseLimiter)
        {
            _m_lastFocusPos = _cameraFocusPosBeforeLimitation;
        }
        public override Vector3 updateFocusPos(_APosLimiter _focusPoseLimiter)
        {
            if (_m_targetTransform == null)
                return _m_lastFocusPos;
            
            Vector3 cameraPos = CameraController.instance.cameraPos;
            _m_lastFocusPos = cameraPos + _m_targetTransform.rotation * Vector3.forward;
            if (_focusPoseLimiter is SoftBorder2DPosLimiter { posGetter: not null } softBorder2DPosLimiter &&
                softBorder2DPosLimiter.posGetter.intersectionWorldPointWithWorldRay(new Ray(cameraPos, _m_lastFocusPos - cameraPos), out Vector3 intersectionWorldPoint))
            {
                _m_lastFocusPos = intersectionWorldPoint;
            }
            return _m_lastFocusPos;
        }

        public new void setMovingDone()
        {
            base.setMovingDone();
        }
    }
}
