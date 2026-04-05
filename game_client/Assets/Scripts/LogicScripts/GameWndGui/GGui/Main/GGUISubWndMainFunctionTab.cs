using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMainFunctionTab : _ANPGGUIBasicSubWnd<GGUIMonoMainFunctionTab>
    {
        private bool _m_isSelect;
        
        
        public GGUISubWndMainFunctionTab(GGUIMonoMainFunctionTab _wnd) 
            : base(_wnd)
        {
            initWnd();
        }


        public event Action<EMainFunctionTabType> onClick;
        public EMainFunctionTabType type { get { if (wnd == null) return EMainFunctionTabType.NONE; return wnd.tabType; } }
        

        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnTabClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnTabClick);
            refreshWnd();
        }


        public void refreshWnd(bool _isSelect)
        {
            _m_isSelect = _isSelect;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.selectShow, _m_isSelect);
            ALUGUICommon.setGameObjEnable(wnd.unselectShow, !_m_isSelect);
        }


        private void _onBtnTabClick(GameObject _)
        {
            if (wnd == null)
                return;
            
            onClick?.Invoke(wnd.tabType);
        }
    }
}