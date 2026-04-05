using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟等级预览信息列表Item
    /// </summary>
    public class GGUIWndGuildLevelPreviewContainerItem : _ATALBasicUISubWnd<GGUIMonoGuildLevelPreviewContainerItem>
    {
        public GGUIWndGuildLevelPreviewContainerItem(GGUIMonoGuildLevelPreviewContainerItem _wnd)
            : base(_wnd)
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
        /// <param name="_desc"></param>
        /// <param name="_value"></param>
        public void setInfo(string _desc, string _value)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_desc));
            ALUGUICommon.setLabelTxt(wnd.txtValue, TextTranslate.instance.getLanguage(_value));
        }
    }
}
