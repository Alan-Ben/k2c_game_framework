
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildStepUpSuccess : _ANPGGUIBasicWnd<GGUIMonoChildStepUpSuccess>
    {
        [NotNull] public static GGUIWndChildStepUpSuccess instance { get { return _g_instance ??= new GGUIWndChildStepUpSuccess(); } }
        private static GGUIWndChildStepUpSuccess _g_instance;


        private GGUISubWndChildInfo _m_childInfoWnd;
        private NPGGUIWndCommonToggleEx _m_notTodayToggle;
        private NPGGuiWndTexture _m_bgTexture;
        
        private ChildInfo _m_childInfo;


        public GGUIWndChildStepUpSuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        public bool dontShowAgainToday { get { return _m_notTodayToggle is { isOn: true }; } }
        protected override string _monoAssetPath { get { return GGUIMonoChildStepUpSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildStepUpSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_notTodayToggle?.showWnd();
            _m_childInfoWnd?.showWnd();
            _m_bgTexture?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_notTodayToggle?.hideWnd();
            _m_childInfoWnd?.hideWnd();
            _m_bgTexture?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_notTodayToggle?.resetWnd();
            _m_childInfoWnd?.resetWnd();
            _m_bgTexture?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_notTodayToggle?.discard();
            _m_childInfoWnd?.discard();
            _m_bgTexture?.discard();
            _m_notTodayToggle = null;
            _m_childInfoWnd = null;
            _m_bgTexture = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnConfirmClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoDontShowToday != null)
            {
                _m_notTodayToggle = new NPGGUIWndCommonToggleEx(wnd.monoDontShowToday);
                _m_notTodayToggle.clickDelegate = _onNotTodayToggleClick;
            }
            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            if (wnd.imgStepBg != null)
                _m_bgTexture = new NPGGuiWndTexture(wnd.imgStepBg);
            
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnConfirmClick);
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
            int step = _m_childInfo.step;
            wnd.setStep(step);
            ALUGUICommon.setLabelTxt(wnd.txtStepUpDesc, TextTranslate.instance.getLanguage(string.Format(TransKeyConst.child_stepUpDesc_name, step), _m_childInfo.name));
            _m_bgTexture?.setTexture(_m_childInfo.attrRef.getBgResByStep(step));
        }
        
        
        private void _onBtnConfirmClick(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        private void _onNotTodayToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            _toggle?.setSelected(!_toggle.isOn);
        }
    }
}