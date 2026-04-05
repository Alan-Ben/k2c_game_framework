using ALPackage;
using Common.MarsEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingRepairBtnFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingRepairBtn, GGUIWndMarsBuildingRepairBtnFollower>
    {
        private readonly GResPathIndex _m_resIndex;


        public GGUIWndMarsBuildingRepairBtnFollowerController()
        {
            _m_resIndex = new GResPathIndex(7315);
        }
        public GGUIWndMarsBuildingRepairBtnFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingRepairBtnFollower _createItemWnd(GGUIMonoMarsBuildingRepairBtn _wndMono)
        {
            GGUIWndMarsBuildingRepairBtnFollower wnd = new GGUIWndMarsBuildingRepairBtnFollower(_wndMono);
            wnd.showWnd();
            return wnd;
        }
    }

    public class GGUIWndMarsBuildingRepairBtnFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingRepairBtn>
    {
        public GGUIWndMarsBuildingRepairBtnFollower(GGUIMonoMarsBuildingRepairBtn _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_TEAM_SOLDIER_NUM_CHG, refreshWnd);
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamHeroChg += _onTeamHeroChg;
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_TEAM_SOLDIER_NUM_CHG, refreshWnd);
            NPPlayer.instance.marsComp.exploreSubComponent.onTeamHeroChg -= _onTeamHeroChg;
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnRepair, _onBtnRepairClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnRepair, _onBtnRepairClick);

            // teamSoldierRatioConfigList配置按照从小到大排序
            if (wnd.teamSoldierRatioConfigList != null)
            {
                wnd.teamSoldierRatioConfigList.Sort((_configA, _configB) =>
                {
                    if (_configB == null) return -1;
                    if (_configA == null) return 1;
                    if(ReferenceEquals(_configA, _configB)) return 0;
                    
                    return _configA.soldierRatio.CompareTo(_configB.soldierRatio);
                });
            }
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            bool hasEmptyTeam = false;//是否有未编队队伍
            float lowestSoldierRatio = 1f;//最低带兵率
            NPPlayer.instance.marsComp.exploreSubComponent.actionWithAllTeam(_teamInfo =>
            {
                if (_teamInfo is not { isUnlock: true })
                    return true;

                if (_teamInfo.heroCount <= 0)
                {
                    hasEmptyTeam = true;
                    return false;
                }

                float soldierRatio = 1f * _teamInfo.soldierNum / _teamInfo.soldierMax;
                if (soldierRatio < lowestSoldierRatio)
                    lowestSoldierRatio = soldierRatio;
                
                return true;
            });

            if (hasEmptyTeam)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasEmptyTeamShow, true);
                if (wnd.teamSoldierRatioConfigList != null)
                {
                    foreach (var config in wnd.teamSoldierRatioConfigList)
                    {
                        if(config != null)
                            ALUGUICommon.setGameObjEnable(config.greaterOrEqualShow, false);
                    }       
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasEmptyTeamShow, false);
                if (wnd.teamSoldierRatioConfigList != null && wnd.teamSoldierRatioConfigList.Count > 0)
                {
                    MarsBuildingRepairTeamSoldierRatioConfig targetConfig = null;
                    foreach (MarsBuildingRepairTeamSoldierRatioConfig config in wnd.teamSoldierRatioConfigList)
                    {
                        if (config == null)
                            continue;

                        ALUGUICommon.setGameObjEnable(config.greaterOrEqualShow, false);
                        if(config.soldierRatio > lowestSoldierRatio)
                            continue;
                
                        targetConfig = config;
                    }        
                    
                    if (targetConfig != null)
                    {
                        ALUGUICommon.setGameObjEnable(targetConfig.greaterOrEqualShow, true);
                    }
                }
            }
        }

        private void _onBtnRepairClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreTeamEdit.instance, GGUIWndMarsExploreTeamEdit.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_TEAM_EDIT);
        }
        
        private void _onTeamHeroChg(MarsExploreTeamInfo _)
        {
            refreshWnd();
        }
    }
}
