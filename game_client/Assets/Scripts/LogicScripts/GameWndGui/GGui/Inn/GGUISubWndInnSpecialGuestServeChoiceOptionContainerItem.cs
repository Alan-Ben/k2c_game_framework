using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnSpecialGuestServeChoiceOptionContainerItem : _ATALBasicUISubWnd<GGUIMonoInnSpecialGuestServeChoiceOptionContainerItem>
    {
        private readonly Action<long> _m_onOptionClick;
        
        private InnSpecialGuestChoiceOptionRefObj _m_optionRefObj;
        private NPGGuiWndTexture _m_iconWnd;
        private long _m_optionId = -1;
        private bool _m_select;


        public GGUISubWndInnSpecialGuestServeChoiceOptionContainerItem(GGUIMonoInnSpecialGuestServeChoiceOptionContainerItem _wnd, Action<long> _onOptionClick) : base(_wnd)
        {
            _m_onOptionClick = _onOptionClick;
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }


        public void refreshWnd([NotNull] InnSpecialGuestChoiceOptionRefObj _optionRefObj, bool _selected)
        {
            _m_optionRefObj = _optionRefObj;
            _m_optionId = _optionRefObj.id;
            _m_select = _selected;
            
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_optionRefObj == null)
                return;

            _m_iconWnd?.setTexture(_m_optionRefObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_optionRefObj.option_desc));
            wnd.setSelected(_m_select);
        }


        private void _onBtnSelectClick(GameObject _obj)
        {
            if (_m_onOptionClick == null || _m_optionId < 0)
                return;

            _m_onOptionClick(_m_optionId);
        }
    }
}