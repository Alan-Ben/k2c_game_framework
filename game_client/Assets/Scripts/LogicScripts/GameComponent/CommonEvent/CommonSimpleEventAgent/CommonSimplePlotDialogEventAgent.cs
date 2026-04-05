using System;
using Common.EventObj;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonSimplePlotDialogEventAgent : _ACommonSimpleEventAgent
    {
        private CommonEventPlotDialogRefObj _m_rCommonEventDialogRefObj;
        private EventDealAddInfo _m_iEventDealAddInfo;//事件处理的额外信息

        private CommonSimplePlotDialogEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj, _ICommonSimpleEventDealExtOp _eventDealExtOp)
            : base(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp)
        {
            if (commonEventRefObj == null || commonEventRefObj.eventInstanceSubRefObj == null)
                return;
            
            _m_rCommonEventDialogRefObj = null;

            try
            {
                _m_rCommonEventDialogRefObj = (CommonEventPlotDialogRefObj)commonEventRefObj.eventInstanceSubRefObj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CommonSimplePlotDialogEventAgent] 构造时传入的事件类型是:{commonEventRefObj.eventInstanceSubRefObj.eventType}, 但是强制转化为CommonEventPlotDialogRefObj时错误:{e}");
            }
        }
        
        public static CommonSimplePlotDialogEventAgent createEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj
            , _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            return new CommonSimplePlotDialogEventAgent(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp);
        }

        protected override void _onUpdateServerEventShowInfo()
        {
        }

        protected override void _realDealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            if (_m_rCommonEventDialogRefObj == null)
            {
                _break?.Invoke();
                return;
            }
            
            _updateEventDealAddInfo(_dealAddInfo);

            Action<bool, byte[]> reqCallBack = (_isSucc, _retMsg) =>
            {
                if (_isSucc)
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
                    _break?.Invoke();
                }
            };

            NPDialogueRefObj refObj = GRefdataCoreMgr.instance.dialogueMap.getRef(_m_rCommonEventDialogRefObj.dialog_id);
            QueueMgr.instance.AddNode(new GMainQueuePlotDialogueNode(refObj, _startDeal, ()=> reqDealEvent(null, reqCallBack),
                () =>
                {
                    if (!isEventDoneDataLevel)//若在退出事件窗口时事件还未完成, 那么说明是中断退出
                        _break?.Invoke();
                }));
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
            
            //对话事件显示结果时不需要什么额外的展示信息
            CommonEventShowResultInfo resultInfo = new CommonEventShowResultInfo(
                _m_rCommonEventDialogRefObj?.event_result_title, _m_rCommonEventDialogRefObj?.event_result_desc, _eventDoneInfo);
            return resultInfo;
        }

        /// <summary>
        /// 强制关闭窗口
        /// </summary>
        public override void forceBreakEventDeal()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GMainQueuePlotDialogueNode));
        }

        #region _ICommonEventShowInfo接口方法

        public override string eventName { get { return _m_rCommonEventDialogRefObj?.event_name; } }
        public override string eventSimpleDesc { get { return _m_rCommonEventDialogRefObj?.event_simple_desc; } }
        public override NPGTextureIndex inEventListIcon { get { return _m_rCommonEventDialogRefObj?.lEventListIcon; } }
        public override string eventDetailDesc { get { return _m_rCommonEventDialogRefObj?.event_detail_desc; } }

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