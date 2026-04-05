using ALPackage;
using Common.TravelObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历增加实力事件数据
    /// </summary>
    public class TravelAddPowerEventInfo : _ATravelEventInfo
    {
        private TravelEventAddPowerRefObj _m_rAddPowerEventRefObj;//增加实力事件数据
        
        private _IHeroCardShow _m_selectedHeroInfo;//选中的大臣信息
        
        public TravelAddPowerEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelAddPowerEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelAddPowerEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        public TravelEventAddPowerRefObj addPowerEventRefObj
        {
            get
            {
                if(_m_rAddPowerEventRefObj == null || _m_rAddPowerEventRefObj.event_id != eventId)
                    _m_rAddPowerEventRefObj = GRefdataCoreMgr.instance.travelEventAddPowerRefCore.getRef(eventId);

                return _m_rAddPowerEventRefObj;
            }
        }

        public _IHeroCardShow selectedHeroInfo { get => _m_selectedHeroInfo; set => _m_selectedHeroInfo = value; }
        
        protected override void _dealEvent()
        {
            _m_selectedHeroInfo = null;
            
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    _showEventDialog(_done);
                })
                .addDelegateProcess((_done) =>
                {
                    // 显示选择大臣窗口
                    QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_ADD_POWER_DEAL_WND, true
                        , true, false, null, GGUIWndTravelAddPowerEvent.instance, true, false,
                        () =>
                        {
                            GGUIWndTravelAddPowerEvent.instance.setData(this);
                        }, null, null, () =>
                        {
                            // 未选择妃子
                            if (_m_selectedHeroInfo == null)
                            {
                                breakEventDeal();//中断事件处理
                            }
                            // 选择了妃子
                            else
                            {
                                _done?.Invoke();
                            }
                        }));
                })
                .addDelegateProcess((_done) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益
                    
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealAddPowerTravel(instanceId, _m_selectedHeroInfo?.id ?? 0, (_msg) =>
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
            _m_selectedHeroInfo = null;

            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    // 显示选择大臣窗口
                    QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_ADD_POWER_DEAL_WND, true
                        , true, false, null, GGUIWndTravelAddPowerEvent.instance, true, false,
                        () =>
                        {
                            GGUIWndTravelAddPowerEvent.instance.setData(this);
                        }, null, null, () =>
                        {
                            // 未选择大臣
                            if (_m_selectedHeroInfo == null)
                            {
                                breakEventDeal();//中断事件处理
                            }
                            // 选择了大臣
                            else
                            {
                                _done?.Invoke();
                            }
                        }));
                })
                .addDelegateProcess((_done) =>
                {
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealAddPowerTravel(instanceId, _m_selectedHeroInfo?.id ?? 0, (_msg) =>
                    {
                        if (_msg == null)
                        {
                            _done?.Invoke();
                            return;
                        }

                        // 显示上浮提示：XX伙伴实力+{增加的实力}
                        string heroName = GCommon.getItemName(ENPItemType.HERO, _m_selectedHeroInfo?.id ?? 0);
                        int addPower = addPowerEventRefObj?.add_power ?? 0;
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.travel_heroAddPowerTip_str_num, heroName, addPower));
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
            _nodeUITag = UINodeTagConst.C_TRAVEL_ADD_POWER_RESULT;
            
            _ATravelSpecificEventResultInfo<TravelAddPowerEventInfo> resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelAddPowerEventResult resultWnd = new GGUIWndTravelAddPowerEventResult(resultInfo);
            return resultWnd;
        }

        public _ATravelSpecificEventResultInfo<TravelAddPowerEventInfo> getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            _ATravelSpecificEventResultInfo<TravelAddPowerEventInfo> resultInfo = new _ATravelSpecificEventResultInfo<TravelAddPowerEventInfo>(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }

        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_addPowerEventCenterTipText");
        }
    }
}