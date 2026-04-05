
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class CameraFocusPosALFadeController : _ACameraFocusPosController
    {
        private readonly Vector3 _m_targetPos;
        private readonly float _m_accTime;
        private readonly float _m_totalTime;
        private readonly Action _m_complete;

        private Vector3 _m_finalTargetPos;
        private ALRealTimeFloatFadeController _m_fadeValueControllerX;
        private ALRealTimeFloatFadeController _m_fadeValueControllerY;
        private ALRealTimeFloatFadeController _m_fadeValueControllerZ;

        public CameraFocusPosALFadeController(Vector3 _targetPos, float _totalTime, float _accTime = 0f, Action _complete = null)
        {
            _m_targetPos = _targetPos;
            _m_accTime = _accTime;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }

        protected override void onStart(Vector3 _cameraFocusPos, Vector3 _cameraFocusPosBeforeLimitation, _APosLimiter _focusPoseLimiter)
        {
            if (_focusPoseLimiter != null && !_focusPoseLimiter.isPosInStableArea(_m_targetPos, out Vector3 stablePos))
                _m_finalTargetPos = stablePos;
            else
                _m_finalTargetPos = _m_targetPos;
            
            _m_fadeValueControllerX = new ALRealTimeFloatFadeController(_cameraFocusPosBeforeLimitation.x, _m_finalTargetPos.x, 0, _m_totalTime, _m_accTime);
            _m_fadeValueControllerY = new ALRealTimeFloatFadeController(_cameraFocusPosBeforeLimitation.y, _m_finalTargetPos.y, 0, _m_totalTime, _m_accTime);
            _m_fadeValueControllerZ = new ALRealTimeFloatFadeController(_cameraFocusPosBeforeLimitation.z, _m_finalTargetPos.z, 0, _m_totalTime, _m_accTime);
        }

        public override Vector3 updateFocusPos(_APosLimiter _focusPoseLimiter)
        {
            if (_m_fadeValueControllerX == null ||
                _m_fadeValueControllerY == null ||
                _m_fadeValueControllerZ == null)
            {
                ALLog.Error("[** Camera **] FocusPos 的 ALFadeController 出现错误，内部生成 fadeController 失败。");
                return Vector3.zero;
            }
            
            if (_m_fadeValueControllerX.isDone &&
                _m_fadeValueControllerY.isDone &&
                _m_fadeValueControllerZ.isDone)
            {
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