using System;
using JetBrains.Annotations;

namespace GOE
{
    public abstract class _ANextStepTriggerMgr
    {
        public abstract ENextStepTriggerType triggerType { get; }

        protected bool _m_bIsStartMonitor;//是否开始监听
        protected Action _m_aOnTrigger;

        public _ANextStepTriggerMgr(Action _onTrigger)
        {
            _m_aOnTrigger = _onTrigger;
        }
        
        public void startMonitor()
        {
            _onStartMonitor();
            
            _m_bIsStartMonitor = true;
        }

        public void reset()
        {
            _m_bIsStartMonitor = false;
            
            _onReset();
        }

        /// <summary>
        /// 注册下一步触发器
        /// </summary>
        /// <param name="_trigger"></param>
        public bool regNextStepTrigger(NextStepTrigger _trigger)
        {
            if (_trigger == null || _trigger.triggerType != triggerType)
                return false;
            
            if (_m_bIsStartMonitor)
            {
                Debug.LogError($"[_ANextStepTriggerMgr<{triggerType}> regNextStepTrigger] 在已经开始监听后还在注册触发器");
                return false;
            }
            
            return _regNextStepTrigger(_trigger);
        }
        
        protected virtual void _dealTrigger(NextStepTrigger _trigger)
        {
            if (_trigger != null)
            {
                if (!string.IsNullOrEmpty(_trigger.triggerFunc))
                {
                    //触发后执行效果
                    _NPPlayerEffectSerializeInfo effectSerializeInfo = _NPPlayerEffectSerializeInfo.ReadFromString(_trigger.triggerFunc);
                    effectSerializeInfo?.dealEffect();
                }
            }
            
            _m_aOnTrigger?.Invoke();
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        protected abstract void _onStartMonitor();

        /// <summary>
        /// 重置
        /// </summary>
        protected abstract void _onReset();
        
        /// <summary>
        /// 注册下一步触发器
        /// </summary>
        /// <param name="_trigger"></param>
        protected abstract bool _regNextStepTrigger([NotNull] NextStepTrigger _trigger);

        /// <summary>
        /// 是否所有触发器都触发了
        /// </summary>
        public abstract bool allTriggerDone();
    }
}