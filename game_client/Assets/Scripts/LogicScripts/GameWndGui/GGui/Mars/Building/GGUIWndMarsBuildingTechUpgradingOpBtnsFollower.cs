using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingTechUpgradingOpBtnsFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingTechUpgradingOpBtns, GGUIWndMarsBuildingTechUpgradingOpBtnsFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingTechUpgradingOpBtnsFollowerController()
        {
            _m_resIndex = new GResPathIndex(7316); // 火星拓展-科研室hud
        }
        public GGUIWndMarsBuildingTechUpgradingOpBtnsFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingTechUpgradingOpBtnsFollower _createItemWnd(GGUIMonoMarsBuildingTechUpgradingOpBtns _wndMono)
        {
            GGUIWndMarsBuildingTechUpgradingOpBtnsFollower wnd = new GGUIWndMarsBuildingTechUpgradingOpBtnsFollower(_wndMono);
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
    /// 科研所升级中状态操作按钮跟随窗口
    /// </summary>
    public class GGUIWndMarsBuildingTechUpgradingOpBtnsFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingTechUpgradingOpBtns>
    {
        private _IMarsBuildingView _m_buildingView;


        public GGUIWndMarsBuildingTechUpgradingOpBtnsFollower(GGUIMonoMarsBuildingTechUpgradingOpBtns _wnd) : base(_wnd)
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
            ALUGUICommon.uncombineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnResearch, _onBtnResearchClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
            ALUGUICommon.combineBtnClick(wnd.btnSpeedUp, _onBtnSpeedUpClick);
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

            // 打开研究所详情窗口
            QueueMgr.instance.AddNode(new GNodeMarsExpandBuildingDetail(_m_buildingView));
        }

        /// <summary>
        /// 点击加速按钮
        /// </summary>
        private void _onBtnSpeedUpClick(GameObject _obj)
        {
            if (_m_buildingView == null || _m_buildingView.buildingInfo == null)
                return;

            GGUIWndMarsTimeSpeedUp.addNode(_m_buildingView.buildingInfo, _m_buildingView.buildingInfo);
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