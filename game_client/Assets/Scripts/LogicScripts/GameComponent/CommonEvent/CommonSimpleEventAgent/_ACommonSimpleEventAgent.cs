using System;
using System.Collections.Generic;
using ALBasicProtocolPack;
using ALPackage;
using Common.EventEnum;
using Common.EventObj;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 通用简易事件处理代理(不含过程数据，只用请求操作成功一次就完成事件)
    /// </summary>
    public abstract class _ACommonSimpleEventAgent : _AEventAgent, _ICommonEventShowInfo
    {
        protected long _m_lEventId;//通用事件id
        protected byte[] _m_ServerEventShowInfo;//服务器给的事件显示信息
        protected _ICommonSimpleEventDealExtOp _m_EventDealExtOp;//事件处理外部操作
        protected CommonEventRefObj _m_rCommonEventRefObj;//通用事件事件数据
        protected NPDialogueRefObj _m_rBeforeEventDialogRefObj;//事件前置对话数据
        protected NPDialogueRefObj _m_rAfterEventDialogRefObj;//事件后置对话数据
        protected CommonEvent_DoneInfo _m_iEventDoneInfo;//事件完成信息

        private bool _m_bHasReqEventDone;//是否已经请求过事件完成(因为简易事件只要请求过一次处理事件就完成, 所以这个标志记录是否请求过处理)
        public override bool isEventDoneDataLevel { get { return _m_bHasReqEventDone; } }
        public CommonEvent_DoneInfo eventDoneInfo { get { return _m_iEventDoneInfo; } }

        protected _ACommonSimpleEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj, _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            _m_lEventId = _eventId;
            _m_ServerEventShowInfo = _eventShowInfo;
            _m_rCommonEventRefObj = _commonEventRefObj;
            _m_rBeforeEventDialogRefObj = _m_rCommonEventRefObj == null ? null : GRefdataCoreMgr.instance.dialogueMap.getRef(_m_rCommonEventRefObj.before_event_dialog_id);
            _m_rAfterEventDialogRefObj = _m_rCommonEventRefObj == null ? null : GRefdataCoreMgr.instance.dialogueMap.getRef(_m_rCommonEventRefObj.after_event_dialog_id);

            _m_EventDealExtOp = _eventDealExtOp;
            _m_bHasReqEventDone = false;
            _m_iEventDoneInfo = null;

            if (_commonEventRefObj == null || _commonEventRefObj.eventInstanceSubRefObj == null)
            {
                Debug.LogError("[_ACommonEventAgent] 构造数据异常, _commonEventRefObj == null || _commonEventRefObj.eventInstanceSubRefObj == null, " +
                               "因为事件主表CommonEventRefObj中不存在事件类型信息, 因此必须要存在事件子表信息才能确定事件类型并对应构造出具体事件");
            }
            
            _onUpdateServerEventShowInfo();
        }
        
        /// <summary>
        /// 更新事件展示信息
        /// </summary>
        /// <param name="_showInfo"></param>
        public void updateServerEventShowInfo(byte[] _showInfo)
        {
            _m_ServerEventShowInfo = _showInfo;
         
            _onUpdateServerEventShowInfo();
        }
        
        /// <summary>
        /// 更新服务器给的事件展示数据时调用
        /// </summary>
        protected abstract void _onUpdateServerEventShowInfo();

        public CommonEventRefObj commonEventRefObj { get { return _m_rCommonEventRefObj; } }

        /// <summary>
        /// 事件id
        /// </summary>
        public long eventId { get { return _m_lEventId; } }
        
        /// <summary>
        /// 事件类型
        /// </summary>
        public ECommonEventType commonEventType
        {
            get
            {
                if (commonEventRefObj == null)
                    return ECommonEventType.NONE;

                return commonEventRefObj.eventInstanceSubRefObj?.eventType ?? ECommonEventType.NONE;
            } 
        }

        /// <summary>
        /// 事件开始对话配表数据
        /// </summary>
        public NPDialogueRefObj beforeEventDialogRefObj { get { return _m_rBeforeEventDialogRefObj; } }
        
        /// <summary>
        /// 事件结束对话配表数据
        /// </summary>
        public NPDialogueRefObj afterEventDialogRefObj { get { return _m_rAfterEventDialogRefObj; } }

        #region 事件处理

        protected override void _dealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            // 显示剧情前置对话
            showBeforeEventDialog(() =>
            {
                //调用子类处理方法
                _realDealEvent(_dealAddInfo, _startDeal, ()=>
                {
                    GCommon.triggerTutorial();//尝试触发引导(因为事件可能在窗口关闭后才去请求事件完成, 这时若事件没有后置剧情或奖励弹窗, 这样就不会有Node切换, 导致引导不触发)

                    _dealDone?.Invoke();
                }, _break);
            });
        }

        /// <summary>
        /// 子类重写的事件处理方法
        /// </summary>
        /// <param name="_dealDone"></param>
        protected abstract void _realDealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break);

        protected override void _realReqDealEvent(byte[] _dealEventInfo, Action<bool, byte[]> _reqCallBack)
        {
            if (_m_EventDealExtOp == null || _m_EventDealExtOp.reqDealEvent == null)
            {
                _reqCallBack?.Invoke(false, null);
            }
            else
            {
                _m_EventDealExtOp.reqDealEvent(_dealEventInfo, (_isSucc, _msg)=>
                {
                    if (_isSucc)//若请求成功, 标志置为true
                    {
                        _m_iEventDoneInfo = getEventDealDoneInfoByRetDealEventMsg(_msg);
                        _m_bHasReqEventDone = true;
                    }

                    _reqCallBack?.Invoke(_isSucc, _msg);
                });
            }
        }

        /// <summary>
        /// 将事件处理回包转化为事件完成信息的方法
        /// </summary>
        /// <param name="_retMsg"></param>
        /// <returns></returns>
        public CommonEvent_DoneInfo getEventDealDoneInfoByRetDealEventMsg(byte[] _retMsg)
        {
            return _m_EventDealExtOp?.getEventDealDoneInfoByRetDealEventMsg?.Invoke(_retMsg);
        } 

        #endregion

        #region 自动处理事件

        protected override void _autoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break)
        {
            showBeforeEventDialog(() =>
            {
                _realAutoDealEvent(_dealAddInfo, _dealDone, _break);
            }, true, true);
        }
        
        /// <summary>
        /// 子类重写的事件自动处理方法
        /// </summary>
        /// <param name="_dealDone"></param>
        protected abstract void _realAutoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break);

        protected override void _realReqAutoDealEvent(byte[] _dealEventInfo, Action<bool, byte[]> _reqCallBack)
        {
            if (_m_EventDealExtOp == null || _m_EventDealExtOp.reqAutoDealEvent == null)
            {
                _reqCallBack?.Invoke(false, null);
            }
            else
            {
                _m_EventDealExtOp.reqAutoDealEvent(_dealEventInfo, (_isSucc, _msg)=>
                {
                    if (_isSucc)
                    {
                        _m_iEventDoneInfo = getEventDealDoneInfoByRetAutoDealEventMsg(_msg);
                        _m_bHasReqEventDone = true;//若请求成功, 标志置为true
                    }
                    
                    _reqCallBack?.Invoke(_isSucc, _msg);
                });
            }
        }
        protected override void _realReqAutoDealEventWithNoOp(Action<bool, byte[]> _reqCallBack)
        {
            if (_m_EventDealExtOp == null || _m_EventDealExtOp.reqAutoDealEvent == null)
            {
                _reqCallBack?.Invoke(false, null);
            }
            else
            {
                _m_EventDealExtOp.reqAutoDealEventWithNoOp((_isSucc, _msg)=>
                {
                    if (_isSucc)
                    {
                        _m_iEventDoneInfo = getEventDealDoneInfoByRetAutoWithNoOpDealEventMsg(_msg);
                        _m_bHasReqEventDone = true;//若请求成功, 标志置为true
                    }
                    
                    _reqCallBack?.Invoke(_isSucc, _msg);
                });
            }
        }

        /// <summary>
        /// 将事件自动处理回包转化为事件完成信息的方法
        /// </summary>
        /// <param name="_retMsg"></param>
        /// <returns></returns>
        public CommonEvent_DoneInfo getEventDealDoneInfoByRetAutoDealEventMsg(byte[] _retMsg)
        {
            return _m_EventDealExtOp?.getEventDealDoneInfoByRetAutoDealEventMsg?.Invoke(_retMsg);
        } 

        /// <summary>
        /// 将事件自动处理回包转化为事件完成信息的方法，不需要重置数据的结果
        /// </summary>
        /// <param name="_retMsg"></param>
        /// <returns></returns>
        public CommonEvent_DoneInfo getEventDealDoneInfoByRetAutoWithNoOpDealEventMsg(byte[] _retMsg)
        {
            return _m_EventDealExtOp?.getEventDealDoneInfoByRetAutoWithNoOpDealEventMsg?.Invoke(_retMsg);
        } 
        
        #endregion

        #region _ICommonEventShowInfo

        public abstract string eventName { get; }
        public abstract string eventSimpleDesc { get; }
        public abstract NPGTextureIndex inEventListIcon { get; }
        public abstract string eventDetailDesc { get; }

        #endregion
        
        #region 部分事件通用展示方法

        /// <summary>
        /// 事件结束展示通用流程
        /// </summary>
        /// <param name="_eventDoneInfo"></param>
        /// <param name="_showDone"></param>
        public void eventDealDoneGeneralShowProcess(CommonEvent_DoneInfo _eventDoneInfo, Action _onAfterEventDialogShow, Action _startShowResultWnd, Action _showDone)
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_processDone) =>//显示后置剧情对话
                {
                    showAfterEventDialog(()=>
                    {
                        _onAfterEventDialogShow?.Invoke();
                        _processDone?.Invoke();
                    });
                })
                .addDelegateProcess((_processDone) =>//显示事件结果窗口
                {
                    showEventDealResultWnd(_eventDoneInfo, _startShowResultWnd, _processDone);
                })
                .addProcess(() =>//调用事件完成回调
                {
                    _showDone?.Invoke();
                })
                .deal();
        } 
        
        /// <summary>
        /// 事件结束展示通用流程
        /// </summary>
        /// <param name="_commonEventShowResultInfo"></param>
        /// <param name="_showDone"></param>
        public void eventDealDoneGeneralShowProcess(CommonEventShowResultInfo _commonEventShowResultInfo, Action _onAfterEventDialogShow, Action _startShowResultWnd, Action _showDone, bool _onNoNormalRewardNeedShowResultWnd = false)
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_processDone) =>//显示后置剧情对话
                {
                    showAfterEventDialog(()=>
                    {
                        _processDone?.Invoke();
                    });
                    _onAfterEventDialogShow?.Invoke();
                })
                .addDelegateProcess((_processDone) =>//显示事件结果窗口
                {
                    showEventDealResultWnd(_commonEventShowResultInfo, _startShowResultWnd, _processDone, _onNoNormalRewardNeedShowResultWnd);
                })
                .addProcess(() =>//调用事件完成回调
                {
                    _showDone?.Invoke();
                })
                .deal();
        } 
        
        /// <summary>
        /// 展示事件处理结果(若展示方式和通用展示方式不同, 可自行在子类重写)
        /// </summary>
        /// <param name="_eventDoneInfo">事件处理结果信息</param>
        /// <param name="_showDone"></param>
        public virtual void showEventDealResultWnd(CommonEvent_DoneInfo _eventDoneInfo, Action _startShowResultWnd, Action _showDone, bool _onNoNormalRewardNeedShowResultWnd = false)
        {
            showEventDealResultWnd(_makeEventShowResultInfo(_eventDoneInfo), _startShowResultWnd, _showDone, _onNoNormalRewardNeedShowResultWnd);
        }
        
        /// <summary>
        /// 展示事件处理结果(若展示方式和通用展示方式不同, 可自行在子类重写)
        /// </summary>
        /// <param name="_eventShowResultInfo">事件处理结果信息</param>
        /// <param name="_showDone"></param>
        public virtual void showEventDealResultWnd(CommonEventShowResultInfo _eventShowResultInfo, Action _startShowResultWnd, Action _showDone, bool _onNoNormalRewardNeedShowResultWnd)
        {
            if (_eventShowResultInfo == null)
            {
                _showDone?.Invoke();
                return;
            }

            List<NPCommon.NPCommon_ItemInfo> rewardList = _eventShowResultInfo.rewardList;
            // 当没有奖励需要展示 且 结果描述全部为空时, 不显示事件结算窗口 
            if ((rewardList == null || rewardList.Count <= 0) && string.IsNullOrEmpty(_eventShowResultInfo.resultTitle) &&
                string.IsNullOrEmpty(_eventShowResultInfo.resultDesc))
            {
                CommonRewardDealer.showSpecial(_eventShowResultInfo.gainItemFilterData, null);
                _showDone?.Invoke();
            }
            else
            {
                ENpRewardShowType rewardShowType = _m_rCommonEventRefObj?.eRewardShowType ?? ENpRewardShowType.DEFAULT;
                switch (rewardShowType)
                {
                    case ENpRewardShowType.TIP:
                        _startShowResultWnd?.Invoke();
                        CommonRewardDealer.showSpecial(_eventShowResultInfo.gainItemFilterData, null);
                        GCommon.showGainRewardTip(rewardList);
                        _showDone?.Invoke();
                        break;
                    default:
                        GGUIWndCommonEventResult.showCommonEventResultNotice(_eventShowResultInfo, _startShowResultWnd, _showDone, _onNoNormalRewardNeedShowResultWnd);
                        break;
                }
            }
        }

        /// <summary>
        /// 将服务器给的事件结果数据转化为展示结果数据(需要使用showEventDealResult进行展示时重写)
        /// </summary>
        /// <param name="_eventDoneInfo"></param>
        /// <returns></returns>
        protected virtual CommonEventShowResultInfo _makeEventShowResultInfo(CommonEvent_DoneInfo _eventDoneInfo)
        {
            Debug.LogError("[_ACommonSimpleEventAgent] 请子类重写_makeEventShowResultInfo 方法");
            return null;
        }

        /// <summary>
        /// 显示事件前置剧情对话
        /// </summary>
        public virtual void showBeforeEventDialog(Action _showDone, bool _isMain = true, bool _needAutoPlay = false)
        {
            if (commonEventRefObj == null || commonEventRefObj.before_event_dialog_id <= 0)
            {
                _showDone?.Invoke();
                return;
            }
            
            GCommon.enterDialogueNode(commonEventRefObj.before_event_dialog_id, _showDone, _isMain, _needAutoPlay);
        }
        
        /// <summary>
        /// 显示事件后置剧情对话
        /// </summary>
        public void showAfterEventDialog(Action _showDone, bool _isMain = true, bool _needAutoPlay = false)
        {
            if (commonEventRefObj == null || commonEventRefObj.after_event_dialog_id <= 0)
            {
                _showDone?.Invoke();
                return;
            }

            GCommon.enterDialogueNode(commonEventRefObj.after_event_dialog_id, _showDone, _isMain, _needAutoPlay);
        }

        /// <summary>
        /// 强制中断事件处理
        /// </summary>
        public abstract void forceBreakEventDeal();

        #endregion
    }
}