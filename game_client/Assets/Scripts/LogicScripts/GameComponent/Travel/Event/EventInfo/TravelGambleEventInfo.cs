using System;
using ALPackage;
using Common.TravelObj;
using NPCommon;
using System.Collections.Generic;
using Common.TravelEnum;

namespace GOE
{
    /// <summary>
    /// 游历博彩事件下注状态
    /// </summary>
    public enum ETravelGambleAnteState
    {
        WAITING_ANTE,//等待下注
        ANTED,//已下注
        WAIVE,//放弃
    }
    
    /// <summary>
    /// 游历博彩事件数据
    /// </summary>
    public class TravelGambleEventInfo : _ATravelEventInfo
    {
        private TravelEventGambleRefObj _m_rGambleEventRefObj;
        private ETravelGambleAnteState _m_eAnteState;
        private int _m_selectedAnteCount;

        private string _m_sOptionTag;//选择选项tag

        public string selectedOptionTag
        {
            get => _m_sOptionTag;
            set => _m_sOptionTag = value ?? string.Empty;
        }

        public TravelGambleEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId) : base(_instanceId, _eventRefObj, _posId)
        {
        }

        /// <summary>
        /// 博彩事件配表数据
        /// </summary>
        public TravelEventGambleRefObj gambleEventRefObj
        {
            get
            {
                if (_m_rGambleEventRefObj == null || _m_rGambleEventRefObj.event_id != eventId)
                    _m_rGambleEventRefObj = GRefdataCoreMgr.instance.travelEventGambleRefCore.getRef(eventId);
                return _m_rGambleEventRefObj;
            }
        }

        public ETravelGambleAnteState anteState
        {
            get => _m_eAnteState;
            set => _m_eAnteState = value;
        }

        /// <summary>
        /// 玩家选择的押注数量（由 GGUIWndTravelGambleAnte 写回）
        /// </summary>
        public int selectedAnteCount 
        { 
            get => _m_selectedAnteCount;
            set
            {
                _m_selectedAnteCount = value;
                _m_selectedAnteCount = _m_selectedAnteCount < (_m_rGambleEventRefObj?.min_ante ?? 0) ? 0 : _m_selectedAnteCount;//设置的下注金额不可小于最小押注金额, 小于最小押注金额, 押注金额置为0
            }
        }

        protected override void _dealEvent()
        {
            _m_eAnteState = ETravelGambleAnteState.WAITING_ANTE;
            selectedAnteCount = 0;
            _m_sOptionTag = string.Empty;

            _dealGambleEvent(true);
        }

        protected override void _dealEventSimple()
        {
            _m_eAnteState = ETravelGambleAnteState.WAITING_ANTE;
            selectedAnteCount = 0;
            _m_sOptionTag = string.Empty;

            _dealGambleEvent(false);
        }

        protected override _ITravelResultWnd _getEventResultWnd(Travel_EventResult _serverResultInfo, long _oldEarnings, out string _nodeUITag)
        {
            _nodeUITag = UINodeTagConst.C_TRAVEL_GAMBLE_EVENT_RESULT;

            TravelGambleEventResultInfo resultInfo = getEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelGambleEventResult resultWnd = new GGUIWndTravelGambleEventResult(resultInfo);
            return resultWnd;
        }

        public TravelGambleEventResultInfo getEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            TravelGambleEventResultInfo resultInfo = new TravelGambleEventResultInfo(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }

        public override string getEventCenterTipText()
        {
            return TextTranslate.instance.getLanguage("#1_travel_gambleEventCenterTipText");
        }

        private void _dealGambleEvent(bool _needShowDialog)
        {
            ALProcess process = ALProcess.CreateProcess();
            if (_needShowDialog)
            {
                process.addDelegateProcess((_done) =>
                {
                    _showEventDialog(_done);
                });
            }

            process
                .addDelegateProcess((_done) =>
                {
                    // 显示事件窗口
                    QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(
                        EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_GAMBLE_EVENT_SHOW_WND, false,
                        false, false, null, GGUIWndTravelGambleEvent.instance, true, false,
                        () =>
                        {
                            GGUIWndTravelGambleEvent.instance.setData(this);
                        }, null, null, () =>
                        {
                            _done?.Invoke();
                        }));
                    // 打开下注窗口
                    GGUIWndTravelGambleEvent.instance.openAnteWnd();
                })
                .addProcess(() =>
                {
                    // 若事件在数据层面完成
                    if (inDataLevelEventDone)
                    {
                        setEventDealDone();
                        return;
                    }

                    switch (_m_eAnteState)
                    {
                        case ETravelGambleAnteState.WAIVE://若放弃
                            NPPlayer.instance.travelComp.reqDealGambleTravel(instanceId, _m_selectedAnteCount, true,
                                (_isSucc, _retMsg) =>
                                {
                                    if (_isSucc)
                                    {
                                        setEventDealDone();
                                    }
                                    else
                                    {
                                        breakEventDeal();
                                    }
                                });
                            break;
                        
                        default://其他状态退出了事件处理窗口, 都算做中断处理
                            breakEventDeal();
                            break;
                    }
                })
                .deal();
        }
    }

    /// <summary>
    /// 游历博彩事件结果信息
    /// </summary>
    public class TravelGambleEventResultInfo : CommonTravelEventResultInfo
    {
        private TravelGambleEventInfo _m_gambleEventInfo;
        private Travel_GambleResult _m_gambleResult;

        public TravelGambleEventResultInfo(TravelGambleEventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _oldEarnings)
            : base(_eventInfo, _rewardList, _extData, _oldEarnings)
        {
            _m_gambleEventInfo = _eventInfo;

            if (_extData != null && _extData.Length > 0)
            {
                _m_gambleResult = new Travel_GambleResult();
                _m_gambleResult.readPackage(_extData);
            }
        }

        public TravelGambleEventInfo gambleEventInfo { get { return _m_gambleEventInfo; } }
        public Travel_GambleResult gambleResult { get { return _m_gambleResult; } }
        public long diamondChange { get { return _m_gambleResult?.getDiamondChange() ?? 0; } }
        public ETravelGambleResult travelGambleResult { get { return _m_gambleResult?.getResultType() ?? ETravelGambleResult.NONE; } }
        public long betAmount { get { return _m_gambleResult?.getBetAmount() ?? 0; } }
    }
}
