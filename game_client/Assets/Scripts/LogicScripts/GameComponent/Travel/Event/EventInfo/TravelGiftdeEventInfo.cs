using ALPackage;
using Common.TravelObj;

namespace GOE
{
    /// <summary>
    /// 游历卷王事件数据
    /// </summary>
    public class TravelGiftdeEventInfo : _ATravelEventInfo
    {
        private TravelEventGiftedRefObj _m_rGiftdeEventRefObj;//卷王事件数据
        
        public TravelGiftdeEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelGiftdeEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelGiftdeEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        public TravelEventGiftedRefObj giftedEventRefObj
        {
            get
            {
                if(_m_rGiftdeEventRefObj == null || _m_rGiftdeEventRefObj.event_id != eventId)
                    _m_rGiftdeEventRefObj = GRefdataCoreMgr.instance.travelEventGiftedRefCore.getRef(eventId);

                return _m_rGiftdeEventRefObj;
            }
        }
        
        protected override void _dealEvent()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    _showEventDialog(_done);
                })
                .addDelegateProcess((_done) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益
                    
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealGiftedTravel(instanceId, (_msg) =>
                    {
                        if (_msg == null || _msg.getResult() == null)
                        {
                            _done?.Invoke();
                            return;
                        }

                        _showEventResultWnd(_msg.getResult(), oldEarnings, null, _done);
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                .addProcess(() =>
                {
                    setEventDealDone();
                })
                .deal();
        }

        protected override void _dealEventSimple()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益

                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealGiftedTravel(instanceId, (_msg) =>
                    {
                        _done?.Invoke();
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                .addProcess(() =>
                {
                    setEventDealDone();
                })
                .deal();
        }

        protected override _ITravelResultWnd _getEventResultWnd(Travel_EventResult _serverResultInfo, long _oldEarnings, out string _nodeUITag)
        {
            _nodeUITag = UINodeTagConst.C_TRAVEL_GIFTED_EVENT_RESULT;
            
            _ATravelSpecificEventResultInfo<TravelGiftdeEventInfo> resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelGiftedEventResult resultWnd = new GGUIWndTravelGiftedEventResult(resultInfo);
            return resultWnd;
        }
        
        public _ATravelSpecificEventResultInfo<TravelGiftdeEventInfo> getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            _ATravelSpecificEventResultInfo<TravelGiftdeEventInfo> resultInfo = new _ATravelSpecificEventResultInfo<TravelGiftdeEventInfo>(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }
        
        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_giftdeEventCenterTipText");
        }
    }
}