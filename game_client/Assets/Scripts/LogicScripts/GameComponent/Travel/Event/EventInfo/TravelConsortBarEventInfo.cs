using System.Collections.Generic;
using ALPackage;
using Common.TravelObj;

namespace GOE
{
    /// <summary>
    /// 游历妃子酒馆事件数据
    /// </summary>
    public class TravelConsortBarEventInfo : _ATravelEventInfo
    {
        private TravelEventConsortBarRefObj _m_rConsortBarEventRefObj;//妃子酒馆事件数据
        private TravelEventConsortRole _m_eventMainRole;//事件主角色
        
        private _IConsortShowInfo _m_selectedConsortInfo;//选中的妃子数据
        private ETravelConsortUnlockStat _m_selectConsortUnlockStat;//选中的妃子解锁状态
        private TravelEventConsortBarCostRefObj _m_selectBarCostRefObj;//妃子酒馆事件消耗数据
        
        public TravelConsortBarEventInfo(Travel_Event _event) : base(_event)
        {
        }

        public TravelConsortBarEventInfo(long _instanceId, long _eventId, long _posId) : base(_instanceId, _eventId, _posId)
        {
        }

        public TravelConsortBarEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }
        
        public TravelEventConsortBarRefObj consortBarEventRefObj
        {
            get
            {
                if(_m_rConsortBarEventRefObj == null || _m_rConsortBarEventRefObj.event_id != eventId)
                    _m_rConsortBarEventRefObj = GRefdataCoreMgr.instance.travelEventConsortBarRefCore.getRef(eventId);

                return _m_rConsortBarEventRefObj;
            }
        }

        /// <summary>
        /// 选中的妃子数据
        /// </summary>
        public _IConsortShowInfo selectConsortShowInfo
        {
            get => _m_selectedConsortInfo;
            set
            {
                _m_selectedConsortInfo = value;
                _m_selectConsortUnlockStat = NPPlayer.instance.travelComp.getTravelConsortUnlockStat(_m_selectedConsortInfo?.consortId ?? 0);
                _m_eventMainRole = null;
            }
        }

        public ETravelConsortUnlockStat selectConsortUnlockStat { get => _m_selectConsortUnlockStat; }

        /// <summary>
        /// 酒馆事件消耗数据
        /// </summary>
        public TravelEventConsortBarCostRefObj selectBarCostRefObj { get => _m_selectBarCostRefObj; set => _m_selectBarCostRefObj = value; }

        public override _ITravelEventRole getEventMainRole()
        {
            if (_m_eventMainRole == null && _m_selectedConsortInfo != null)
                _m_eventMainRole = new TravelEventConsortRole(_m_selectedConsortInfo.consortId);

            return _m_eventMainRole;
        }

