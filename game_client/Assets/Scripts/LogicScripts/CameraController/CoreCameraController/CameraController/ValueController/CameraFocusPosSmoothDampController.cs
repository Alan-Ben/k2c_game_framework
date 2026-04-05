
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 相机焦点以平滑阻尼算法移动到目标点的控制器
    /// </summary>
    public class CameraFocusPosSmoothDampController : _ACameraFocusPosController
    {
        private readonly Vector3 _m_targetPos;
        private readonly float _m_smoothTime;

        private Vector3 _m_finalTargetPos;
        private Vector3 _m_focusPos;
        private Vector3 _m_smoothDampVelocity;

        public CameraFocusPosSmoothDampController(Vector3 _targetPos, float _smoothTime)
        {
            _m_targetPos = _targetPos;
            _m_smoothTime = _smoothTime;
        }
        public CameraFocusPosSmoothDampController(Vector3 _targetPos)
        {
            _m_targetPos = _targetPos;
            _m_smoothTime = GGameCommonInfo.instance.obj == null ? 0.3f : GGameCommonInfo.instance.obj.defaultSmoothDampTime;
        }

        protected override void onStart(Vector3 _cameraFocusPos, Vector3 _cameraFocusPosBeforeLimitation, _APosLimiter _focusPoseLimiter)
        {
            _m_focusPos = _cameraFocusPosBeforeLimitation;
            if (_focusPoseLimiter != null && !_focusPoseLimiter.isPosInStableArea(_m_targetPos, out Vector3 stablePos))
                _m_finalTargetPos = stablePos;
            else
                _m_finalTargetPos = _m_targetPos;
            _m_smoothDampVelocity = Vector3.zero;   
        }
        public override Vector3 updateFocusPos(_APosLimiter _focusPoseLimiter)
        {
            _m_focusPos = Vector3.SmoothDamp(_m_focusPos, _m_finalTargetPos, ref _m_smoothDampVelocity, _m_smoothTime, float.PositiveInfinity, Time.deltaTime);
            return _m_focusPos;
        }
    }
}