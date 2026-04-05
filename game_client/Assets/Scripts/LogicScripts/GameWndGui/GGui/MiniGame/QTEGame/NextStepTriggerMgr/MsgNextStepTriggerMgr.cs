using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class MsgNextStepTriggerMgr : _ANextStepTriggerMgr
    {
        [NotNull] private Dictionary<ENextStepTriggerMsgType, List<NextStepTrigger>> _m_dMsgTriggerDic = new Dictionary<ENextStepTriggerMsgType, List<NextStepTrigger>>();
        
        public MsgNextStepTriggerMgr(Action _onTrigger) : base(_onTrigger)
        {
        }

        public override ENextStepTriggerType triggerType { get { return ENextStepTriggerType.MSG; } }
        protected override void _onStartMonitor()
        {
            WinMsg.RegisterMsg(WinMsgType.MSG_NEXT_STEP_TRIGGER, _receiveTriggerMsg);
        }

        protected override void _onReset()
        {
            WinMsg.UnregisterMsg(WinMsgType.MSG_NEXT_STEP_TRIGGER, _receiveTriggerMsg);

            _m_dMsgTriggerDic.Clear();
        }

        protected override bool _regNextStepTrigger(NextStepTrigger _trigger)
        {
            // 由于在基类已经判断过触发器类型, 所以这里不再判断

            if (_trigger.triggerMsgType == ENextStepTriggerMsgType.NONE)
                return false;
            
            List<NextStepTrigger> triggerList = null;
            if (!_m_dMsgTriggerDic.TryGetValue(_trigger.triggerMsgType, out triggerList) || triggerList == null)
            {
                triggerList = new List<NextStepTrigger>();
                _m_dMsgTriggerDic[_trigger.triggerMsgType] = triggerList;
            }
            
            triggerList.Add(_trigger);

            return true;
        }

        public override bool allTriggerDone()
        {
            if (!_m_bIsStartMonitor)
                return false;

            return _m_dMsgTriggerDic.Count <= 0;
        }
        
        protected override void _dealTrigger(NextStepTrigger _trigger)
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
            
            // 这里重写基类dealTrigger方法, 去掉了_m_aOnTrigger调用, 因为改触发器触发以组为单位触发, 所以不需要单个触发器调用_m_aOnTrigger
        }
        
        /// <summary>
        /// 触发
        /// </summary>
        /// <param name="_objs"></param>
        private void _receiveTriggerMsg(params object[] _objs)
        {
            // 若触发器还未开始监听, 不执行
            if (!_m_bIsStartMonitor)
                return;

            if (_objs == null || _objs[0] == null || !(_objs[0] is ENextStepTriggerMsgType triggerMsgType))
                return;
            
            List<NextStepTrigger> triggerList = null;
            if (!_m_dMsgTriggerDic.TryGetValue(triggerMsgType, out triggerList))
                return;

            _m_dMsgTriggerDic.Remove(triggerMsgType);//从字典中移除
            
            if (triggerList != null)
            {
                for (int i = triggerList.Count - 1; i >= 0; i--)
                {
                    NextStepTrigger trigger = triggerList[i];
                    _dealTrigger(trigger);
                }
                
                triggerList.Clear();
            }

            _m_aOnTrigger?.Invoke();
        }
    }
}