using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 客户端本地的七日目标任务数据
    /// </summary>
    /// <remarks>
    /// 这个数据只是用来监听任务变动情况的
    /// </remarks>
    public class SevenDayGoalsTaskInfo
    {
        [NotNull] private readonly SevenDayGoalsTaskRewardRefObj _m_refObj;
        private readonly SevenDayGoalsTaskRefObj _m_taskRefObj;
        private bool _m_hadDrawReward;

        private Action<long> _m_chgAction;
        private bool _m_isRegMsg;//是否注册了消息监听

        internal SevenDayGoalsTaskInfo([NotNull] SevenDayGoalsTaskRewardRefObj _refObj, bool _hadDrawReward)
        {
            _m_refObj = _refObj;
            _m_taskRefObj = _m_refObj.task_ref;
            _m_hadDrawReward = _hadDrawReward;
        }
        
        
        [NotNull] public SevenDayGoalsTaskRewardRefObj refObj { get { return _m_refObj; } }
        public SevenDayGoalsTaskRefObj taskRefObj { get { return _m_taskRefObj; } }
        public long taskId { get { return _m_refObj.id; } }

        public bool hadDrawReward
        {
            get { return _m_hadDrawReward; }
        }

        public void init()
        {
            if (!_m_hadDrawReward && _m_taskRefObj?.add_msg_type_list != null)
            {
                _m_isRegMsg = true;
                foreach (string msgType in _m_taskRefObj.add_msg_type_list)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), msgType, out WinMsgType temp);
                    if (!isParse || temp == WinMsgType.NONE)
                        continue;

                    WinMsg.RegisterMsgAct(temp, _refreshTask);
                }
            }
        }
        public void discard()
        {
            _unRegisterMsg();
        }

        private void _unRegisterMsg()
        {
            if (_m_isRegMsg && _m_taskRefObj?.add_msg_type_list != null)
            {
                foreach (string msgType in _m_taskRefObj.add_msg_type_list)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), msgType, out WinMsgType temp);
                    if (!isParse || temp == WinMsgType.NONE)
                        continue;

                    WinMsg.UnregisterMsgAct(temp, _refreshTask);
                }
            }
            _m_isRegMsg = false;
        }
        
        public void setHadDrawReward()
        {
            _m_hadDrawReward = true;
            _unRegisterMsg();
        }

        public void registerTaskChg(Action<long> _onTaskChg)
        {
            _m_chgAction += _onTaskChg;
        }
        public void unregisterTaskChg(Action<long> _onTaskChg)
        {
            _m_chgAction -= _onTaskChg;
        }
        public long getCount()
        {
            return _m_taskRefObj?.process_cur_count?.CalculateVariableResult(null) ?? 0;
        }


        private void _refreshTask()
        {
            _m_chgAction?.Invoke(_m_refObj.id);
        }
    }
}