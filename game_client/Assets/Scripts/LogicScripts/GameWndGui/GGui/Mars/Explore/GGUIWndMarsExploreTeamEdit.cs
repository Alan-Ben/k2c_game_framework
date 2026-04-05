using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreTeamEdit : _ANPGGUIBasicWnd<GGUIMonoMarsExploreTeamEdit>
    {
        private static GGUIWndMarsExploreTeamEdit _g_instance;
        [NotNull] public static GGUIWndMarsExploreTeamEdit instance { get { return _g_instance ??= new GGUIWndMarsExploreTeamEdit(); } }
        

        private GGUISubWndMarsExploreTeamEditItemContainer _m_subWndItemContainer;
        

        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreTeamEdit.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreTeamEdit.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        public GGUIWndMarsExploreTeamEdit() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_subWndItemContainer?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_subWndItemContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_subWndItemContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnCloseAdditional, _onClickClose);

            _m_subWndItemContainer?.discard();
            _m_subWndItemContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnCloseAdditional, _onClickClose);

            if (wnd.monoItemContainer != null)
                _m_subWndItemContainer = new GGUISubWndMarsExploreTeamEditItemContainer(wnd.monoItemContainer);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_subWndItemContainer?.refreshWnd();
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_EDIT);
        }
    }
}
