using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildGraduationResult : _ANPGGUIBasicWnd<GGUIMonoChildGraduationResult>
    {
        [NotNull] public static GGUIWndChildGraduationResult instance { get { return _g_instance ??= new GGUIWndChildGraduationResult(); } }
        private static GGUIWndChildGraduationResult _g_instance;


        private GGUISubWndChildInfo _m_childInfoWnd;
        private NPGGUIWndCommonItem _m_presentItemWnd;

        private _IChildInfo _m_childInfo;
        private _IItem _m_presentItem;
        
        
        public GGUIWndChildGraduationResult() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildGraduationResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildGraduationResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_childInfoWnd?.showWnd();
            _m_presentItemWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childInfoWnd?.hideWnd();
            _m_presentItemWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
            _m_presentItemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childInfoWnd?.discard();
            _m_presentItemWnd?.discard();
            _m_childInfoWnd = null;
            _m_presentItemWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onBtnGotoClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            if (wnd.monoPresentItem != null)
                _m_presentItemWnd = new NPGGUIWndCommonItem(wnd.monoPresentItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onBtnGotoClick);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }


        public void refreshWnd(_IChildInfo _childInfo, _IItem _presentItem)
        {
            _m_childInfo = _childInfo;
            _m_presentItem = _presentItem;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_childInfo == null)
                return;
            
            _m_childInfoWnd?.refreshWnd(_m_childInfo);
            _m_presentItemWnd?.showWnd(_m_presentItem);
        }
        
        
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onBtnGotoClick(GameObject _)
        {
            _onBtnCloseClick(null);
            QueueMgr.instance.AddNode(new GNodeAdultMain());
        }
        private void _onBtnDetailClick(GameObject _)
        {
            GGUIWndChildGraduatedDetail.instance.refreshWnd(_m_childInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildGraduatedDetail.instance, GGUIWndChildGraduatedDetail.instance.showWnd, EUIQueueStageType.MAIN, string.Empty, true, false);
        }
    }
}