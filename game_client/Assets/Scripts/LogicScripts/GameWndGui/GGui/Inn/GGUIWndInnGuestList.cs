
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnGuestList : _ATALBasicUIWnd<GGUIMonoInnGuestList>
    {
        [NotNull] public static GGUIWndInnGuestList instance { get { return _g_instance ??= new GGUIWndInnGuestList(); } }
        private static GGUIWndInnGuestList _g_instance;


        private GGUISubWndInnGuestListPageTab _m_pageTab;
        

        public GGUIWndInnGuestList()
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnGuestList.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnGuestList.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }


        protected override void _onShowWnd()
        {
            _m_pageTab?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_pageTab?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_pageTab?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_pageTab?.discard();
            _m_pageTab = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.pageTab != null)
                _m_pageTab = new GGUISubWndInnGuestListPageTab(wnd.pageTab);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
        }
        
        
        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_GUEST_LIST);
        }
    }
}