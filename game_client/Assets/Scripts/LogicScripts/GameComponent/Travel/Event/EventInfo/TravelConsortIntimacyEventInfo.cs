using ALPackage;
using Common.TravelObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 游历妃子亲密度事件数据
    /// </summary>
    public class TravelConsortIntimacyEventInfo : _ATravelEventInfo
    {
        private TravelEventConsortIntimacyRefObj _m_rConsortIntimacyEventRefObj;//妃子亲密度事件数据
        
        public TravelConsortIntimacyEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelConsortIntimacyEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelConsortIntimacyEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        public TravelEventConsortIntimacyRefObj consortIntimacyEventRefObj
        {
            get
            {
                if(_m_rConsortIntimacyEventRefObj == null || _m_rConsortIntimacyEventRefObj.event_id != eventId)
                    _m_rConsortIntimacyEventRefObj = GRefdataCoreMgr.instance.travelEventConsortIntimacyRefCore.getRef(eventId);

                return _m_rConsortIntimacyEventRefObj;
            }
        }
        
        protected override void _dealEvent()
        {
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    // 展示事件对话
                    _showEventDialog(_done);
                })
                .addDelegateProcess((_done) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益
                    
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealConsortIntimacyTravel(instanceId, (_msg) =>
                    {
                        if (_msg == null || _msg.getResult() == null)
                        {
                            _done?.Invoke();
                            return;
                        }

                        // 展示事件结果窗口
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
                    NPPlayer.instance.travelComp.reqDealConsortIntimacyTravel(instanceId, (_msg) =>
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
            _nodeUITag = UINodeTagConst.C_TRAVEL_CONSORT_EVENT_RESULT;

            TravelConsortIntimacyEventResultInfo resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelConsortEventResult wnd = new GGUIWndTravelConsortEventResult(resultInfo);
            return wnd;
        }
        
        public TravelConsortIntimacyEventResultInfo getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            TravelConsortIntimacyEventResultInfo resultInfo = new TravelConsortIntimacyEventResultInfo(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }
        
        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_consortIntimacyEventCenterTipText", GCommon.getItemName(ENPItemType.CONSORT, consortIntimacyEventRefObj?.consort_id ?? 0));
        }
    }
}