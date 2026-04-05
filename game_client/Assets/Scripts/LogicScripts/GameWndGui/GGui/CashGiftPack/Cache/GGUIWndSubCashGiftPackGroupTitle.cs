using ALPackage;

namespace GOE
{
    /// <summary>
    /// 礼包组标题
    /// </summary>
    public class GGUIWndSubCashGiftPackGroupTitle : _ANPGGUIBasicSubWnd<GGUIMonoSubCashGiftPackGroupTitle>
    {
        public GGUIWndSubCashGiftPackGroupTitle(GGUIMonoSubCashGiftPackGroupTitle _wnd) : base(_wnd)
        {
            initWnd();
        }
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
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_titleKey"></param>
        public void setInfo(string _titleKey)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtGiftPackName, TextTranslate.instance.getLanguage(_titleKey));
        }
    }
}