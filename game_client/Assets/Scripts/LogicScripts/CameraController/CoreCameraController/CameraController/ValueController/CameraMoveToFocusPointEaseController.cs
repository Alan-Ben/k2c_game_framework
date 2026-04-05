using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class CameraMoveToFocusPointEaseController : _ACameraMoveController
    {
        private readonly Vector3 _m_targetPos;
        private readonly ESceneMoveType _m_moveType;
        private readonly EaseType _m_easeType;
        private readonly float _m_totalTime;
        private readonly Action _m_complete;

        private Vector3 _m_finalTargetPos;
        private float _m_currentTime;
        private Vector3 _m_startPos;
        private bool _m_isDone;

        public CameraMoveToFocusPointEaseController(Vector3 _targetPos, ESceneMoveType _moveType, EaseType _easeType, float _totalTime, Action _complete = null)
        {
            _m_targetPos = _targetPos;
            _m_moveType = _moveType;
            _m_easeType = _easeType;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }
        public CameraMoveToFocusPointEaseController(Vector3 _targetPos, ESceneMoveType _moveType, float _totalTime, Action _complete = null)
        {
            _m_targetPos = _targetPos;
            _m_moveType = _moveType;
            _m_easeType = GGameCommonInfo.instance.obj == null ? EaseType.InOutQuad : GGameCommonInfo.instance.obj.defaultValueEaseType;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }

        protected override void onStart(Vector3 _cameraPos, Vector3 _focusPos, Vector3 _cameraPosBeforeLimitation, Vector3 _focusPosBeforeLimitation, _APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            _m_startPos = _cameraPosBeforeLimitation;
            Vector3 goalPos = _m_moveType switch
            {
                ESceneMoveType.XY => NPGameUtility.getNewCameraPosKeepHeightAndDirectionToSeeTargetXY(_cameraPosBeforeLimitation,
                    _focusPosBeforeLimitation, _m_targetPos, out _),
                ESceneMoveType.XZ => NPGameUtility.getNewCameraPosKeepHeightAndDirectionToSeeTargetXZ(_cameraPosBeforeLimitation,
                    _focusPosBeforeLimitation, _m_targetPos, out _),
                ESceneMoveType.YZ => NPGameUtility.getNewCameraPosKeepHeightAndDirectionToSeeTargetYZ(_cameraPosBeforeLimitation,
                    _focusPosBeforeLimitation, _m_targetPos, out _),
                _ => NPGameUtility.getNewCameraPosKeepHeightAndDirectionToSeeTargetXZ(_cameraPosBeforeLimitation, _focusPosBeforeLimitation,
                    _m_targetPos, out _)
            };
            
            if (_posLimiter != null && !_posLimiter.isPosInStableArea(goalPos, out Vector3 stablePos))
                _m_finalTargetPos = stablePos;
            else
                _m_finalTargetPos = goalPos;
            
            _m_currentTime = 0;
            _m_isDone = false;
        }
        public override Vector3 updateCameraPos(_APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            _m_currentTime += Time.deltaTime;
            if (_m_currentTime >= _m_totalTime)
            {
                _m_currentTime = _m_totalTime;
                if (!_m_isDone)
                {
                    _m_isDone = true;
                    setMovingDone();
                    ALCommonActionMonoTask.addMonoTask(_m_complete);
                }

                return _m_finalTargetPos;
            }

            return Vector3.Lerp(_m_startPos, _m_finalTargetPos, _m_easeType.evaluate(_m_currentTime, _m_totalTime));
        }
    }
}