
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 以 EaseType 的曲线实现相机坐标移动
    /// </summary>
    public class CameraPosEaseController : _ACameraPosController
    {
        private readonly Vector3 _m_targetPos;
        private readonly EaseType _m_easeType;
        private readonly float _m_totalTime;
        private readonly Action _m_complete;
        
        private Vector3 _m_finalTargetPos;
        private float _m_currentTime;
        private Vector3 _m_startPos;

        public CameraPosEaseController(Vector3 _targetPos, EaseType _easeType, float _totalTime, Action _complete = null)
        {
            _m_targetPos = _targetPos;
            _m_easeType = _easeType;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }
        public CameraPosEaseController(Vector3 _targetPos, float _totalTime, Action _complete = null)
        {
            _m_targetPos = _targetPos;
            _m_easeType = GGameCommonInfo.instance.obj == null ? EaseType.InOutQuad : GGameCommonInfo.instance.obj.defaultValueEaseType;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }

        protected override void onStart(Vector3 _cameraPos, Vector3 _cameraPosBeforeLimitation, _APosLimiter _posLimiter)
        {
            _m_startPos = _cameraPosBeforeLimitation;
            
            if (_posLimiter != null && !_posLimiter.isPosInStableArea(_m_targetPos, out Vector3 stablePos))
                _m_finalTargetPos = stablePos;
            else
                _m_finalTargetPos = _m_targetPos;
            
            _m_currentTime = 0;
        }

        public override Vector3 updateCameraPos(_APosLimiter _posLimiter)
        {
            _m_currentTime += Time.deltaTime;
            if (_m_currentTime >= _m_totalTime)
            {
                _m_currentTime = _m_totalTime;
                setMovingDone();
                ALCommonActionMonoTask.addMonoTask(_m_complete);

                return _m_finalTargetPos;
            }

            return Vector3.Lerp(_m_startPos, _m_finalTargetPos, _m_easeType.evaluate(_m_currentTime, _m_totalTime));
        }
    }
}