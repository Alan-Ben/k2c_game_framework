
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 相机透视下的视野范围以 EaseType 的曲线改变到目标值的控制器
    /// </summary>
    public class CameraFieldOfViewEaseController : _ACameraFieldOfViewController
    {
        private readonly float _m_targetValue;
        private readonly EaseType _m_easeType;
        private readonly float _m_totalTime;
        private readonly Action _m_complete;
        
        private float _m_currentTime;
        private float _m_startValue;

        public CameraFieldOfViewEaseController(float _targetValue, EaseType _easeType, float _totalTime, Action _complete = null)
        {
            _m_targetValue = _targetValue;
            _m_easeType = _easeType;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }
        public CameraFieldOfViewEaseController(float _targetValue, float _totalTime, Action _complete = null)
        {
            _m_targetValue = _targetValue;
            _m_easeType = GGameCommonInfo.instance.obj == null ? EaseType.InOutQuad : GGameCommonInfo.instance.obj.defaultValueEaseType;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }

        protected override void onStart(float _cameraFieldOfView)
        {
            _m_startValue = _cameraFieldOfView;
            _m_currentTime = 0;
        }
        public override float updateFieldOfView()
        {
            _m_currentTime += Time.deltaTime;
            if (_m_currentTime >= _m_totalTime)
            {
                _m_currentTime = _m_totalTime;
                setMovingDone();
                ALCommonActionMonoTask.addMonoTask(_m_complete);

                return _m_targetValue;
            }

            return Mathf.Lerp(_m_startValue, _m_targetValue, _m_easeType.evaluate(_m_currentTime, _m_totalTime));
        }
    }
}