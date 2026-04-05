using System;
using ALBasicProtocolPack;
using Common.EventObj;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonSimpleDialogEventAgent : _ACommonSimpleEventAgent
    {
        private Func<byte[], CommonEvent_DoneInfo> _m_fGetEventDealDoneInfoByRetDealEventMsg;//将处理事件回包转化为处理结果回包
        private Func<byte[], CommonEvent_DoneInfo> _m_fGetEventDealDoneInfoByRetAutoDealEventMsg;//将处理事件回包转化为处理结果回包

        private CommonEventDialogRefObj _m_rCommonEventDialogRefObj;
        private EventDealAddInfo _m_iEventDealAddInfo;//事件处理的额外信息
        private bool _m_bIsBreak;//是否中断

        private CommonSimpleDialogEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj, _ICommonSimpleEventDealExtOp _eventDealExtOp)
            : base(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp)
        {
            if (commonEventRefObj == null || commonEventRefObj.eventInstanceSubRefObj == null)
                return;
            
            _m_rCommonEventDialogRefObj = null;
            _m_bIsBreak = false;

            try
            {
                _m_rCommonEventDialogRefObj = (CommonEventDialogRefObj)commonEventRefObj.eventInstanceSubRefObj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CommonSimpleDialogEventAgent] 构造时传入的事件类型是:{commonEventRefObj.eventInstanceSubRefObj.eventType}, 但是强制转化为CommonEventDialogRefObj时错误:{e}");
            }
        }
        
        public static CommonSimpleDialogEventAgent createEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj
            , _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            return new CommonSimpleDialogEventAgent(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp);
        }

        protected override void _onUpdateServerEventShowInfo()
        {
        }

        public override void dealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            _m_bIsBreak = false;
            base.dealEvent(_dealAddInfo, _startDeal, _dealDone, _break);
        }

        protected override void _realDealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            if (_m_bIsBreak)
                return;

            _updateEventDealAddInfo(_dealAddInfo);
            
            _startDeal?.Invoke();
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
                        eventDealDoneGeneralShowProcess(eventDoneInfo, null, null, _dealDone);
                    }
                }
                else
                {
                    _break?.Invoke();
                }
            };
            
            //因为对话事件的对话直接由事件主表的事件前对话配置, 所以对话事件只要直接请求处理事件协议就行
            reqDealEvent(null, reqCallBack);
        }

        public override bool canAutoDealEvent { get { return true; } }

        public override void autoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break)
        {
            _m_bIsBreak = false;
            base.autoDealEvent(_dealAddInfo, _dealDone, _break);
        }

        protected override void _realAutoDealEvent(_IEventDealAddInfo _dealAddInfo, Action _dealDone, Action _break)
        {
            if (_m_bIsBreak)
                return;

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
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MAIN_SPACE_DIALOG);
        }

        /// <summary>
        /// 强制中断对话事件
        /// </summary>
        public void forceBreakDialogueEvent()
        {
            _m_bIsBreak = true;
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MAIN_SPACE_DIALOG);
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