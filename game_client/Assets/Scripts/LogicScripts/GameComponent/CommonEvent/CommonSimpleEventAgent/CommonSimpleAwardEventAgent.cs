using System;
using System.Collections.Generic;
using ALBasicProtocolPack;
using Common.EventObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 通用奖励事件处理基类
    /// </summary>
    public class CommonSimpleAwardEventAgent : _ACommonSimpleEventAgent
    {
        private CommonEventAwardRefObj _m_rCommonEventAwardRefObj;
        private CommonEventAwardShowRefObj _m_rCommonEventAwardShowRefObj;
        private List<NPCommonCostItem> _m_lEventRewardList;//事件奖励
        private EventDealAddInfo _m_iEventDealAddInfo;//事件处理的额外信息
        
        public CommonEventAwardRefObj CommonEventAwardRefObj { get { return _m_rCommonEventAwardRefObj; } }
        public CommonEventAwardShowRefObj CommonEventAwardShowRefObj { get { return _m_rCommonEventAwardShowRefObj; } }
        public List<NPCommonCostItem> eventRewardList { get { return _m_lEventRewardList; } }

        private CommonSimpleAwardEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj, _ICommonSimpleEventDealExtOp _eventDealExtOp)
            : base(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp)
        {
            _m_rCommonEventAwardRefObj = null;
            _m_rCommonEventAwardShowRefObj = null;
            _m_lEventRewardList = null;

            if (commonEventRefObj == null || commonEventRefObj.eventInstanceSubRefObj == null)
                return;
            
            try
            {
                _m_rCommonEventAwardRefObj = (CommonEventAwardRefObj)commonEventRefObj.eventInstanceSubRefObj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CommonSimpleAwardEventAgent] 构造时传入的事件类型是:{commonEventRefObj.eventInstanceSubRefObj.eventType}, 但是强制转化为CommonEventAwardRefObj时错误:{e}");
            }

            if (_m_rCommonEventAwardRefObj != null)
            {
                _m_rCommonEventAwardShowRefObj = GRefdataCoreMgr.instance.commonEventAwardShowRefCore.getRef(_m_rCommonEventAwardRefObj.award_show_id);
                
                CommonEventRewardRefObj rewardRefObj = GRefdataCoreMgr.instance.commonEventRewardRefCore.getRef(_m_rCommonEventAwardRefObj.event_reward_id);
                if (rewardRefObj != null)
                    _m_lEventRewardList = rewardRefObj.lItemList;
            }
        }

        public static CommonSimpleAwardEventAgent createEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj
            , _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            return new CommonSimpleAwardEventAgent(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp);
        }

        protected override void _onUpdateServerEventShowInfo()
        {
        }

        protected override void _realDealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            if (_m_rCommonEventAwardShowRefObj == null || _m_EventDealExtOp == null)
            {
                _break?.Invoke();
                return;
            }
            
            _updateEventDealAddInfo(_dealAddInfo);

            //设置对应的窗口资源id
            if (GGUIWndCommonSimpleAwardEvent.instance.uiResId != _m_rCommonEventAwardShowRefObj.ui_res_id)
                GGUIWndCommonSimpleAwardEvent.instance.uiResId = _m_rCommonEventAwardShowRefObj.ui_res_id;

            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst_CommonEvent.C_COMMON_AWARD_EVENT_NODE, _m_EventDealExtOp.dealEventNeedOpenTransBk,
                false, true, null, GGUIWndCommonSimpleAwardEvent.instance, true, true,
                () =>
                {
                    GGUIWndCommonSimpleAwardEvent.instance.setEventAgent(this, ()=>
                    {
                        if (_m_iEventDealAddInfo != null && _m_iEventDealAddInfo.showResult != null)
                        {
                            _m_iEventDealAddInfo.showResult(_makeEventShowResultInfo(eventDoneInfo), _dealDone);
                        }
                        else
                        {
                            if (afterEventDialogRefObj != null && afterEventDialogRefObj.is_need_bk)//若需要显示模糊背景, 先关闭事件处理窗口
                            {
                                _closeEventDealWnd();
                                eventDealDoneGeneralShowProcess(eventDoneInfo, null, null, _dealDone);
                            }
                            else
                            {
                                // 不需要模糊背景时, 大概率事件有自己背景, 要在背景加载完成后退出
                                eventDealDoneGeneralShowProcess(eventDoneInfo, _closeEventDealWnd, null, _dealDone);
                            }
                        }
                    });
                    _startDeal?.Invoke();
                }, null, null, () =>
                {
                    if(!isEventDoneDataLevel)// 若在退出事件窗口时事件还未完成, 那么说明是中断退出
                        _break?.Invoke();
                    // else // 若在退出事件窗口时事件已经完成, 那么会在事件中进行事件窗口关闭, 这里不需要额外判断了
                    // {
                    //     if (_m_iEventDealAddInfo != null && _m_iEventDealAddInfo.showResult != null)
                    //     {
                    //         _m_iEventDealAddInfo.showResult(_makeEventShowResultInfo(eventDoneInfo), _dealDone);
                    //     }
                    //     else
                    //     {
                    //         eventDealDoneGeneralShowProcess(eventDoneInfo, null, _dealDone);
                    //     }
                    // }
                }, _m_EventDealExtOp.dealEventIsOnlyUINode));
        }


        public override bool canAutoDealEvent { get { return true; } }
        protected override void _realAutoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break)
        {
            _updateEventDealAddInfo(_dealAddInfo);

            reqAutoDealEvent((_isSucc, _resMsg) =>
            {
                if (_isSucc)
                {
                    if (eventDoneInfo != null)
                    {
                        if (_m_iEventDealAddInfo != null && _m_iEventDealAddInfo.showResult != null)
                        {
                            _m_iEventDealAddInfo.showResult(_makeEventShowResultInfo(eventDoneInfo), _dealDone);
                        }
                        else
                        {
                            showEventDealResultWnd(eventDoneInfo, null, _dealDone);
                        }
                    }
                    else
                    {
                        _dealDone?.Invoke();
                    }
                }
                else
                    _break?.Invoke();
            });
        }
        
        public override byte[] getEventAutoDealInfoByteArray()
        {
            return null;
        }

        protected override CommonEventShowResultInfo _makeEventShowResultInfo(CommonEvent_DoneInfo _eventDoneInfo)
        {
            if (_eventDoneInfo == null)
                return null;
            
            //奖励事件显示结果时不需要什么额外的展示信息
            CommonEventShowResultInfo resultInfo = new CommonEventShowResultInfo(
                _m_rCommonEventAwardShowRefObj?.event_result_title, _m_rCommonEventAwardShowRefObj?.event_result_desc, _eventDoneInfo);
            return resultInfo;
        }

        /// <summary>
        /// 关闭事件处理窗口
        /// </summary>
        private void _closeEventDealWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_CommonEvent.C_COMMON_AWARD_EVENT_NODE);
        }

        #region _ICommonEventShowInfo接口方法

        public override string eventName { get { return _m_rCommonEventAwardShowRefObj?.event_name; } }
        public override string eventDetailDesc { get { return _m_rCommonEventAwardShowRefObj?.event_detail_desc; } }
        public override string eventSimpleDesc { get { return _m_rCommonEventAwardShowRefObj?.event_simple_desc; } }
        public override NPGTextureIndex inEventListIcon { get { return _m_rCommonEventAwardShowRefObj?.event_list_icon; } }

        #endregion
        
        #region 事件具体处理时需要用到的一些方法

        public List<NPCommonCostItem> eventShowReward { get { return _m_lEventRewardList; } }
        public NPGTextureIndex eventDetailIcon { get { return _m_rCommonEventAwardShowRefObj?.detail_icon; } }

        /// <summary>
        /// 请求处理事件
        /// </summary>
        /// <param name="_reqCallBack"></param>
        public void reqDealEvent(Action<bool, CommonEvent_DoneInfo> _reqCallBack)
        {
            reqDealEvent(null, (_isSucc, _resMsg) =>
            {
                _reqCallBack?.Invoke(_isSucc, eventDoneInfo);
            });
        }

        /// <summary>
        /// 强制关闭窗口
        /// </summary>
        public override void forceBreakEventDeal()
        {
            _closeEventDealWnd();
        }

        #endregion

        #region 事件对应的_IEventDealAddInfo

        private class EventDealAddInfo : _IEventDealAddInfo
        {
            private Action<CommonEventShowResultInfo, Action> _m_aShowResultAction;//是否在结果窗口展示时完成事件, 若为false, 结果窗口关闭后才算完成
        
            public EventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResultAction)
            {
                _m_aShowResultAction = _showResultAction;
            }
        
            public Action<CommonEventShowResultInfo, Action> showResult { get { return _m_aShowResultAction; } }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_showResult">CommonEventShowResultInfo 为事件处理结果展示数据, Action为触发事件完成回调_triggerEventDone</param>
        /// <returns></returns>
        public static _IEventDealAddInfo getEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult)
        {
            return new EventDealAddInfo(_showResult);
        }

        private void _updateEventDealAddInfo(_IEventDealAddInfo _dealAddInfo)
        {
            if (_dealAddInfo is EventDealAddInfo)
            {
                _m_iEventDealAddInfo = (EventDealAddInfo) _dealAddInfo;
            }
            else
            {
                _m_iEventDealAddInfo = null;
            }
        }

        #endregion
    }
}