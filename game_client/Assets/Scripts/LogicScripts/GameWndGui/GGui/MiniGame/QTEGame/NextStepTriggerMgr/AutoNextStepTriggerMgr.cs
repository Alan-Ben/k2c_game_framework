using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class AutoNextStepTriggerTask : _IALBaseMonoTask
    {
        private NextStepTrigger _m_tTrigger;
        private Action<NextStepTrigger> _m_aOnTrigger;
        private bool _m_bIsEnable;
        
        public AutoNextStepTriggerTask(NextStepTrigger _trigger, Action<NextStepTrigger> _onTrigger)
        {
            _m_tTrigger = _trigger;
            _m_aOnTrigger = _onTrigger;

            _m_bIsEnable = false;
        }

        public bool isEnable()
        {
            return _m_bIsEnable;
        }
        
        public void deal()
        {
            if (!_m_bIsEnable)
                return;
            
            _m_bIsEnable = false;
            
            _m_aOnTrigger?.Invoke(_m_tTrigger);
        }

        public void startTask()
        {
            _m_bIsEnable = true;
            
            ALMonoTaskMgr.instance.addMonoTask(this, _m_tTrigger?.autoTriggerDelayTime ?? 0);
        }

        public void discard()
        {
            _m_bIsEnable = false;
        }
    }
    
    public class AutoNextStepTriggerMgr : _ANextStepTriggerMgr
    {
        [NotNull] private List<AutoNextStepTriggerTask> _m_lTaskList = new List<AutoNextStepTriggerTask>();
        
        public AutoNextStepTriggerMgr(Action _onTrigger) : base(_onTrigger)
        {
        }

        public override ENextStepTriggerType triggerType { get { return ENextStepTriggerType.AUTO; } }
        protected override void _onStartMonitor()
        {
            for (int i = 0; i < _m_lTaskList.Count; i++)
            {
                AutoNextStepTriggerTask task = _m_lTaskList[i];
                if(task != null)
                    task.startTask();
            }
        }

        protected override void _onReset()
        {
            for (int i = 0; i < _m_lTaskList.Count; i++)
            {
                AutoNextStepTriggerTask task = _m_lTaskList[i];
                if(task != null)
                    task.discard();
            }
            
            _m_lTaskList.Clear();
        }

        protected override bool _regNextStepTrigger(NextStepTrigger _trigger)
        {
            // 由于在基类已经判断过触发器类型, 所以这里不再判断
            
            _m_lTaskList.Add(new AutoNextStepTriggerTask(_trigger, _onTaskTrigger));

            return true;
        }

        private void _onTaskTrigger(NextStepTrigger _trigger)
        {
            _dealTrigger(_trigger);
        }

        public override bool allTriggerDone()
        {
            if (!_m_bIsStartMonitor)
                return false;

            if (_m_lTaskList.Count <= 0)
                return true;

            foreach (var task in _m_lTaskList)
            {
                if (task != null && task.isEnable())
                    return false;
            }
            
            return true;
        }
    }
}