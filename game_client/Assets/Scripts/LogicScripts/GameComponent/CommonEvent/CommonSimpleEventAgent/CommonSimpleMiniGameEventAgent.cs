using System;
using Common.EventObj;
using GOE.MiniGame;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonSimpleMiniGameEventAgent : _ACommonSimpleEventAgent
    {
        private CommonEventMiniGameRefObj _m_rCommonEventMiniGameRefObj;
        private CommonEventMiniGameShowRefObj _m_rCommonEventMiniGameShowRefObj;
        private EventDealAddInfo _m_iEventDealAddInfo;//事件处理的额外信息

        private CommonSimpleMiniGameEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull] CommonEventRefObj _commonEventRefObj, _ICommonSimpleEventDealExtOp _eventDealExtOp) : base(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp)
        {
            if (commonEventRefObj == null || commonEventRefObj.eventInstanceSubRefObj == null)
                return;
            
            _m_rCommonEventMiniGameRefObj = null;

            try
            {
                _m_rCommonEventMiniGameRefObj = (CommonEventMiniGameRefObj)commonEventRefObj.eventInstanceSubRefObj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CommonSimpleMiniGameEventAgent] 构造时传入的事件类型是:{commonEventRefObj.eventInstanceSubRefObj.eventType}, 但是强制转化为CommonEventMiniGameRefObj时错误:{e}");
            }
            
            if (_m_rCommonEventMiniGameRefObj != null)
            {
                _m_rCommonEventMiniGameShowRefObj = GRefdataCoreMgr.instance.commonEventMiniGameShowRefCore.getRef(_m_rCommonEventMiniGameRefObj.mini_game_show_id);
            }
        }
        
        public static CommonSimpleMiniGameEventAgent createEventAgent(long _eventId, byte[] _eventShowInfo, [NotNull]CommonEventRefObj _commonEventRefObj
            , _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            return new CommonSimpleMiniGameEventAgent(_eventId, _eventShowInfo, _commonEventRefObj, _eventDealExtOp);
        }

        public override byte[] getEventAutoDealInfoByteArray()
        {
            return null;
        }

        protected override void _onUpdateServerEventShowInfo()
        {
        }

        protected override void _realDealEvent(_IEventDealAddInfo _dealAddInfo, Action _startDeal, Action _dealDone, Action _break)
        {
            if (_m_rCommonEventMiniGameShowRefObj == null)
            {
                Debug.LogError("[CommonSimpleMiniGameEventAgent _realDealEvent] _m_rCommonEventMiniGameShowRefObj == null");
                _break?.Invoke();
                return;
            }
            
            _updateEventDealAddInfo(_dealAddInfo);
            
            // 请求处理事件完成
            Action<bool, byte[]> reqCallBack = (_isSucc, _retMsg) =>
            {
                MiniGameMgr.exitGame();
                if (_isSucc)
                {
                    if (_m_iEventDealAddInfo != null && _m_iEventDealAddInfo.showResult != null)
                    {
                        _m_iEventDealAddInfo.showResult(_makeEventShowResultInfo(eventDoneInfo), ()=>
                        {
                            _dealDone?.Invoke();
                        });
                    }
                    else
                    {
                        if (afterEventDialogRefObj != null && afterEventDialogRefObj.is_need_bk)
                        {
                            _closeEventDealWnd();
                            eventDealDoneGeneralShowProcess(eventDoneInfo, null, null, _dealDone);
                        }
                        else
                        {
                            eventDealDoneGeneralShowProcess(eventDoneInfo, _closeEventDealWnd, null, _dealDone);
                        }
                    }
                }
                else
                {
                    _break?.Invoke();
                }
            };
            
            MiniGameMgr.enterGame(_m_rCommonEventMiniGameShowRefObj.mini_game_main_id, () =>
            {
                if (!isEventDoneDataLevel)//若未请求过, 则请求完成
                    reqDealEvent(null, reqCallBack);//向服务器发起完成事件请求
            }, _isSucc =>
            {
                if (_isSucc)//若已经完成 且 还未向服务器发起完成事件请求
                {
                    if (!isEventDoneDataLevel)//若未请求过, 则请求完成
                    {
                        //完成游戏后, 向服务器发起完成事件请求
                        reqDealEvent(null, reqCallBack);    
                    }
                    // else//若请求过完成, 会走上面请求完成表现, 那边会调用_dealDone, 这里就不调用了
                    // {
                    //     _dealDone?.Invoke();
                    // }
                }
                else
                {
                    _break?.Invoke();
                }
            });
            _startDeal?.Invoke();
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
        
        private void _closeEventDealWnd()
        {
            MiniGameMgr.exitGame();
        }

        public override string eventName { get { return _m_rCommonEventMiniGameShowRefObj?.event_name; } }
        public override string eventSimpleDesc { get { return _m_rCommonEventMiniGameShowRefObj?.event_simple_desc; } }
        public override NPGTextureIndex inEventListIcon { get { return _m_rCommonEventMiniGameShowRefObj?.event_list_icon; } }
        public override string eventDetailDesc { get { return _m_rCommonEventMiniGameShowRefObj?.event_detail_desc; } }
        public override void forceBreakEventDeal()
        {
            _closeEventDealWnd();
        }

        protected override CommonEventShowResultInfo _makeEventShowResultInfo(CommonEvent_DoneInfo _eventDoneInfo)
        {
            if (_eventDoneInfo == null)
                return null;
            
            //奖励事件显示结果时不需要什么额外的展示信息
            CommonEventShowResultInfo resultInfo = new CommonEventShowResultInfo(
                _m_rCommonEventMiniGameShowRefObj?.event_result_title, _m_rCommonEventMiniGameShowRefObj?.event_result_desc, _eventDoneInfo);
            return resultInfo;
        }
        
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