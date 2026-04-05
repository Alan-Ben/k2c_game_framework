using ALPackage;

namespace GOE
{
    /// <summary>
    /// 选项事件格 - 选项
    /// </summary>
    public class GGUIWndCommonChoiceEventOptionItem : _AGGUIWndOptionItem<GGUIMonoCommonChoiceEventOptionItem, GGUIWndCommonChoiceEventOptionItem>
    {
        private NPGGuiWndTexture _m_wOptionIcon;//选项图片
        
        private CommonEventChoiceOptionRefObj _m_rChoiceOptionRefObj;
        public CommonEventChoiceOptionRefObj choiceOptionRefObj { get { return _m_rChoiceOptionRefObj; } }

        public GGUIWndCommonChoiceEventOptionItem(GGUIMonoCommonChoiceEventOptionItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _m_wOptionIcon?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_wOptionIcon?.discardTexture();
        }

        protected override void _onDiscardEx()
        {
            _m_wOptionIcon?.discard();
            _m_wOptionIcon = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if(wnd == null)
                return;

            if (wnd.texIcon != null)
                _m_wOptionIcon = new NPGGuiWndTexture(wnd.texIcon);
        }

        public void setData(CommonEventChoiceOptionRefObj _choiceOptionRefObj)
        {
            if (_choiceOptionRefObj == null)
                return;

            _m_rChoiceOptionRefObj = _choiceOptionRefObj;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || _m_rChoiceOptionRefObj == null)
                return;

            if (_m_wOptionIcon != null)
            {
                _m_wOptionIcon.showWnd();
                _m_wOptionIcon.setTexture(_m_rChoiceOptionRefObj.option_icon);
            }
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_rChoiceOptionRefObj.option_desc, _m_rChoiceOptionRefObj.option_desc_args));
        }
    }
}