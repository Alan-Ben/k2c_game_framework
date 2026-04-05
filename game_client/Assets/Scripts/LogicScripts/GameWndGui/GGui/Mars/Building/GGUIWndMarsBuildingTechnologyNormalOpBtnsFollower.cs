using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 科研所普通状态操作按钮控制器
    /// </summary>
    public class GGUIWndMarsBuildingTechnologyNormalOpBtnsFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingTechNormalOpBtns, GGUIWndMarsBuildingTechnologyNormalOpBtnsFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingTechnologyNormalOpBtnsFollowerController()
        {
            _m_resIndex = new GResPathIndex(7304); // 火星拓展-科研室hud
        }
        public GGUIWndMarsBuildingTechnologyNormalOpBtnsFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingTechnologyNormalOpBtnsFollower _createItemWnd(GGUIMonoMarsBuildingTechNormalOpBtns _wndMono)
        {
            GGUIWndMarsBuildingTechnologyNormalOpBtnsFollower wnd = new GGUIWndMarsBuildingTechnologyNormalOpBtnsFollower(_wndMono);
            wnd.refreshWnd(_m_buildingView);
            wnd.showWnd();
            return wnd;
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            wnd?.refreshWnd(_m_buildingView);
        }
        
        public void tick()
        {
            wnd?.tick();
        }
    }

    /// <summary>
    /// 科研所操作按钮跟随窗口
    /// </summary>
    public class GGUIWndMarsBuildingTechnologyNormalOpBtnsFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingTechNormalOpBtns>
    {
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingTechnologyNormalOpBtnsFollower(GGUIMonoMarsBuildingTechNormalOpBtns _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
         
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_UPGRADING_TECHNOLOGY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.technologySubComponent.upgradingTechnologyCountDownChg += _refreshCountDown;
        }
        protected override void _onHideWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_UPGRADING_TECHNOLOGY_CHG, refreshWnd);
            NPPlayer.instance.marsComp.technologySubComponent.upgradingTechnologyCountDownChg += _refreshCountDown;
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgradeBuilding, _onBtnUpgradeBuildingClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnTechUpgradeSpeedUp, _onBtnTechUpgradeSpeedUpClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnResearch, _onBtnResearchClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.combineBtnClick(wnd.btnUpgradeBuilding, _onBtnUpgradeBuildingClick);
            ALUGUICommon.combineBtnClick(wnd.btnTechUpgradeSpeedUp, _onBtnTechUpgradeSpeedUpClick);
            ALUGUICommon.combineBtnClick(wnd.btnResearch, _onBtnResearchClick);
        }


        public void refreshWnd(_IMarsBuildingView _buildingView)
        {
            _m_buildingView = _buildingView;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_buildingView == null || _m_buildingView.buildingInfo == null)
                return;

            MarsTechnologyInfo upgradingTechnology = NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo;
            bool isTechUpgrading = upgradingTechnology?.isUpgrading ?? false;
            bool isTechUpgraded = upgradingTechnology?.isUpgraded ?? false;
            
            ALUGUICommon.setGameObjEnable(wnd.hasTechOnUpgradedShowGoList, isTechUpgraded);
            ALUGUICommon.setGameObjEnable(wnd.hasTechOnUpgradingShowGoList, isTechUpgrading);
            ALUGUICommon.setGameObjEnable(wnd.noTechOnUpgradingHideGoList, !isTechUpgrading && !isTechUpgraded);

            _refreshCountDown();
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshCountDown()
        {
            if(wnd == null)
                return;

            MarsTechnologyInfo upgradingTechnology = NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo;
            
            // 当前在升级中
            if (upgradingTechnology != null && upgradingTechnology.isUpgrading)
            {
                ALUGUICommon.setLabelTxt(wnd.txtTechUpgradingCountDown, TimeUtil.millisecondsToTime_dhms(upgradingTechnology.remainingUpgradeTimeMs));
            }
        }

        public void tick()
        {
        }
        
        /// <summary>
        /// 点击详情按钮
        /// </summary>
        private void _onBtnDetailClick(GameObject _obj)
        {
            if (_m_buildingView == null || wnd == null)
                return;

            // 打开科研所详情窗口
            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingDetail(_m_buildingView));
        }


        /// <summary>
        /// 点击建筑升级按钮
        /// </summary>
        private void _onBtnUpgradeBuildingClick(GameObject _obj)
        {
            if (_m_buildingView == null || wnd == null)
                return;

            // 打开科研所升级窗口
            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingUpgrade(_m_buildingView));
        }


        /// <summary>
        /// 点击加速升级科技按钮
        /// </summary>
        private void _onBtnTechUpgradeSpeedUpClick(GameObject _obj)
        {
            MarsTechnologyInfo upgradingTechnology = NPPlayer.instance.marsComp.technologySubComponent.nowdUpgradingOrUpgradedTechnologyInfo;
            if(upgradingTechnology == null)
                return;
            
            GGUIWndMarsTimeSpeedUp.addNode(upgradingTechnology, upgradingTechnology);
        }

        /// <summary>
        /// 点击研究按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnResearchClick(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeMarsTechnologyTree(EMarsTechnologyType.NONE));
        }
    }
}
