using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreHomeBaseHUDNameFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsExploreHomeBaseHUDName, GGUIWndMarsExploreHomeBaseHUDNameFollower>
    {
        private readonly GResPathIndex _m_resIndex;


        public GGUIWndMarsExploreHomeBaseHUDNameFollowerController()
        {
            _m_resIndex = new GResPathIndex(7413);
        }
        public GGUIWndMarsExploreHomeBaseHUDNameFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsExploreHomeBaseHUDNameFollower _createItemWnd(GGUIMonoMarsExploreHomeBaseHUDName _wndMono)
        {
            GGUIWndMarsExploreHomeBaseHUDNameFollower wnd = new GGUIWndMarsExploreHomeBaseHUDNameFollower(_wndMono);
            wnd.showWnd();
            return wnd;
        }
    }

    public class GGUIWndMarsExploreHomeBaseHUDNameFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsExploreHomeBaseHUDName>
    {
        public GGUIWndMarsExploreHomeBaseHUDNameFollower(GGUIMonoMarsExploreHomeBaseHUDName _wnd) : base(_wnd)
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
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreHomeBaseName_playerName, NPPlayer.instance.playerInfo.PlayerName));
        }
    }
}
