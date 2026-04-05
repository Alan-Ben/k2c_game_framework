using UnityEngine;

namespace GOE
{
    public class CameraMoveInertiaController : _ACameraMoveController
    {
        private readonly float _m_airFriction;
        private readonly float _m_slideFriction;
        private readonly Vector3 _m_originVelocityDirection;
        private readonly float _m_originVelocityValue;

        private Vector3 _m_cameraPos;
        private Vector3 _m_cameraPosAfterLimitation;
        private Vector3 _m_velocityDirection;
        private float _m_velocityValue;

        public CameraMoveInertiaController(Vector3 _velocity, float _airFriction, float _slideFriction)
        {
            _m_originVelocityDirection = _m_velocityDirection = _velocity.normalized;
            _m_originVelocityValue = _m_velocityValue = _velocity.magnitude;
            _m_airFriction = _airFriction;
            _m_slideFriction = _slideFriction;
        }
        public CameraMoveInertiaController(Vector3 _direction, float _velocity, float _airFriction, float _slideFriction)
        {
            _m_originVelocityDirection = _m_velocityDirection = _direction.normalized;
            _m_originVelocityValue = _m_velocityValue = _velocity;
            _m_airFriction = _airFriction;
            _m_slideFriction = _slideFriction;
        }

        protected override void onStart(Vector3 _cameraPos, Vector3 _focusPos, Vector3 _cameraPosBeforeLimitation, Vector3 _focusPosBeforeLimitation, _APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            _m_cameraPos = _cameraPosBeforeLimitation;
            _m_cameraPosAfterLimitation = _cameraPos;
            _m_velocityDirection = _m_originVelocityDirection;
            _m_velocityValue = _m_originVelocityValue;
        }
        
        public override Vector3 updateCameraPos(_APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            _m_velocityValue += (-_m_airFriction * _m_velocityValue * _m_velocityValue - _m_slideFriction) * Time.unscaledDeltaTime;
            if (_m_velocityValue <= 0)
            {
                setMovingDone();
                return _m_cameraPos;
            }

            Vector3 offset = _m_velocityDirection * (_m_velocityValue * Time.unscaledDeltaTime);
            _m_cameraPos += offset;
            _m_cameraPosAfterLimitation += offset;
            if (_posLimiter != null)
                _onCameraPosBeLimited(_posLimiter.limitPos(_m_cameraPos, out bool _));
            return _m_cameraPos;
        }

        private void _onCameraPosBeLimited(Vector3 _limitedCameraPos)
        {
            if (_limitedCameraPos == _m_cameraPos)
            {
                setMovingDone();
                return;
            }
            
            // 计算出遭到限制的移动方向
            Vector3 directionLimited = (_limitedCameraPos - _m_cameraPosAfterLimitation).normalized;            
            // 消除掉被限制的移动方向
            float perspectiveDirection = Vector3.Dot(directionLimited, _m_velocityDirection);
            if (perspectiveDirection < 0)
            {
                _m_velocityDirection -= perspectiveDirection * directionLimited;
                _m_velocityValue *= _m_velocityDirection.magnitude;
                _m_velocityDirection.Normalize();
            }

            _m_cameraPosAfterLimitation = _limitedCameraPos;
        }
    }
}