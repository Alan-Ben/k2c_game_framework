using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAnecdoteEventChoiceContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoAnecdoteEventChoiceContainerItem>
    {
        private readonly Action<int> _m_onSelect;
        
        private AnecdoteEventChoiceOptionRefObj _m_optionRef;
        private int _m_index;
        private bool _m_isSelected;

        private NPGGuiWndTexture _m_choiceTex;
        
        
        public GGUISubWndAnecdoteEventChoiceContainerItem(GGUIMonoAnecdoteEventChoiceContainerItem _wnd, Action<int> _onSelect) 
            : base(_wnd)
        {
            _m_onSelect = _onSelect;
            
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_choiceTex?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_choiceTex?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_choiceTex?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_choiceTex?.discard();
            _m_choiceTex = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onSelectBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgChoiceTex != null)
                _m_choiceTex = new NPGGuiWndTexture(wnd.imgChoiceTex);
            
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onSelectBtnClick);
        }
        

        public void refreshWnd(AnecdoteEventChoiceOptionRefObj _optionRef, int _index, bool _isSelected)
        {
            _m_optionRef = _optionRef;
            _m_index = _index;
            _m_isSelected = _isSelected;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_optionRef == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtChoiceName, TextTranslate.instance.getLanguage(_m_optionRef.option_name));
            _m_choiceTex?.setTexture(_m_optionRef.option_tex);
            wnd.setSelect(_m_isSelected);
        }
        
        
        private void _onSelectBtnClick(GameObject _obj)
        {
            _m_onSelect?.Invoke(_m_index);
        }
    }
}