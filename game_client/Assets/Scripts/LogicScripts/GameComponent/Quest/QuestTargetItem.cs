using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NPEnum;
using ALPackage;
using Common.QuestObj;
using JetBrains.Annotations;

namespace GOE
{
    // 任务目标数据结构
    public class QuestTargetItem
    {
        //任务目标id 
        private long _m_questTargetId;

        //任务目标服务端计数
        private long _m_curCount;

        //任务目标客户端计数
        private long _m_curCusCount;

        //配表数据
        private QuestTargetRefObj _m_targetRefObj;

        //任务步骤
        private QuestStepItem _m_stepItem;

        //客户端触发的消息list
        [NotNull]private Dictionary<WinMsgType, List<MsgRecAction>> _m_clientTriggerDelegateDict;

        //构造函数
        public QuestTargetItem(Quest_Target _target,QuestStepItem _stepItem)
        {
            if (null == _target)
                return;

            _m_clientTriggerDelegateDict = new Dictionary<WinMsgType, List<MsgRecAction>>();
            _m_questTargetId = _target.getTargetId();

            _m_curCount = _target.getCurCount();

            _m_stepItem = _stepItem;

            //配表数据 
            _m_targetRefObj = GRefdataCoreMgr.instance.questTargetMap.getRef(_m_questTargetId);
            if (null == _m_targetRefObj)
            {
                ALLog.Error($"can not find {_m_questTargetId}'s QuestTargetRefObj!");
                return;
            }

            //获取当前计数器个数
            _m_curCusCount = _m_targetRefObj.process_cur_count.CalculateVariableResult(null);

        }
        
        //任务目标id
        public long questTargetId { get { return _m_questTargetId; } }

        //获取配表信息
        public QuestTargetRefObj targetRefObj { get { return _m_targetRefObj; } }
        public QuestStepRefObj stepRefObj { get { return _m_stepItem?.questStepRefObj; } }

        public long questStepId { get { return null == _m_stepItem ? 0 : _m_stepItem.questStepId; } }
        public void init()
        {
            //目标变动监听
            _addRegister();

            //检查是否可以自动完成
            if (getQuestTargetIsFinish())
                _m_stepItem.checkStepCanGet(false);
        }

        /// <summary>
        /// 重新计算客户端计数
        /// </summary>
        public void reCalcCurCusCount()
        {
            if(null == _m_targetRefObj)
                return;
            
            //获取当前计数器个数
            _m_curCusCount = _m_targetRefObj.process_cur_count.CalculateVariableResult(null);
        }

        /// <summary>
        /// 设置服务端的目标计数
        /// </summary>
        public void setCurCount(long _curCount)
        {
            if (_curCount == _m_curCount)
                return;

            //原本的总计数
            long lastTotalCount = getQuestTargetRealCount();

            _m_curCount = _curCount;

            //目标数量变动推送
            WinMsg.SendMsg(WinMsgType.QUEST_TARGET_UPDATE, this);

            if (getQuestTargetIsFinish())
                _m_stepItem.checkStepCanGet(true);

            //检查是否能显示任务完成提示
            _checkCanShowTip(lastTotalCount);
            //检查是否完成触发引导
            _checkTriggerTutorial(lastTotalCount);
        }

        /// <summary>
        /// 设置客户端目标计数器
        /// </summary>
        private void _setCusCount()
        {
            if (null == _m_targetRefObj)
                return;

            //原本的总计数
            long lastTotalCount = getQuestTargetRealCount();

            long count = _m_targetRefObj.process_cur_count.CalculateVariableResult(null);
            if (count != _m_curCusCount)
            {
                _m_curCusCount = count;

                //目标数量变动推送
                WinMsg.SendMsg(WinMsgType.QUEST_TARGET_UPDATE, this);

                if (getQuestTargetIsFinish())
                    _m_stepItem.checkStepCanGet(true);
            }

            //检查是否能显示任务完成提示
            _checkCanShowTip(lastTotalCount);
            //检查是否完成触发引导
            _checkTriggerTutorial(lastTotalCount);
        }


        /// <summary>
        /// 获取任务目标的实际计数
        /// </summary>
        /// <returns></returns>
        public long getQuestTargetRealCount()
        {
            if (null == _m_targetRefObj || null == _m_targetRefObj.process_cur_count)
                return _m_curCount;

            return _m_curCount + _m_curCusCount;
        }

        /// <summary>
        /// 任务目标是否完成
        /// </summary>
        /// <returns></returns>
        public bool getQuestTargetIsFinish()
        {
            if (null == _m_targetRefObj)
                return false;

            return getQuestTargetRealCount() >= _m_targetRefObj.process_count;
        }

