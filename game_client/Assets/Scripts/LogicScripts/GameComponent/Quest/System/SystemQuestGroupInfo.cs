using ALPackage;
using Common.QuestObj;
using JetBrains.Annotations;
using NPCommon;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 系统任务组信息
    /// </summary>
    public class SystemQuestGroupInfo : _ISystemQuest
    {
        //组id
        private long _m_lGroupId;
        //当前任务数据
        private SystemQuestRefObj _m_curSystemQuestRef;
        //任务目标服务端计数
        private long _m_lCurServerCount;
        //任务目标客户端计数
        private long _m_lCurCustomCount;
        //客户端触发的消息list
        [NotNull] private Dictionary<WinMsgType, List<MsgRecAction>> _m_clientTriggerDelegateDict;
        //是否可以弹出任务完成提示标记
        private bool _m_bCanShowTipTag = false;


        /// <summary>
        /// 系统任务组id
        /// </summary>
        public long groupId { get { return _m_lGroupId; } }
        /// <summary>
        /// 当前系统任务配置
        /// </summary>
        public SystemQuestRefObj curSystemQuestRef { get { return _m_curSystemQuestRef; } }
        /// <summary>
        /// 获取当前计数
        /// </summary>
        public long curCount { get { return _m_lCurServerCount + _m_lCurCustomCount; } }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool isFinish { get { return _m_curSystemQuestRef != null && _m_curSystemQuestRef.process_count <= curCount; } }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string questName { get { return _m_curSystemQuestRef != null ? TextTranslate.instance.getLanguage(_m_curSystemQuestRef.quest_name, _m_curSystemQuestRef.quest_name_args) : null; } }
        /// <summary>
        /// 展示名称
        /// </summary>
        public string showNameStr { get { return _m_curSystemQuestRef != null ? TextTranslate.instance.getLanguage(TransKeyConst.common_strDotStr, _m_curSystemQuestRef.step, questName) : null; } }
        /// <summary>
        /// 目标计数
        /// </summary>
        public long targetCount { get { return _m_curSystemQuestRef != null ? _m_curSystemQuestRef.process_count : 0; } }
        /// <summary>
        /// 进度值格式化显示方式
        /// </summary>
        public EValueFormatType processNumFormat { get{ return _m_curSystemQuestRef != null ? _m_curSystemQuestRef.process_num_format : EValueFormatType.NORMAL; } }
        /// <summary>
        /// 是否可以领取奖励
        /// </summary>
        public bool canGetReward { get { return isFinish; } }
        /// <summary>
        /// 奖励列表
        /// </summary>
        public List<NPCommonCostItem> rewardItemList { get { return _m_curSystemQuestRef != null ? _m_curSystemQuestRef.done_gain_item_list : null; } }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_info"></param>
        public SystemQuestGroupInfo(SystemQuest_Info _info)
        {
            _m_clientTriggerDelegateDict = new Dictionary<WinMsgType, List<MsgRecAction>>();
            updateInfo(_info);
        }

        /// <summary>
        /// 初始化是否可以弹出任务完成提示标记，由于会引用到其他组件数据去判断，所以需要在所有组件都初始化完之后再初始化
        /// </summary>
        public void initCanShowTipTag()
        {
            _m_bCanShowTipTag = true;
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(SystemQuest_Info _info)
        {
            if (_info == null)
                return;

            //记录原本的任务数据
            long lastCount = curCount;
            SystemQuestRefObj lastRef = _m_curSystemQuestRef;

            //更新数据
            _removeRegister();
            _m_lGroupId = _info.getGroupId();
            _m_curSystemQuestRef = GRefdataCoreMgr.instance.getSystemQuestRefObj(_info.getGroupId(), _info.getStep());
            _m_lCurServerCount = _info.getCount();
            _m_lCurCustomCount = _m_curSystemQuestRef?.process_cur_count?.CalculateVariableResult(null) ?? 0;
            _addRegister();

            //检查是否能显示任务完成提示
            if(lastRef != null && _m_curSystemQuestRef != null && lastRef.group_id == _m_curSystemQuestRef.group_id && lastRef.step == _m_curSystemQuestRef.step)
                _checkCanShowTip(lastCount);
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void discard()
        {
            _m_lGroupId = 0;
            _m_curSystemQuestRef = null;
            _m_lCurServerCount = 0;
            _m_lCurCustomCount = 0;
            _m_bCanShowTipTag = false;
            _removeRegister();
        }

        /// <summary>
        /// 处理领取奖励
        /// </summary>
        public void dealGetReward(Action<List<NPCommon_ItemInfo>> _callback)
        {
            if (_m_curSystemQuestRef == null || !canGetReward)
                return;

            NPPlayer.instance.systemQuestComp.reqSystemQuestDrawReward(_m_curSystemQuestRef.group_id, _m_curSystemQuestRef.step,
                (_ret) =>
                {
                    _callback?.Invoke(_ret.getItemList());
                    WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.SYSTEM_QUEST_REWARD_DONE);
                });
        }

        /// <summary>
        /// 处理前往
        /// </summary>
        public void dealGoTo()
        {
            if (_m_curSystemQuestRef == null || _m_curSystemQuestRef.go_to == null || canGetReward)
                return;

            _m_curSystemQuestRef.go_to.dealEffect();
        }

        /// <summary>
        /// 重新计算当前客户端目标计数
        /// </summary>
        public void reCalcCurCusCount()
        {
            _m_lCurCustomCount = _m_curSystemQuestRef?.process_cur_count?.CalculateVariableResult(null) ?? 0;
        }

        /// <summary>
        /// 监听客户端目标数变动
        /// </summary>
        private void _addRegister()
        {
            if (null == _m_curSystemQuestRef)
                return;

            //注册进度刷新事件
            if (null != _m_curSystemQuestRef.addMsgTypeList)
            {
                for (int i = 0; i < _m_curSystemQuestRef.addMsgTypeList.Count; i++)
                {
                    WinMsgType temp = _m_curSystemQuestRef.addMsgTypeList[i];
                    WinMsg.RegisterMsgAct(temp, _setCusCount);
                }
            }

            //如果是客户端驱动的，需要注册客户端监听事件
            if (_m_curSystemQuestRef.is_client_target)
            {
                //每次注册先清空一次
                _clearTriggerClientTarget();

                if (null != _m_curSystemQuestRef.client_trigger_msg)
                {
                    foreach (ClientTriggerMsgInfo clientTriggerMsgInfo in _m_curSystemQuestRef.client_trigger_msg)
                    {
                        //注册消息，并且把回调存起来
                        _addTriggerClientTarget(clientTriggerMsgInfo);
                    }
                }
            }

        }

        /// <summary>
        /// 移除监听
        /// </summary>
        private void _removeRegister()
        {
            if (_m_curSystemQuestRef == null)
                return;

            //监听客户端目标数变动
            if (null != _m_curSystemQuestRef.addMsgTypeList)
            {
                for (int i = 0; i < _m_curSystemQuestRef.addMsgTypeList.Count; i++)
                {
                    WinMsgType temp = _m_curSystemQuestRef.addMsgTypeList[i];
                    WinMsg.UnregisterMsgAct(temp, _setCusCount);
                }
            }

            //如果是客户端驱动的，需要注销客户端监听事件
            if (_m_curSystemQuestRef.is_client_target)
            {
                _clearTriggerClientTarget();
            }

        }

        //注册消息，并且把回调存起来
        private void _addTriggerClientTarget(ClientTriggerMsgInfo _clientTriggerMsg)
        {
            if (null == _clientTriggerMsg)
                return;

            WinMsgType msgType = (WinMsgType)ALCommon.EnumParse(typeof(WinMsgType), _clientTriggerMsg.msgType, true);
            //获取一个新的委托
            MsgRecAction msgRecAction = _getMsgRecAction(msgType, _clientTriggerMsg.msgArgs);
            if (null == msgRecAction)
                return;

            //存起来
            List<MsgRecAction> broadcast;
            if (!_m_clientTriggerDelegateDict.TryGetValue(msgType, out broadcast))
            {
                broadcast = new List<MsgRecAction>();
                _m_clientTriggerDelegateDict.Add(msgType, broadcast);
            }

            if (!broadcast.Contains(msgRecAction))
            {
                broadcast.Add(msgRecAction);
            }

            //监听委托
            WinMsg.RegisterMsg(msgType, msgRecAction);
        }

        //反注册所有监听了的消息
        private void _clearTriggerClientTarget()
        {
            if (_m_clientTriggerDelegateDict.Count == 0)
                return;

            foreach (KeyValuePair<WinMsgType, List<MsgRecAction>> kv in _m_clientTriggerDelegateDict)
            {
                foreach (MsgRecAction msgRecAction in kv.Value)
                {
                    WinMsg.UnregisterMsg(kv.Key, msgRecAction);
                }
            }

            _m_clientTriggerDelegateDict.Clear();
        }

        //获取一个新的监听回调委托
        private MsgRecAction _getMsgRecAction(WinMsgType _msgType, string _msgArgs)
        {
            MsgRecAction msgRecAction = new MsgRecAction((object[] _objs) =>
            {
                //是否需要触发
                bool isNeedTrigger = false;

                //没有配置参数说明不校验参数，是这个枚举就进度加1
                if (string.IsNullOrEmpty(_msgArgs))
                {
                    isNeedTrigger = true;
                }
                else
                {
                    switch (_msgType)
                    {
                        //玩家进入某个个人物件交互范围，带的参数是个人物件id
                        case WinMsgType.ON_PLAYER_ENTER_PRIVATE_ITEM_INTERACTIVE_RANGE:
                        //对话结束，带的参数是对话id
                        case WinMsgType.DIALOG_END:
                            if (null == _objs || _objs.Length == 0 || null == _objs[0])
                                break;
                            //是这个对话id代表触发
                            if (_msgArgs == _objs[0].ToString())
                                isNeedTrigger = true;
                            break;
                        //对话结束，带的参数是对话id
                        case WinMsgType.SET_TUTORIAL_DONE:
                            if (null == _objs || _objs.Length == 0 || null == _objs[0])
                                break;
                            //是这引导id代表触发
                            if (_msgArgs == _objs[0].ToString())
                                isNeedTrigger = true;
                            break;
                        //默认都触发
                        default:
                            isNeedTrigger = true;
                            break;
                    }
                }

                if (isNeedTrigger)
                    _triggerClientTarget(_objs);
            });

            return msgRecAction;
        }

        /// <summary>
        /// 触发客户端进度变更的消息，第一个参数如果有值则是增加的进度值
        /// </summary>
        /// <param name="_objs"></param>
        private void _triggerClientTarget(object[] _objs)
        {
            if (_m_curSystemQuestRef == null)
                return;

            int addCount = 1;
            //发送消息给服务器增加进度，这里不改变本地进度。流程统一听从服务器处理
            NPPlayer.instance.systemQuestComp.reqSystemQuestAddClientCount(_m_lGroupId, _m_curSystemQuestRef.step, addCount);
        }

        /// <summary>
        /// 设置客户端目标计数器
        /// </summary>
        private void _setCusCount()
        {
            if (null == _m_curSystemQuestRef)
                return;

            long lastCount = curCount;
            long count = _m_curSystemQuestRef.process_cur_count?.CalculateVariableResult(null) ?? 0;
            if (count != _m_lCurCustomCount)
            {
                _m_lCurCustomCount = count;

                //检查是否能显示任务完成提示
                _checkCanShowTip(lastCount);

                //目标数量变动推送
                WinMsg.SendMsg(WinMsgType.ON_SYSTEM_QUEST_COUNT_CHG, this);
            }
        }

        /// <summary>
        /// 检查是否能显示任务完成提示
        /// </summary>
        /// <param name="_lastCount"></param>
        private void _checkCanShowTip(long _lastCount)
        {
            if (!_m_bCanShowTipTag || _lastCount == curCount || _m_curSystemQuestRef == null)
                return;

            if (_lastCount < _m_curSystemQuestRef.process_count && curCount >= _m_curSystemQuestRef.process_count)
                NPGUIAddSceneCenterTip.instance.showIconTextTip(null, questName, GRefdataCoreMgr.instance.npGeneral.system_quest_finish_center_tip_id);
        }
    }
}
