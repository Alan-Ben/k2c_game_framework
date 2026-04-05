using ALPackage;
using UnityEngine;

namespace GOE
{
    public interface _IShakable
    {
        int taskSerialize { get; }
        RectTransform shakeRoot { get; }
        Vector3 shakeOriginPos { get; }
    }

    /// <summary>
    /// 在Update里使用的一个震动任务
    /// </summary>
    public class ShakeTask : _IALBaseMonoTask
    {
        private _IShakable _m_iInstance;
        private int _m_iSerialize;
        private float _m_fDuration;
        private float _m_fShakeTime;
        private float _m_fIntensity;
        private bool _m_bUseUnscaledTime;

        public ShakeTask(_IShakable _instance, float _shakeIntensity, float _shakeDuration, bool _useUnscaledTime)
        {
            _m_iInstance = _instance;
            if (_instance != null)
                _m_iSerialize = _instance.taskSerialize;
            _m_fShakeTime = _m_fDuration = _shakeDuration;
            _m_fIntensity = _shakeIntensity;
            _m_bUseUnscaledTime = _useUnscaledTime;
        }
        
        public void deal()
        {
            if (_m_iInstance == null || _m_iSerialize != _m_iInstance.taskSerialize)
                return;

            onUpdate();

            if (isTaskEnd())
            {
                //重置下数据
                if (_m_iInstance != null && null != _m_iInstance.shakeRoot) 
                    _m_iInstance.shakeRoot.anchoredPosition = _m_iInstance.shakeOriginPos;
                return;
            }

            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        protected void onUpdate()
        {
            if(null == _m_iInstance || null == _m_iInstance.shakeRoot)
                return;
            
            Vector3 shakeValue = Random.insideUnitSphere * _m_fIntensity;
            shakeValue = Vector3.Lerp(Vector3.zero, shakeValue, _m_fShakeTime / _m_fDuration);
            _m_iInstance.shakeRoot.anchoredPosition = _m_iInstance.shakeOriginPos + shakeValue;
            _m_fShakeTime -= _getDeltaTime();            
        }

        protected bool isTaskEnd()
        {
            return _m_fShakeTime <= 0;
        }

        private float _getDeltaTime()
        {
            return _m_bUseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        }
    }
}