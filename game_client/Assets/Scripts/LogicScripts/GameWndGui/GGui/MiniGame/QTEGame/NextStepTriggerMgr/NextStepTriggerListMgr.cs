using System;
using JetBrains.Annotations;

namespace GOE
{
    public class NextStepTriggerListMgr
    {
        [NotNull] private ClickNextStepTriggerMgr _m_ClickTriggerMgr;
        [NotNull] private MsgNextStepTriggerMgr _m_MsgTriggerMgr;
        [NotNull] private AutoNextStepTriggerMgr _m_AutoTriggerMgr;
        
        private NextStepTriggerList _m_lTriggerList;
        private Action _m_aOnCanGotoNextStep;

        public NextStepTriggerListMgr()
        {
            _m_ClickTriggerMgr = new ClickNextStepTriggerMgr(_onTrigger);
            _m_MsgTriggerMgr = new MsgNextStepTriggerMgr(_onTrigger);
            _m_AutoTriggerMgr = new AutoNextStepTriggerMgr(_onTrigger);
        }
        
        private void _onTrigger()
        {
            if (_m_lTriggerList == null || !_m_lTriggerList.allTriggerToNextStep || 
                (_m_ClickTriggerMgr.allTriggerDone() && _m_MsgTriggerMgr.allTriggerDone() && _m_AutoTriggerMgr.allTriggerDone())
                )
            {
                Action action = _m_aOnCanGotoNextStep;
                reset();
                
                action?.Invoke();
            }
        }

        public void reset()
        {
            _m_ClickTriggerMgr.reset();
            _m_MsgTriggerMgr.reset();
            _m_AutoTriggerMgr.reset();

            _m_lTriggerList = null;
            _m_aOnCanGotoNextStep = null;
        }
        
        public void setTriggerList(NextStepTriggerList _triggerList, Action _onCanGotoNextStep)
        {
            reset();

            if (_triggerList == null || _triggerList.triggerList == null || _triggerList.triggerList.Count <= 0)
            {
                _onCanGotoNextStep?.Invoke();
                return;
            }
            
            _m_lTriggerList = _triggerList;
            _m_aOnCanGotoNextStep = _onCanGotoNextStep;

            int triggerCount = 0;
            
            foreach (var trigger in _triggerList.triggerList)
            {
                if(trigger == null)
                    continue;
                
                switch (trigger.triggerType)
                {
                    case ENextStepTriggerType.CLICK:
                        if (_m_ClickTriggerMgr.regNextStepTrigger(trigger))
                            triggerCount++;
                        break;
                    case ENextStepTriggerType.MSG:
                        if (_m_MsgTriggerMgr.regNextStepTrigger(trigger))
                            triggerCount++;
                        break;
                    case ENextStepTriggerType.AUTO:
                        if (_m_AutoTriggerMgr.regNextStepTrigger(trigger))
                            triggerCount++;
                        break;
                }
            }

            if (triggerCount <= 0)
            {
                reset();
                _onCanGotoNextStep?.Invoke();
                return;
            }
            
            _m_ClickTriggerMgr.startMonitor();
            _m_MsgTriggerMgr.startMonitor();
            _m_AutoTriggerMgr.startMonitor();
        }
    }
}