using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;

namespace GOE
{
    public class MarsExploreBattleEventView : _AMarsExploreEventView<MarsExploreBattleEventInfo, GTDMonoMarsExploreBattleEvent>
    {
        private GGUIWndMarsExploreBattleEventTipFollowerController _m_tipWnd;


        public MarsExploreBattleEventView([NotNull] MarsExploreViewMgr _viewMgr, [NotNull] MarsExploreBattleEventInfo _eventInfo)
            : base(_viewMgr, _eventInfo)
        {
        }


        public override void updateView(bool _newEventStacked)
        {
            if (_m_tipWnd != null)
            {
                _m_tipWnd.refreshWnd();
                if (_newEventStacked)
                    _m_tipWnd.playLoadedEffect();
            }
        }


        protected override void _onInitDone()
        {
            bool needShowLoadedEffect = _m_needShowLoadedEffect;
            base._onInitDone();

            if (followTarget != null)
            {
                _m_tipWnd = new GGUIWndMarsExploreBattleEventTipFollowerController();
                _m_tipWnd.refreshWnd(this);
                if (needShowLoadedEffect)
                    _m_tipWnd.regItemWndLoadDoneDelegate(() => { _m_tipWnd.playLoadedEffect(); });
                GGUIWndMarsExploreHUDRoot.instance.addController(followTarget, _m_tipWnd);
            }
        }
        protected override void _onDiscard()
        {
            _m_tipWnd?.discard();
            _m_tipWnd = null;

            base._onDiscard();
        }


        public override void triggerClick()
        {
            if (eventInfo == null)
                return;

            if (eventInfo.isDone)
            {
                int originLevel = NPPlayer.instance.marsComp.exploreSubComponent.levelRef?.explore_level ?? 1;
                int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                NPGSClientListener.sendRequestByLog(new GC2GS_041_006_ReqGetBattleDoneReward(eventInfo.instanceId), 
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_006_RetGetBattleDoneReward>((_isSuc, _msg) =>
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

            GGUIWndMarsExploreBattleInfo.instance.refreshWnd(specificEventInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreBattleInfo.instance, GGUIWndMarsExploreBattleInfo.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_BATTLE_INFO);
        }
    }
}
