using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnLevelDetail : _ATALBasicUIWnd<GGUIMonoInnLevelDetail>
    {
        [NotNull] public static GGUIWndInnLevelDetail instance { get { return _g_instance ??= new GGUIWndInnLevelDetail(); } }
        private static GGUIWndInnLevelDetail _g_instance;


        private GGUISubWndInnLevelDetailGrid _m_levelGridWnd;


        public GGUIWndInnLevelDetail()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnLevelDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnLevelDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_levelGridWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_levelGridWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_levelGridWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_levelGridWnd?.discard();
            _m_levelGridWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoLevelGrid != null)
                _m_levelGridWnd = new GGUISubWndInnLevelDetailGrid(wnd.monoLevelGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_levelGridWnd?.refreshWnd();
        }


        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_LEVEL_DETAIL);
        }
    }
}