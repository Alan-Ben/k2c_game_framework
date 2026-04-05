
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class CameraOrthographicSizeALFadeController : _ACameraOrthographicSizeController
    {
        private readonly float _m_targetValue;
        private readonly float _m_accTime;
        private readonly float _m_totalTime;
        private readonly Action _m_complete;

        private ALRealTimeFloatFadeController _m_fadeValueController;

        public CameraOrthographicSizeALFadeController(float _targetValue, float _totalTime, float _accTime = 0f, Action _complete = null)
        {
            _m_targetValue = _targetValue;
            _m_accTime = _accTime;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }

        protected override void onStart(float _cameraOrthographicSize)
        {
            _m_fadeValueController = new ALRealTimeFloatFadeController(_cameraOrthographicSize, _m_targetValue, 0, _m_totalTime, _m_accTime);
        }
        public override float updateOrthographicSize()
        {
            if (_m_fadeValueController == null)
            {
                ALLog.Error("[** Camera **] OrthographicSize 的 ALFadeController 出现错误，内部生成 fadeController 失败。");
                return 10;
            }
            
            if (_m_fadeValueController.isDone)
            {
                setMovingDone();
                ALCommonActionMonoTask.addMonoTask(_m_complete);
            }
            
            return _m_fadeValueController.curValue;
        }
    }
}