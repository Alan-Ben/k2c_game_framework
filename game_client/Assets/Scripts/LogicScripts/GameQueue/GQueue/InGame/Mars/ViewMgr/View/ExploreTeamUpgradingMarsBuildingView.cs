using Common.MarsEnum;
using JetBrains.Annotations;

namespace GOE
{
    public class ExploreTeamUpgradingMarsBuildingView : CommonUpgradingMarsBuildingView
    {
        private GGUIWndMarsBuildingRepairBtnFollowerController _m_repairBtn;
        
        
        public ExploreTeamUpgradingMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
        
        
        protected override void _onInitDone()
        {
            base._onInitDone();

            NPPlayer.instance.marsComp.exploreSubComponent.onTeamStateChg += _onTeamDataChg;
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamHeroChg += _onTeamDataChg;
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_TEAM_SOLDIER_NUM_CHG, _checkShowTeamRepairBtn);
            _checkShowTeamRepairBtn();
        }
        protected override void _onDiscard()
        {
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamHeroChg -= _onTeamDataChg;
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamStateChg -= _onTeamDataChg;
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_TEAM_SOLDIER_NUM_CHG, _checkShowTeamRepairBtn);

            _m_repairBtn?.discard();
            
            base._onDiscard();
        }


        public override void triggerClick()
        {
            if (followTarget == null)
                return;

            GGUIWndMarsBuildingTeamExploreOperationBtnsFollowerController operationPanel = new GGUIWndMarsBuildingTeamExploreOperationBtnsFollowerController();
            operationPanel.refreshWnd(this);
            GGUIWndMarsHud.instance.addMutexController(followTarget, operationPanel);
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
            NPPlayer.instance.marsComp.exploreSubComponent.actionWithAllTeam(_teamInfo =>
            {
                if (_teamInfo is not { isUnlock: true })
                    return true;

                if (_teamInfo.heroCount <= 0)
                {
                    needShowRepairBtn = true;
                    return false;
                }

                if (_teamInfo.state == EMarsExploreTeamState.IDLE && !_teamInfo.isSoldierFull)
                {
                    needShowRepairBtn = true;
                    return false;
                }

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
        }
    }
}