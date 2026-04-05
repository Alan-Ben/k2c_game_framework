using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsExploreBossEventView : _AMarsExploreEventView<MarsExploreBossEventInfo, GTDMonoMarsExploreBossEvent>
    {
        private GGUIWndMarsExploreBossEventTipFollowerController _m_tipWnd;

        private NPPlayerFixedCDInfo _m_challengeCountFixedCDInfo; // 可挑战次数CD信息
        private bool _m_bPreHasChallengeCount; // 上次是否有可挑战次数


        public MarsExploreBossEventView([NotNull] MarsExploreViewMgr _viewMgr, [NotNull] MarsExploreBossEventInfo _eventInfo)
            : base(_viewMgr, _eventInfo)
        {
        }


        /// <summary>
        /// 当前是否有可挑战次数
        /// </summary>
        private bool nowHasChallengeCount
        {
            get
            {
                return _m_challengeCountFixedCDInfo == null || _m_challengeCountFixedCDInfo.getCount() > 0;
            }
        }


        public override void updateView(bool _newEventStacked)
        {
            if (nowHasChallengeCount)
            {
                if (_m_tipWnd != null)
                {
                    _m_tipWnd.refreshWnd();
                    if (_newEventStacked)
                        _m_tipWnd.playLoadedEffect();
                }
                else
                {
                    if (followTarget != null)
                    {
                        _m_tipWnd = new GGUIWndMarsExploreBossEventTipFollowerController();
                        _m_tipWnd.refreshWnd(this);
                        if (_newEventStacked)
                            _m_tipWnd.regItemWndLoadDoneDelegate(() => { _m_tipWnd?.playLoadedEffect(); });
                        GGUIWndMarsExploreHUDRoot.instance.addController(followTarget, _m_tipWnd);
                    }
                }
            }
            else
            {
                // 没有可挑战次数时销毁提示窗口
                _m_tipWnd?.discard();
                _m_tipWnd = null;
            }
        }


        protected override void _onInitDone()
        {
            bool needShowLoadedEffect = _m_needShowLoadedEffect;
            // 获取可挑战次数CD信息
            _m_challengeCountFixedCDInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.mars_explore_daily_refresh_fixed_cd);
            _m_bPreHasChallengeCount = nowHasChallengeCount;
            
            base._onInitDone();

            updateView(needShowLoadedEffect);

            // 注册可挑战次数变化事件
            WinMsg.RegisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCdCountChg);
        }


        protected override void _onDiscard()
        {
            // 解除注册可挑战次数变化事件
            WinMsg.UnregisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCdCountChg);

            _m_challengeCountFixedCDInfo = null;
            _m_bPreHasChallengeCount = true;

            _m_tipWnd?.discard();
            _m_tipWnd = null;

            base._onDiscard();
        }


        public override void triggerClick()
        {
            if (eventInfo.isDone)
            {
                int originLevel = NPPlayer.instance.marsComp.exploreSubComponent.levelRef?.explore_level ?? 1;
                int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                NPGSClientListener.sendRequestByLog(new GC2GS_041_009_ReqGetBossDoneReward(eventInfo.instanceId), 
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_009_RetGetBossDoneReward>((_isSuc, _msg) =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                        if (!_isSuc)
                            return;
                        
                        GCommon.dealGainItem(_msg.getItemList(), TransKeyConst.common_getreward_tip, () =>
                        {
                            int newLevel = NPPlayer.instance.marsComp.exploreSubComponent.levelRef?.explore_level ?? 1;
                            if (newLevel > originLevel)
                                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreUpgrade.instance, GGUIWndMarsExploreUpgrade.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_UPGRADE);
                        });
                    }));
                return;
            }
            
            int eventNum = NPPlayer.instance.marsComp.exploreSubComponent.getEventNumAtPos(eventInfo.posId);
            // 如果还有其它事件，打开事件选择窗口
            if (eventNum > 1)
            {
                GGUIWndMarsPosItemSelect.instance.refreshWnd(eventInfo.posId);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsPosItemSelect.instance, GGUIWndMarsPosItemSelect.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_EXPLORE_POS_ITEM_SELECT, true, false);
                return;
            }
            
            GGUIWndMarsExploreBossInfo.instance.refreshWnd(specificEventInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreBossInfo.instance, GGUIWndMarsExploreBossInfo.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_BOSS_INFO);
        }


        /// <summary>
        /// 可挑战次数变化事件处理
        /// 参数: _objs[0] 为 long 类型的 fixed cd ref id
        /// </summary>
        private void _onFixedCdCountChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is long _fixedCdRefId))
                return;

            // 检查是否为火星探索的fixed cd
            if (_fixedCdRefId != GRefdataCoreMgr.instance.npGeneral.mars_explore_daily_refresh_fixed_cd)
                return;

            // 若CD信息为空，尝试重新获取
            if (_m_challengeCountFixedCDInfo == null)
                _m_challengeCountFixedCDInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.mars_explore_daily_refresh_fixed_cd);

            if (!_m_bPreHasChallengeCount && nowHasChallengeCount) // 若之前没有可挑战次数，现在有了
            {
                updateView(true);
            }
            else if (_m_bPreHasChallengeCount && !nowHasChallengeCount) // 若之前有可挑战次数，现在没有了
            {
                updateView(false);
            }

            _m_bPreHasChallengeCount = nowHasChallengeCount;
        }
    }
}
