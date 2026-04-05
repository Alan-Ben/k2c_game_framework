using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildMultiGraduationResult : _ANPGGUIBasicWnd<GGUIMonoChildMultiGraduationResult>
    {
        [NotNull] public static GGUIWndChildMultiGraduationResult instance { get { return _g_instance ??= new GGUIWndChildMultiGraduationResult(); } }
        private static GGUIWndChildMultiGraduationResult _g_instance;


        private GGUISubWndChildGraduationContainer _m_childContainer;
        
        private List<_IChildInfo> _m_childList;
        private List<_IItem> _m_presentList;
        

        public GGUIWndChildMultiGraduationResult() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildMultiGraduationResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildMultiGraduationResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_childContainer?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childContainer?.discard();
            _m_childContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onBtnGotoClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoChildContainer != null)
                _m_childContainer = new GGUISubWndChildGraduationContainer(wnd.monoChildContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onBtnGotoClick);
        }


        public void refreshWnd(List<_IChildInfo> _childList, List<_IItem> _presentList)
        {
            _m_childList = _childList;
            _m_presentList = _presentList;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (_m_childList == null)
                return;
            
            _m_childContainer?.refreshWnd(_m_childList, _m_presentList);
            ALUGUICommon.setLabelTxt(wnd.txtChildNum, TextTranslate.instance.getLanguage(TransKeyConst.child_multiGraduationChildNum_num, _m_childList.Count));
            long earnings = 0;
            foreach (_IChildInfo child in _m_childList)
                earnings += child.earnings;
            ALUGUICommon.setLabelTxt(wnd.txtTotalEarnings, TextTranslate.instance.getLanguage(TransKeyConst.child_multiGraduationTotalEarnings_num, earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
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
    }
}