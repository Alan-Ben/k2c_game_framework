using Common.MarsEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class ExploreTeamNormalMarsBuildingView : CommonNormalMarsBuildingView
    {
        private GGUIWndMarsBuildingRepairBtnFollowerController _m_repairBtn;
        private GGUIWndMarsBuildingRepairTimeFollowerController _m_repairTimeFollower;
        
        
        public ExploreTeamNormalMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
        
        
        protected override void _onInitDone()
        {
            base._onInitDone();

            NPPlayer.instance.marsComp.exploreSubComponent.onTeamStateChg += _onTeamDataChg;
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamHeroChg += _onTeamDataChg;
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_TEAM_SOLDIER_NUM_CHG, _checkShowTeamRepairBtn);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_EXPLORE_TEAM_NORMAL_MARS_BUILDING, _onSimulateClick);
            _checkShowTeamRepairBtn();
        }
        protected override void _onDiscard()
        {
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamHeroChg -= _onTeamDataChg;
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamStateChg -= _onTeamDataChg;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_TEAM_SOLDIER_NUM_CHG, _checkShowTeamRepairBtn);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_EXPLORE_TEAM_NORMAL_MARS_BUILDING, _onSimulateClick);

            _m_repairBtn?.discard();
            _m_repairTimeFollower?.discard();
            _m_repairTimeFollower = null;

            base._onDiscard();
        }


        protected override void _triggerClickInternal()
        {
            if (followTarget == null)
                return;

            GGUIWndMarsBuildingTeamExploreOperationBtnsFollowerController operationPanel = new GGUIWndMarsBuildingTeamExploreOperationBtnsFollowerController();
            operationPanel.refreshWnd(this);
            GGUIWndMarsHud.instance.addMutexController(followTarget, operationPanel);
        }

        public override void tick()
        {
            _m_repairTimeFollower?.tick();
        }

        private void _onSimulateClick()
        {
            triggerClick();
        }
        private void _onTeamDataChg(MarsExploreTeamInfo _)
        {
            _checkShowTeamRepairBtn();
        }
        private void _checkShowTeamRepairBtn()
        {
            if (followTarget == null)
                return;
            
            bool needShowRepairBtn = false;
            MarsExploreTeamInfo repairTeamInfo = null;
            NPPlayer.instance.marsComp.exploreSubComponent.actionWithAllTeam(_teamInfo =>
            {
                if (_teamInfo is not { isUnlock: true })
                    return true;

                if (_teamInfo.heroCount <= 0 || (_teamInfo.state == EMarsExploreTeamState.IDLE && !_teamInfo.isSoldierFull))
                    needShowRepairBtn = true;
                
                if(_teamInfo.state == EMarsExploreTeamState.REPAIR && repairTeamInfo == null)
                    repairTeamInfo = _teamInfo;

                if(needShowRepairBtn && repairTeamInfo != null)
                    return false;
                
                return true;
            });

            if (needShowRepairBtn)
            {
                if (_m_repairBtn == null)
                {
                    _m_repairBtn = new GGUIWndMarsBuildingRepairBtnFollowerController();
                    GGUIWndMarsHud.instance.addController(followTarget, _m_repairBtn);
                }
            }
            else
            {
                if (_m_repairBtn != null)
                {
                    _m_repairBtn.discard();
                    _m_repairBtn = null;
                }
            }

            if (repairTeamInfo != null)
            {
                if (_m_repairTimeFollower == null)
                {
                    _m_repairTimeFollower = new GGUIWndMarsBuildingRepairTimeFollowerController();
                    GGUIWndMarsHud.instance.addController(followTarget, _m_repairTimeFollower);
                }
                _m_repairTimeFollower.refreshWnd(repairTeamInfo);
            }
            else
            {
                if (_m_repairTimeFollower != null)
                {
                    _m_repairTimeFollower.discard();
                    _m_repairTimeFollower = null;
                }
            }
        }
    }
}