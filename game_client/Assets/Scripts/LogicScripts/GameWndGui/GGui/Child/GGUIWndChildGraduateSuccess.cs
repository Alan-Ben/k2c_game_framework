using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildGraduateSuccess : _ANPGGUIBasicWnd<GGUIMonoChildGraduateSuccess>
    {
        [NotNull] public static GGUIWndChildGraduateSuccess instance { get { return _g_instance ??= new GGUIWndChildGraduateSuccess(); } }
        private static GGUIWndChildGraduateSuccess _g_instance;


        private GGUISubWndChildInfo _m_childInfoWnd;
        private NPGGUIWndCommonToggleEx _m_notTodayToggle;
        private NPGGuiWndTexture _m_bgTexture;
        
        private _IChildInfo _m_childInfo;
        
        
        public GGUIWndChildGraduateSuccess() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        public bool dontShowAgainToday { get { return _m_notTodayToggle is { isOn: true }; } }
        protected override string _monoAssetPath { get { return GGUIMonoChildGraduateSuccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildGraduateSuccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }



        protected override void _onShowWnd()
        {
            _m_childInfoWnd?.showWnd();
            _m_notTodayToggle?.showWnd();
            _m_bgTexture?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childInfoWnd?.hideWnd();
            _m_notTodayToggle?.hideWnd();
            _m_bgTexture?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
            _m_notTodayToggle?.resetWnd();
            _m_bgTexture?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_childInfoWnd?.discard();
            _m_notTodayToggle?.discard();
            _m_bgTexture?.discard();
            _m_childInfoWnd = null;
            _m_notTodayToggle = null;
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

            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            if (wnd.monoDontShowToday != null)
            {
                _m_notTodayToggle = new NPGGUIWndCommonToggleEx(wnd.monoDontShowToday);
                _m_notTodayToggle.clickDelegate = _onNotTodayToggleClick;
            }
            if (wnd.imgGraduatingBg != null)
                _m_bgTexture = new NPGGuiWndTexture(wnd.imgGraduatingBg);
            
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnConfirmClick);
        }


        public void refreshWnd(_IChildInfo _childInfo)
        {
            _m_childInfo = _childInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_childInfo == null)
                return;

            _m_childInfoWnd?.refreshWnd(_m_childInfo);
            ALUGUICommon.setLabelTxt(wnd.txtGraduatingDesc, TextTranslate.instance.getLanguage(TransKeyConst.child_graduationDesc_name, _m_childInfo.name));
            _m_bgTexture?.setTexture(_m_childInfo.attrRef.getLastBgRes());
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