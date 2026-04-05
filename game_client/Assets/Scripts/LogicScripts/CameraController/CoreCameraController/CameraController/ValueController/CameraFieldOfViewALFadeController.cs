using System;
using ALPackage;

namespace GOE
{
    public class CameraFieldOfViewALFadeController : _ACameraFieldOfViewController
    {
        private readonly float _m_targetValue;
        private readonly float _m_accTime;
        private readonly float _m_totalTime;
        private readonly Action _m_complete;

        private ALRealTimeFloatFadeController _m_fadeValueController;

        public CameraFieldOfViewALFadeController(float _targetValue, float _totalTime, float _accTime = 0f, Action _complete = null)
        {
            _m_targetValue = _targetValue;
            _m_accTime = _accTime;
            _m_totalTime = _totalTime;
            _m_complete = _complete;
        }

        protected override void onStart(float _cameraFieldOfView)
        {
            _m_fadeValueController = new ALRealTimeFloatFadeController(_cameraFieldOfView, _m_targetValue, 0, _m_totalTime, _m_accTime);
        }
        public override float updateFieldOfView()
        {
            if (_m_fadeValueController == null)
            {
                ALLog.Error("[** Camera **] FOV 的 ALFadeController 出现错误，内部生成 fadeController 失败。");
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