        protected override void _dealEvent()
        {
            // 重置选择妃子数据
            selectConsortShowInfo = null;
            // 重置选中消耗数据
            selectBarCostRefObj = null;
            
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    _showEventDialog(_done);
                })
                .addDelegateProcess((_done) =>
                {
                    // 显示选择妃子窗口
                    GGUIWndTravelConsortBarEventChooseConsort.showTravelConsortBarEventWnd(this, () =>
                    {
                        if (selectConsortShowInfo == null || selectBarCostRefObj == null)
                        {
                            breakEventDeal();//中断事件处理
                        }
                        else
                        {
                            _done?.Invoke();
                        }
                    });
                })
                .addDelegateProcess((_dealServerDone) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益
                    
                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealConsortBarTravel(instanceId, selectConsortShowInfo?.consortId ?? 0, selectBarCostRefObj?.id ?? 0, (_msg) =>
                    {
                        if (_msg == null || _msg.getResult() == null)
                        {
                            _dealServerDone?.Invoke();
                            return;
                        }

                        TravelConsortRefObj travelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(selectConsortShowInfo?.consortId ?? 0);
                        ALProcess resultShowProcess = ALProcess.CreateProcess();
                        resultShowProcess
                            .addDelegateProcess((_showDialogDone) =>
                            {
                                if (selectConsortUnlockStat == ETravelConsortUnlockStat.UNLOCK)
                                {
                                    // 显示亲密度增加对话
                                    if (travelConsortRefObj != null && travelConsortRefObj.bar_intimacy_dialog_list != null && travelConsortRefObj.bar_intimacy_dialog_list.Count > 0)
                                    {
                                        long showDialogId = travelConsortRefObj.bar_intimacy_dialog_list.GetRandomItem();
                                        GCommon.enterDialogueNode(showDialogId, _showDialogDone, true, false);
                                    }
                                    else
                                    {
                                        _showDialogDone?.Invoke();
                                    }
                                }
                                else
                                {
                                    GGUIWndTravelResult_MeetConsortAddLikeTip.showMeetConsortAddLikeTipWnd(() =>
                                    {
                                        List<WCGPairIntLong> stageChgList = NPPlayer.instance.travelComp.getAndRemoveLikeStageChgList(selectConsortShowInfo?.consortId ?? 0); //获取妃子好感度阶段变化列表
                                        if (stageChgList == null || stageChgList.Count <= 0) //若不存在好感度阶段变化, 展示普通好感度事件对话
                                        {
                                            // 显示好感度增加对话
                                            if (travelConsortRefObj != null && travelConsortRefObj.normal_like_dialog_list != null && travelConsortRefObj.normal_like_dialog_list.Count > 0)
                                            {
                                                GCommon.enterDialogueNode(travelConsortRefObj.normal_like_dialog_list.GetRandomItem(), _showDialogDone, true, false);
                                            }
                                            else
                                            {
                                                _showDialogDone?.Invoke();
                                            }
                                        }
                                        else //展示好感度阶段变化对话
                                        {
                                            ALProcess showDialogProcess = ALProcess.CreateProcess();
                                            foreach (WCGPairIntLong needShowLikeStageChgDialog in stageChgList)
                                            {
                                                if (needShowLikeStageChgDialog == null)
                                                    continue;

                                                showDialogProcess.addDelegateProcess((_done) =>
                                                {
                                                    GCommon.enterDialogueNode(needShowLikeStageChgDialog.second(),
                                                        _done, true, false);
                                                });
                                            }

                                            showDialogProcess.addProcess(_showDialogDone);
                                            showDialogProcess.deal();
                                        }
                                    });
                                }
                            })
                            .addDelegateProcess((_showEventResultDone) =>
                            {
                                _showEventResultWnd(_msg.getResult(), oldEarnings, null, _showEventResultDone);
                            })
                            .addProcess(_dealServerDone);
                        resultShowProcess.deal();
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                .addDelegateProcess((_done) =>
                {
                    // 尝试进行获取妃子表现
                    NPPlayer.instance.travelComp.tryShowGainConsort(selectConsortShowInfo?.consortId ?? 0, _done);
                })
                .addProcess(() =>
                {
                    setEventDealDone();
                })
                .deal();
        }

        protected override void _dealEventSimple()
        {
            // 重置选择妃子数据
            selectConsortShowInfo = null;
            // 重置选中消耗数据
            selectBarCostRefObj = null;

            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_done) =>
                {
                    // 显示选择妃子窗口
                    GGUIWndTravelConsortBarEventChooseConsort.showTravelConsortBarEventWnd(this, () =>
                    {
                        if (selectConsortShowInfo == null || selectBarCostRefObj == null)
                        {
                            breakEventDeal();//中断事件处理
                        }
                        else
                        {
                            _done?.Invoke();
                        }
                    });
                })
                .addDelegateProcess((_dealServerDone) =>
                {
                    long oldEarnings = getEarnings();//获取玩家收益

                    // 向服务器请求处理事件
                    NPPlayer.instance.travelComp.reqDealConsortBarTravel(instanceId, selectConsortShowInfo?.consortId ?? 0, selectBarCostRefObj?.id ?? 0, (_msg) =>
                    {
                        _dealServerDone?.Invoke();
                    }, () =>
                    {
                        breakEventDeal();//中断事件处理
                    });
                })
                .addDelegateProcess((_done) =>
                {
                    // 尝试进行获取妃子表现
                    NPPlayer.instance.travelComp.tryShowGainConsort(selectConsortShowInfo?.consortId ?? 0, _done);
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

            TravelConsortBarEventResultInfo resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelConsortEventResult wnd = new GGUIWndTravelConsortEventResult(resultInfo);
            return wnd;
        }
        
        public TravelConsortBarEventResultInfo getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            TravelConsortBarEventResultInfo resultInfo = new TravelConsortBarEventResultInfo(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }
        
        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_consortBarEventCenterTipText");
        }
    }
}