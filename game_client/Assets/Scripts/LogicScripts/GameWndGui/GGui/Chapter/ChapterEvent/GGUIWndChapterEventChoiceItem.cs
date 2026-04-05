using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterEventChoiceItem : _ANPGGUIBasicSubWnd<GGUIMonoChapterEventChoiceItem>
    {
        private readonly Action<int> _m_onSelect;
        
        private ChapterEventChoiceOptionRefObj _m_optionRef;
        private int _m_index;
        
        
        public GGUIWndChapterEventChoiceItem(GGUIMonoChapterEventChoiceItem _wnd, Action<int> _onSelect) 
            : base(_wnd)
        {
            _m_onSelect = _onSelect;
            
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
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
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onSelectBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onSelectBtnClick);
        }
        

        public void refreshWnd(ChapterEventChoiceOptionRefObj _optionRef, int _index)
        {
            _m_optionRef = _optionRef;
            _m_index = _index;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_optionRef == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_optionRef.option_name));
        }
        
        private void _onSelectBtnClick(GameObject _obj)
        {
            _m_onSelect?.Invoke(_m_index);
        }
    }
}