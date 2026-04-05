using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 兵工厂操作按钮控制器
    /// </summary>
    public class GGUIWndMarsBuildingArmoryOperationBtnsFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingArmoryOperationBtns, GGUIWndMarsBuildingArmoryOperationBtnsFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingArmoryOperationBtnsFollowerController()
        {
            _m_resIndex = new GResPathIndex(7300); // 火星拓展-兵工厂hud
        }
        public GGUIWndMarsBuildingArmoryOperationBtnsFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingArmoryOperationBtnsFollower _createItemWnd(GGUIMonoMarsBuildingArmoryOperationBtns _wndMono)
        {
            GGUIWndMarsBuildingArmoryOperationBtnsFollower wnd = new GGUIWndMarsBuildingArmoryOperationBtnsFollower(_wndMono);
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
    /// 兵工厂操作按钮跟随窗口
    /// </summary>
    public class GGUIWndMarsBuildingArmoryOperationBtnsFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingArmoryOperationBtns>
    {
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingArmoryOperationBtnsFollower(GGUIMonoMarsBuildingArmoryOperationBtns _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
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
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClick);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
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

            bool onUpgrading = _m_buildingView.buildingInfo.state is MarsBuildingInfo.StateType.Upgrading;
            ALUGUICommon.setGameObjEnable(wnd.onUpgradingShowGoList, onUpgrading);
            ALUGUICommon.setGameObjEnable(wnd.onUpgradingHideGoList, !onUpgrading);

            _refreshCountDown();
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshCountDown()
        {
            if(wnd == null || _m_buildingView == null)
                return;

            // 当前在升级中
            if (_m_buildingView.buildingInfo != null &&
                _m_buildingView.buildingInfo.state is MarsBuildingInfo.StateType.Upgrading)
            {
                ALUGUICommon.setLabelTxt(wnd.txtUpgradingCountDown,
                    TimeUtil.millisecondsToTime_dhms(_m_buildingView.buildingInfo.remainingBuildOrUpgradeTime));
            }
        }

        public void tick()
        {
            _refreshCountDown();
        }
        
        /// <summary>
        /// 点击详情按钮
        /// </summary>
        private void _onBtnDetailClick(GameObject _obj)
        {
            if (_m_buildingView == null || wnd == null)
                return;

            // 打开兵工厂详情窗口
            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingDetail(wnd.detailWndUIAssetPathId, _m_buildingView));
        }


        /// <summary>
        /// 点击升级按钮
        /// </summary>
        private void _onBtnUpgradeClick(GameObject _obj)
        {
            if (_m_buildingView == null || wnd == null)
                return;

            // 打开兵工厂升级窗口
            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingUpgrade(wnd.upgradeWndAssetPathId, _m_buildingView));
        }


        /// <summary>
        /// 点击加速按钮
        /// </summary>
        private void _onBtnSpeedUpClick(GameObject _obj)
        {
            if (_m_buildingView == null || _m_buildingView.buildingInfo == null)
                return;

            // 打开加速道具使用窗口
            GGUIWndMarsTimeSpeedUp.addNode(_m_buildingView.buildingInfo, _m_buildingView.buildingInfo);
        }
    }
}