        /// <summary>
        /// 监听客户端目标数变动
        /// </summary>
        private void _addRegister()
        {
            if (null == _m_targetRefObj)
                return;

            //注册进度刷新事件
            if (null != _m_targetRefObj.add_msg_type_list)
            {
                WinMsgType temp = 0;
                for (int i = 0; i < _m_targetRefObj.add_msg_type_list.Count; i++)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _m_targetRefObj.add_msg_type_list[i], out temp);
                    if (!isParse || temp == WinMsgType.NONE)
                        continue;

                    WinMsg.RegisterMsgAct(temp, _setCusCount);
                }
            }

            //如果是客户端驱动的，需要注册客户端监听事件
            if(_m_targetRefObj.is_client_target)
            {
                //每次注册先清空一次
                _clearTriggerClientTarget();

                if(null != _m_targetRefObj.client_trigger_msg)
                {
                    foreach (ClientTriggerMsgInfo clientTriggerMsgInfo in _m_targetRefObj.client_trigger_msg)
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
            if (_m_targetRefObj == null)
                return;

            //监听客户端目标数变动
            if (null != _m_targetRefObj.add_msg_type_list)
            {
                WinMsgType temp = 0;
                for (int i = 0; i < _m_targetRefObj.add_msg_type_list.Count; i++)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _m_targetRefObj.add_msg_type_list[i], out temp);
                    if (!isParse || 0 == temp)
                        continue;

                    WinMsg.UnregisterMsgAct(temp, _setCusCount);
                }
            }

            //如果是客户端驱动的，需要注销客户端监听事件
            if (_m_targetRefObj.is_client_target)
            {
                _clearTriggerClientTarget();
            }
            
        }

        ///注册消息，并且把回调存起来
        private void _addTriggerClientTarget(ClientTriggerMsgInfo _clientTriggerMsg)
        {
            if(null == _clientTriggerMsg)
                return;

            WinMsgType msgType = (WinMsgType)ALCommon.EnumParse(typeof(WinMsgType), _clientTriggerMsg.msgType, true);
            //获取一个新的委托
            MsgRecAction msgRecAction = _getMsgRecAction(msgType, _clientTriggerMsg.msgArgs);
            if(null == msgRecAction)
                return;

            //存起来
            List<MsgRecAction> broadcast;
            if(!_m_clientTriggerDelegateDict.TryGetValue(msgType, out broadcast))
            {
                broadcast = new List<MsgRecAction>();
                _m_clientTriggerDelegateDict.Add(msgType, broadcast);
            }

            if(!broadcast.Contains(msgRecAction))
            {
                broadcast.Add(msgRecAction);
            }
            
            //监听委托
            WinMsg.RegisterMsg(msgType, msgRecAction);
        }

        //反注册所有监听了的消息
        private void _clearTriggerClientTarget()
        {
            if(_m_clientTriggerDelegateDict.Count == 0)
                return;
            
            foreach (KeyValuePair<WinMsgType,List<MsgRecAction>> kv in _m_clientTriggerDelegateDict)
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

                if(isNeedTrigger)
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
            int addCount = 1;
            //发送消息给服务器增加进度，这里不改变本地进度。流程统一听从服务器处理
            NPGSClientListener.sendMsgByLog(GSWriter_028_QuestOp.make_004_ReqAddClientTargetCount(_m_stepItem.questId, _m_stepItem.questStepId, _m_questTargetId, addCount));
        }

        public void discard()
        {
            _removeRegister();
            _m_stepItem = null;
            _m_targetRefObj = null;
        }

        /// <summary>
        /// 检查是否完成触发引导
        /// </summary>
        private void _checkTriggerTutorial(long _lastCount)
        {
            if (_m_targetRefObj == null)
                return;

            long curTotalCount = getQuestTargetRealCount();
            if (_lastCount < _m_targetRefObj.process_count && curTotalCount >= _m_targetRefObj.process_count)
                GCommon.triggerTutorial();
        }

        /// <summary>
        /// 检查是否能显示任务完成提示
        /// </summary>
        /// <param name="_lastCount"></param>
        private void _checkCanShowTip(long _lastCount)
        {
            long curTotalCount = getQuestTargetRealCount();
            if (!NPPlayer.instance.questComp.canShowTargetFinishTip || _lastCount == curTotalCount || _m_targetRefObj == null)
                return;

            if (_lastCount < _m_targetRefObj.process_count && curTotalCount >= _m_targetRefObj.process_count && _m_targetRefObj.done_center_tip_id > 0)
                NPGUIAddSceneCenterTip.instance.showIconTextTip(null, _m_targetRefObj.getTargetStr(), _m_targetRefObj.done_center_tip_id);
        }
    }
}
