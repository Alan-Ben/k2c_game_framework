using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreTeamSelect : _ANPGGUIBasicWnd<GGUIMonoMarsExploreTeamSelect>
    {
        private static GGUIWndMarsExploreTeamSelect _g_instance;
        [NotNull] public static GGUIWndMarsExploreTeamSelect instance { get { return _g_instance ??= new GGUIWndMarsExploreTeamSelect(); } }
        

        private GGUISubWndMarsExploreTeamSelectContainer _m_subWndSelectContainer;
        private _IMarsExploreTeamSelectDealer _m_target;

        private new bool _m_bIsShow;


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreTeamSelect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreTeamSelect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        public GGUIWndMarsExploreTeamSelect()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_bIsShow = true;
            
            _m_subWndSelectContainer?.showWnd();
            
            refreshWnd();

            _tryChangeEventWnd();
        }
        protected override void _onHideWnd()
        {
            _tryResetEventWnd();
            
            _m_subWndSelectContainer?.hideWnd();
            
            _m_bIsShow = false;
        }
        protected override void _onReset()
        {
            _m_subWndSelectContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);

            _m_subWndSelectContainer?.discard();
            _m_subWndSelectContainer = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);

            if (wnd.monoSelectContainer != null)
                _m_subWndSelectContainer = new GGUISubWndMarsExploreTeamSelectContainer(wnd.monoSelectContainer);
        }


        public void refreshWnd(_IMarsExploreTeamSelectDealer _target)
        {
            _tryResetEventWnd();
            _m_target = _target;
            _tryChangeEventWnd();
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_target == null)
                return;

            _m_subWndSelectContainer?.refreshWnd(_m_target);
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_SELECT);
            _m_target?.closeWnd();
        }
        private void _tryChangeEventWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_target?.onTeamSelectWndShow();
        }
        private void _tryResetEventWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_target?.onTeamSelectWndHide();
        }
    }
}
