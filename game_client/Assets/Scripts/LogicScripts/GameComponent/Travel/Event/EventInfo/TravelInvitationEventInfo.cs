using ALPackage;
using Common.TravelObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 游历指定邀约事件数据
    /// </summary>
    public class TravelInvitationEventInfo : _ATravelEventInfo
    {
        private TravelEventInvitationRefObj _m_rInvitationEventRefObj;//邀约事件数据
        private _IConsortShowInfo _m_lSelectConsortInfo;
        
        public TravelInvitationEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelInvitationEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelInvitationEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        public TravelEventInvitationRefObj invitationEventRefObj
        {
            get
            {
                if(_m_rInvitationEventRefObj == null || _m_rInvitationEventRefObj.event_id != eventId)
                    _m_rInvitationEventRefObj = GRefdataCoreMgr.instance.travelEventInvitationRefCore.getRef(eventId);

                return _m_rInvitationEventRefObj;
            }
        }
        public _IConsortShowInfo selectConsortInfo { get => _m_lSelectConsortInfo; set => _m_lSelectConsortInfo = value; }
        
        protected override void _dealEvent()
        {
            _m_lSelectConsortInfo = null;
            
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    _showEventDialog(_done);
                })
                .addDelegateProcess((_done) =>
                {
                    // 显示选择妃子窗口
                    QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_INVITATION_EVENT_SELECT_CONSORT, true
                        , true, false, null, GGUIWndTravelInvitationEventSelectConsort.instance, true, false,
                        () =>
                        {
                            GGUIWndTravelInvitationEventSelectConsort.instance.setData(this);
                        }, null, null, () =>
                        {
                            // 未选择妃子
                            if (selectConsortInfo == null)
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
                    long oldExp = getExp();//获取玩家经验
                    long oldEarnings = getEarnings();//获取玩家收益
                    
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealInvitationTravel(instanceId, selectConsortInfo?.consortId ?? 0, (_msg) =>
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
            _m_lSelectConsortInfo = null;

            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    // 显示选择妃子窗口
                    QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_INVITATION_EVENT_SELECT_CONSORT, true
                        , true, false, null, GGUIWndTravelInvitationEventSelectConsort.instance, true, false,
                        () =>
                        {
                            GGUIWndTravelInvitationEventSelectConsort.instance.setData(this);
                        }, null, null, () =>
                        {
                            // 未选择妃子
                            if (selectConsortInfo == null)
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
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealInvitationTravel(instanceId, selectConsortInfo?.consortId ?? 0, (_msg) =>
                    {
                        if (_msg == null)
                        {
                            _done?.Invoke();
                            return;
                        }

                        // 显示上浮提示：邀约成功，下次约会必定遇见{妃子名}
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.travel_invitationEventSimpleResult_str, selectConsortInfo?.consortTransName));
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
            _nodeUITag = UINodeTagConst.C_TRAVEL_INVITATION_EVENT_RESULT;
                
            _ATravelSpecificEventResultInfo<TravelInvitationEventInfo> resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelInvitationEventResult resultWnd = new GGUIWndTravelInvitationEventResult(resultInfo);
            return resultWnd;
        }
        
        public _ATravelSpecificEventResultInfo<TravelInvitationEventInfo> getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            _ATravelSpecificEventResultInfo<TravelInvitationEventInfo> resultInfo = new _ATravelSpecificEventResultInfo<TravelInvitationEventInfo>(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }
        
        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_invitationEventCenterTipText");
        }
    }
}