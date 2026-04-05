using System;
using System.Collections.Generic;
using ALBasicProtocolPack;
using Common.EventObj;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonSimpleChoiceEventAgent : _ACommonSimpleEventAgent
    {
        private CommonEventChoiceRefObj _m_rCommonEventChoiceRefObj;
        private CommonEventChoiceShowRefObj _m_rCommonEventChoiceShowRefObj;
        private EventDealAddInfo _m_iEventDealAddInfo;//事件处理的额外信息

        private List<CommonEventChoiceOptionRefObj> _m_lOptionList;//选项配表数据列表
        
        public CommonEventChoiceRefObj CommonEventChoiceRefObj { get { return _m_rCommonEventChoiceRefObj; } }
        public CommonEventChoiceShowRefObj CommonEventChoiceShowRefObj { get { return _m_rCommonEventChoiceShowRefObj;} }
        public List<CommonEventChoiceOptionRefObj> optionList { get { return _m_lOptionList; } }

        private CommonSimpleChoiceEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj, _ICommonSimpleEventDealExtOp _eventDealExtOp)
            : base(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp)
        {
            _m_rCommonEventChoiceRefObj = null;
            _m_rCommonEventChoiceShowRefObj = null;
            _m_lOptionList = null;

            if (commonEventRefObj == null || commonEventRefObj.eventInstanceSubRefObj == null)
                return;
            
            try
            {
                _m_rCommonEventChoiceRefObj = (CommonEventChoiceRefObj)commonEventRefObj.eventInstanceSubRefObj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CommonSimpleChoiceEventAgent] 构造时传入的事件类型是:{commonEventRefObj.eventInstanceSubRefObj.eventType}, 但是强制转化为CommonEventChoiceRefObj时错误:{e}");
            }
            
            if (_m_rCommonEventChoiceRefObj != null)
            {
                _m_rCommonEventChoiceShowRefObj = GRefdataCoreMgr.instance.commonEventChoiceShowRefCore.getRef(_m_rCommonEventChoiceRefObj.choice_show_id);

                if (_m_rCommonEventChoiceRefObj.lOptionIdList != null)
                {
                    _m_lOptionList = new List<CommonEventChoiceOptionRefObj>();
                    CommonEventChoiceOptionRefObj optionRefObj = null;
                    foreach (long optionId in _m_rCommonEventChoiceRefObj.lOptionIdList)
                    {
                        optionRefObj = GRefdataCoreMgr.instance.commonEventChoiceOptionRefCore.getRef(optionId);
                        if(optionRefObj != null)
                            _m_lOptionList.Add(optionRefObj);
                    }
                }
            }
        }
        
        public static CommonSimpleChoiceEventAgent createEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj
            , _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            return new CommonSimpleChoiceEventAgent(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp);
        }

        protected override void _onUpdateServerEventShowInfo()
        {
        }

        protected override void _realDealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            if (_m_EventDealExtOp == null)
            {
                _break?.Invoke();
                return;
            }
            
            _updateEventDealAddInfo(_dealAddInfo);

            long uiPathId = _m_rCommonEventChoiceShowRefObj != null ? _m_rCommonEventChoiceShowRefObj.ui_res_id : UIResPathConst.C_COMMON_EVENT_CHOICE_EVENT_RES_ID;
            if (GGUIWndCommonSimpleChoiceEvent.instance.uiResId != uiPathId)
            {
                GGUIWndCommonSimpleChoiceEvent.instance.forceDiscard();
                GGUIWndCommonSimpleChoiceEvent.instance.uiResId = uiPathId;
            }
            
            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst_CommonEvent.C_COMMON_CHOICE_EVENT_NODE, _m_EventDealExtOp.dealEventNeedOpenTransBk,
                false, true, null, GGUIWndCommonSimpleChoiceEvent.instance, true, true,
                () =>
                {
                    GGUIWndCommonSimpleChoiceEvent.instance.setEventAgent(this, () =>
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
                    // else  // 若在退出事件窗口时事件已经完成, 那么会在事件中进行事件窗口关闭, 这里不需要额外判断了
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
                }, _m_EventDealExtOp.dealEventIsOnlyUINode)
            );
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
            if (_m_lOptionList == null)
                return null;
            
            CommonEvent_DealInfo_Choice dealInfoChoice = new CommonEvent_DealInfo_Choice();
            long selectChoiceId = _m_lOptionList.GetRandomItem()?.option_id ?? 0;
            dealInfoChoice.setOptionId(selectChoiceId);

            return dealInfoChoice.makePackage();
        }
        
        protected override CommonEventShowResultInfo _makeEventShowResultInfo(CommonEvent_DoneInfo _eventDoneInfo)
        {
            if (_eventDoneInfo == null)
                return null;

            byte[] eventDealInfoByte = _eventDoneInfo.getExtraInfo();
            if (eventDealInfoByte != null && eventDealInfoByte.Length > 0)
            {
                CommonEvent_DealInfo_Choice dealInfoChoice = new CommonEvent_DealInfo_Choice();
                dealInfoChoice.readPackage(eventDealInfoByte);

                CommonEventChoiceOptionRefObj choiceOptionRefObj = _m_lOptionList?.Find((_obj) =>
                {
                    if (_obj == null)
                        return false;

                    return _obj.option_id == dealInfoChoice.getOptionId();
                });

                return new CommonEventShowResultInfo(choiceOptionRefObj?.event_result_title, choiceOptionRefObj?.event_result_desc, _eventDoneInfo);
            }
            else
            {
                return new CommonEventShowResultInfo(string.Empty, string.Empty, _eventDoneInfo);
            }
        }
        
        /// <summary>
        /// 关闭事件处理窗口
        /// </summary>
        private void _closeEventDealWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_CommonEvent.C_COMMON_CHOICE_EVENT_NODE);
        }

        #region _ICommonEventShowInfo接口方法

        public override string eventName { get { return _m_rCommonEventChoiceShowRefObj?.event_name; } }
        public override string eventDetailDesc { get { return _m_rCommonEventChoiceShowRefObj?.event_detail_desc; } }
        public override string eventSimpleDesc { get { return _m_rCommonEventChoiceShowRefObj?.event_simple_desc; } }
        public override NPGTextureIndex inEventListIcon { get { return _m_rCommonEventChoiceShowRefObj?.event_list_icon; } }

        #endregion
        
        #region 事件具体处理时需要用到的一些方法

        public NPGTextureIndex eventDetailIcon { get { return _m_rCommonEventChoiceShowRefObj?.detail_icon; } }
        public void reqDealEvent(CommonEventChoiceOptionRefObj selectOption, Action<bool, CommonEvent_DoneInfo> _reqCallBack)
        {
            if (selectOption == null || _m_lOptionList == null || !_m_lOptionList.Contains(selectOption))
            {
                _reqCallBack?.Invoke(false, null);
                return;
            }

            CommonEvent_DealInfo_Choice dealInfoChoice = new CommonEvent_DealInfo_Choice(selectOption.option_id);
            reqDealEvent(dealInfoChoice.makePackage(), (_isSucc, _resMsg) =>
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