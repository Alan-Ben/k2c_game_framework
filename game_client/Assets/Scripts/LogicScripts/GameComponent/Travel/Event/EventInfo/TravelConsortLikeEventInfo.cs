using System.Collections.Generic;
using ALPackage;
using Common.TravelObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 游历妃子好感度事件数据
    /// </summary>
    public class TravelConsortLikeEventInfo : _ATravelEventInfo
    {
        private TravelEventConsortLikeRefObj _m_rConsortLikeEventRefObj;//妃子好感度事件数据
        public TravelConsortLikeEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelConsortLikeEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelConsortLikeEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        public TravelEventConsortLikeRefObj consortLikeEventRefObj
        {
            get
            {
                if(_m_rConsortLikeEventRefObj == null || _m_rConsortLikeEventRefObj.event_id != eventId)
                    _m_rConsortLikeEventRefObj = GRefdataCoreMgr.instance.travelEventConsortLikeRefCore.getRef(eventId);

                return _m_rConsortLikeEventRefObj;
            }
        }
        
        protected override void _dealEvent()
        {
            Travel_EventResult travelEventResult = null;
            long oldEarnings = getEarnings();//获取玩家收益

            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(2);
                    stepCounter.regAllDoneDelegate(_done);
                    
                    // 展示事件对话
                    _showEventDialog(() =>
                    {
                        // 显示好感度增加提示
                        GGUIWndTravelResult_MeetConsortAddLikeTip.showMeetConsortAddLikeTipWnd(stepCounter.addDoneStepCount);
                    });
                    
                    // 同步向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealConsortLikeTravel(instanceId, (_msg) =>
                    {
                        if (_msg == null || _msg.getResult() == null)
                        {
                            stepCounter.addDoneStepCount();
                            return;
                        }

                        travelEventResult = _msg.getResult();
                        stepCounter.addDoneStepCount();
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                // 展示好感度增加对话
                .addDelegateProcess((_done) =>
                {
                    List<WCGPairIntLong> stageChgList = NPPlayer.instance.travelComp.getAndRemoveLikeStageChgList(consortLikeEventRefObj?.consort_id ?? 0);//获取妃子好感度阶段变化列表
                    if(stageChgList == null || stageChgList.Count <= 0)//若不存在好感度阶段变化, 展示普通好感度事件对话
                    {
                        TravelConsortRefObj travelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(consortLikeEventRefObj?.consort_id ?? 0);
                        // 不存在对话需要展示
                        if(travelConsortRefObj == null || travelConsortRefObj.normal_like_dialog_list == null || travelConsortRefObj.normal_like_dialog_list.Count <= 0)
                        {
                            _done?.Invoke();
                            return;
                        }
                    
                        GCommon.enterDialogueNode(travelConsortRefObj.normal_like_dialog_list.GetRandomItem(), _done, true, false);
                    }
                    else//展示阶段变化对话
                    {
                        ALProcess showDialogProcess = ALProcess.CreateProcess();
                        foreach (WCGPairIntLong needShowLikeStageChgDialog in stageChgList)
                        {
                            if (needShowLikeStageChgDialog == null)
                                continue;

                            showDialogProcess.addDelegateProcess((_showDialogDone) =>
                            {
                                GCommon.enterDialogueNode(needShowLikeStageChgDialog.second(), _showDialogDone, true, false);
                            });
                        }
                        showDialogProcess.addProcess(_done);
                        showDialogProcess.deal();
                    }
                })
                .addDelegateProcess((_done) =>
                {
                    // 展示事件结果
                    _showEventResultWnd(travelEventResult, oldEarnings, null, _done);
                })
                .addDelegateProcess((_done) =>
                {
                    // 尝试进行获取妃子表现
                    NPPlayer.instance.travelComp.tryShowGainConsort(consortLikeEventRefObj?.consort_id ?? 0, _done);
                })
                .addProcess(() =>
                {
                    setEventDealDone();
                })
                .deal();
        }

        protected override void _dealEventSimple()
        {
            Travel_EventResult travelEventResult = null;
            long oldEarnings = getEarnings();//获取玩家收益

            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealConsortLikeTravel(instanceId, (_msg) =>
                    {
                        if (_msg == null || _msg.getResult() == null)
                        {
                            _done?.Invoke();
                            return;
                        }

                        travelEventResult = _msg.getResult();
                        _done?.Invoke();
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                .addDelegateProcess((_done) =>
                {
                    // 尝试进行获取妃子表现
                    NPPlayer.instance.travelComp.tryShowGainConsort(consortLikeEventRefObj?.consort_id ?? 0, _done);
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

            TravelConsortLikeEventResultInfo resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelConsortEventResult wnd = new GGUIWndTravelConsortEventResult(resultInfo);
            return wnd;
        }
        
        public TravelConsortLikeEventResultInfo getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            TravelConsortLikeEventResultInfo resultInfo = new TravelConsortLikeEventResultInfo(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }
        
        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_consortLikeEventCenterTipText", GCommon.getItemName(ENPItemType.CONSORT, consortLikeEventRefObj?.consort_id ?? 0));
        }
    }
}