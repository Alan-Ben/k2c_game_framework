using ALPackage;
using Common.MarsEnum;
using System.Globalization;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingTeamExploreOperationBtnsFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingTeamExploreOperationBtns, GGUIWndMarsBuildingTeamExploreOperationBtnsFollower>
    {
        private readonly GResPathIndex _m_resIndex;

        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingTeamExploreOperationBtnsFollowerController()
        {
            _m_resIndex = new GResPathIndex(7309);
        }
        public GGUIWndMarsBuildingTeamExploreOperationBtnsFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }

        
        protected override GGUIWndMarsBuildingTeamExploreOperationBtnsFollower _createItemWnd(GGUIMonoMarsBuildingTeamExploreOperationBtns _wndMono)
        {
            GGUIWndMarsBuildingTeamExploreOperationBtnsFollower wnd = new GGUIWndMarsBuildingTeamExploreOperationBtnsFollower(_wndMono);
            wnd.refreshWnd(_m_buildingView);
            wnd.showWnd();
            return wnd;
        }
        
        
        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            wnd?.refreshWnd(_m_buildingView);
        }
    }

    public class GGUIWndMarsBuildingTeamExploreOperationBtnsFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingTeamExploreOperationBtns>
    {
        private _IMarsBuildingView _m_buildingView;
        private new bool _m_bIsShow;
        
        
        public GGUIWndMarsBuildingTeamExploreOperationBtnsFollower(GGUIMonoMarsBuildingTeamExploreOperationBtns _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_TEAM_EXPLORE_REPAIR, _onSimulateClickRepair);
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg += _onBuildingStateChg;
            
            refreshWnd();

            _tryShowSelect();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_BUILDING_TEAM_EXPLORE_REPAIR, _onSimulateClickRepair);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.buildingSubComponent.onBuildingStateChg -= _onBuildingStateChg;
            
            _tryHideSelect();
            
            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRepair, _onBtnRepairClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnAssist, _onClickAssist);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
            ALUGUICommon.combineBtnClick(wnd.btnRepair, _onBtnRepairClick);     
            ALUGUICommon.combineBtnClick(wnd.btnAssist, _onClickAssist);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _tryHideSelect();
            _m_buildingView = _buildingView;
            _tryShowSelect();
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null)
                return;

            MarsBuildingInfo buildingInfo = _m_buildingView.buildingInfo;
            if (buildingInfo == null)
                return;

            bool isUpgrading = buildingInfo.state == MarsBuildingInfo.StateType.Upgrading;
            bool isConstructing = buildingInfo.state == MarsBuildingInfo.StateType.Constructing;

            bool canAskHelp = false;
            //队伍数量不多，这里直接全遍历即可
            NPPlayer.instance.marsComp.exploreSubComponent.doEachTeam(
                (_teamInfo) => {
                        if (_teamInfo != null && _teamInfo.getUIState() == EMarsExploreTeamUIState.CanAskHelp)
                        {
                            canAskHelp = true;
                        }
                    } );

            bool canAssist = NPPlayer.instance.guildMarsHelpComp.isJoinGuildAndBuildHelpBuilding() && canAskHelp;

            ALUGUICommon.setGameObjEnable(wnd.canAssistShowGos, canAssist);
            ALUGUICommon.setGameObjEnable(wnd.canAssistHideGos, !canAssist);
            
            wnd.setUpgradingState(isUpgrading || isConstructing);
        }
        
        /// <summary>
        /// 建筑状态变化事件处理
        /// </summary>
        private void _onBuildingStateChg(long _buildingId, MarsBuildingInfo.StateType _oldState, MarsBuildingInfo.StateType _newState)
        {
            var buildingRef = GRefdataCoreMgr.instance.marsBuildingRefCore.getRef(_buildingId);
            if (buildingRef != null && buildingRef.building_type == EMarsBuildingType.HELP)
            {
                if (_newState == MarsBuildingInfo.StateType.Normal)
                    refreshWnd();
            }
        }

        private void _onBtnDetailClick(GameObject _obj)
        {
            if (_m_buildingView == null || wnd == null)
                return;

            // 打开兵工厂详情窗口
            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingDetail(_m_buildingView));
        }
        private void _onBtnUpgradeClick(GameObject _obj)
        {
            if (_m_buildingView == null || wnd == null)
                return;

            // 打开兵工厂升级窗口
            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingUpgrade(_m_buildingView));
        }
        private void _onBtnSpeedUpClick(GameObject _obj)
        {
            if (_m_buildingView == null || _m_buildingView.buildingInfo == null)
                return;

            GGUIWndMarsTimeSpeedUp.addNode(_m_buildingView.buildingInfo, _m_buildingView.buildingInfo);
        }
        private void _onBtnRepairClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamEdit.instance, GGUIWndMarsExploreTeamEdit.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_EDIT);
        }
        
        private void _onClickAssist(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamEdit.instance, GGUIWndMarsExploreTeamEdit.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_EDIT);
        }
        
        private void _onSimulateClickRepair()
        {
            if (wnd == null) return;
            _onBtnRepairClick(wnd.btnRepair);
        }


        private void _tryShowSelect()
        {
            if (wnd == null || _m_buildingView == null || !_m_bIsShow)
                return;
            
            _m_buildingView.setSelected(true);
        }
        private void _tryHideSelect()
        {
            if (wnd == null || _m_buildingView == null || !_m_bIsShow)
                return;
            
            _m_buildingView.setSelected(false);
        }
    }
}
