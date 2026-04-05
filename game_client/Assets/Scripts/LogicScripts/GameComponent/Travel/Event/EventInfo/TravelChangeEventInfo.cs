using ALPackage;
using Common.TravelObj;

namespace GOE
{
    /// <summary>
    /// 游历妃子兑换事件数据
    /// </summary>
    public class TravelChangeEventInfo : _ATravelEventInfo
    {
        private TravelEventChangeRefObj _m_rChangeEventRefObj;
        private bool _m_bNeedChange;//是否交换
        
        public TravelChangeEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelChangeEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelChangeEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        public TravelEventChangeRefObj changeEventRefObj
        {
            get
            {
                if (_m_rChangeEventRefObj == null || _m_rChangeEventRefObj.event_id != eventId)
                    _m_rChangeEventRefObj = GRefdataCoreMgr.instance.travelEventChangeRefCore.getRef(eventId);

                return _m_rChangeEventRefObj;
            }
        }

        public bool needChange { get { return _m_bNeedChange; } set { _m_bNeedChange = value; } }
        
        protected override void _dealEvent()
        {
            _m_bNeedChange = changeEventRefObj?.default_change ?? false;
            
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
                    NPPlayer.instance.travelComp.reqDealChangeTravel(instanceId, _m_bNeedChange, (_msg) =>
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
            _m_bNeedChange = changeEventRefObj?.default_change ?? false;

            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益

                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealChangeTravel(instanceId, _m_bNeedChange, (_msg) =>
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
            _nodeUITag = UINodeTagConst.C_TRAVEL_EVENT_COMMON_RESULT;

            _ATravelSpecificEventResultInfo<TravelChangeEventInfo> resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelChangeEventResult resultWnd = new GGUIWndTravelChangeEventResult(resultInfo);
            return resultWnd;
        }
        
        public _ATravelSpecificEventResultInfo<TravelChangeEventInfo> getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            _ATravelSpecificEventResultInfo<TravelChangeEventInfo> resultInfo = new _ATravelSpecificEventResultInfo<TravelChangeEventInfo>(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }
        
        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_changeEventCenterTipText");
        }
    }
}