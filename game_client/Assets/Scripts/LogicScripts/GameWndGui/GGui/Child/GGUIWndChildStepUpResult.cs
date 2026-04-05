
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildStepUpResult : _ANPGGUIBasicWnd<GGUIMonoChildStepUpResult>
    {
        [NotNull] public static GGUIWndChildStepUpResult instance { get { return _g_instance ??= new GGUIWndChildStepUpResult(); } }
        private static GGUIWndChildStepUpResult _g_instance;


        private GGUISubWndChildInfo _m_childInfoWnd;
        private ChildInfo _m_childInfo;
        
        
        public GGUIWndChildStepUpResult() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildStepUpResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildStepUpResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_childInfoWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childInfoWnd?.discard();
            _m_childInfoWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }


        public void refreshWnd(ChildInfo _childInfo)
        {
            _m_childInfo = _childInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_childInfo == null)
                return;
            
            _m_childInfoWnd?.refreshWnd(_m_childInfo);
            wnd.refreshRandomShow();
            wnd.setStep(_m_childInfo.step);
        }
        
        
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}