
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class CameraMoveToFocusPointALFadeController : _ACameraMoveController
    {
        private readonly Vector3 _m_targetPos;
        private readonly ESceneMoveType _m_moveType;
        private readonly float _m_accTime;
        private readonly float _m_totalTime;
        private readonly Action _m_complete;
        
        private Vector3 _m_finalTargetPos;
        private ALRealTimeFloatFadeController _m_fadeValueControllerX;
        private ALRealTimeFloatFadeController _m_fadeValueControllerY;
        private ALRealTimeFloatFadeController _m_fadeValueControllerZ;
        private bool _m_isDone;

        public CameraMoveToFocusPointALFadeController(Vector3 _targetPos, ESceneMoveType _moveType, float _totalTime, float _accTime = 0f, Action _complete = null)
        {
            _m_targetPos = _targetPos;
            _m_moveType = _moveType;
            _m_accTime = _accTime;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }

        protected override void onStart(Vector3 _cameraPos, Vector3 _focusPos, Vector3 _cameraPosBeforeLimitation, Vector3 _focusPosBeforeLimitation, _APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
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
            
            _m_fadeValueControllerX = new ALRealTimeFloatFadeController(_cameraPosBeforeLimitation.x, _m_finalTargetPos.x, 0, _m_totalTime, _m_accTime);
            _m_fadeValueControllerY = new ALRealTimeFloatFadeController(_cameraPosBeforeLimitation.y, _m_finalTargetPos.y, 0, _m_totalTime, _m_accTime);
            _m_fadeValueControllerZ = new ALRealTimeFloatFadeController(_cameraPosBeforeLimitation.z, _m_finalTargetPos.z, 0, _m_totalTime, _m_accTime);
            _m_isDone = false;
        }
        public override Vector3 updateCameraPos(_APosLimiter _posLimiter, _APosLimiter _focusPosLimiter)
        {
            if (_m_fadeValueControllerX == null ||
                _m_fadeValueControllerY == null ||
                _m_fadeValueControllerZ == null)
            {
                ALLog.Error("[** Camera **] CameraPos 的 ALFadeController 出现错误，内部生成 fadeController 失败。");
                return Vector3.zero;
            }
            
            if (!_m_isDone &&
                _m_fadeValueControllerX.isDone &&
                _m_fadeValueControllerY.isDone &&
                _m_fadeValueControllerZ.isDone)
            {
                _m_isDone = true;
                setMovingDone();
                ALCommonActionMonoTask.addMonoTask(_m_complete);
            }
            
            return new Vector3(
                _m_fadeValueControllerX.curValue,
                _m_fadeValueControllerY.curValue,
                _m_fadeValueControllerZ.curValue);
        }
    }
}