using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsBuildingExploreEntranceBtnFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingExploreEntranceBtn, GGUIWndMarsBuildingExploreEntranceBtnFollower>
    {
        private readonly GResPathIndex _m_resIndex;


        public GGUIWndMarsBuildingExploreEntranceBtnFollowerController()
        {
            _m_resIndex = new GResPathIndex(7194);
        }
        public GGUIWndMarsBuildingExploreEntranceBtnFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingExploreEntranceBtnFollower _createItemWnd(GGUIMonoMarsBuildingExploreEntranceBtn _wndMono)
        {
            GGUIWndMarsBuildingExploreEntranceBtnFollower wnd = new GGUIWndMarsBuildingExploreEntranceBtnFollower(_wndMono);
            wnd.showWnd();
            return wnd;
        }
        public void tick()
        {
            wnd?.refreshExploreEnergyShow();
        }
    }

    public class GGUIWndMarsBuildingExploreEntranceBtnFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingExploreEntranceBtn>
    {
        public GGUIWndMarsBuildingExploreEntranceBtnFollower(GGUIMonoMarsBuildingExploreEntranceBtn _wnd) : base(_wnd)
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

            ALUGUICommon.uncombineBtnClick(wnd.btnExploreEntrance, _onBtnExploreEntranceClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnExploreEntrance, _onBtnExploreEntranceClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            refreshExploreEnergyShow();
        }
        public void refreshExploreEnergyShow()
        {
            if (wnd == null || !_m_bIsShow)   
                return;

            long lazyCdId = GRefdataCoreMgr.instance.npGeneral.mars_explore_cd;
            long count = NPPlayer.instance.lazyCdComp.getCount(lazyCdId);
            long maxCount = NPPlayer.instance.lazyCdComp.getMaxCount(lazyCdId);
            wnd.refreshExploreEnergyShow(count, maxCount);
        }


        private void _onBtnExploreEntranceClick(GameObject _obj)
        {
            GNodeMarsExplore.openOrQuitToNode(null);
        }
    }
}
