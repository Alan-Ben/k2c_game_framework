using ALPackage;

namespace GOE
{
    /// <summary>
    /// 文本列表item
    /// </summary>
    public class GGUIWndCommonTextContainerItem : _ATALBasicUISubWnd<GGUIMonoCommonTextContainerItem>
    {
        public GGUIWndCommonTextContainerItem(GGUIMonoCommonTextContainerItem _mono) : base(_mono)
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
        /// <param name="_content"></param>
        public void setInfo(string _content)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtContent, _content);
        }
    }
}