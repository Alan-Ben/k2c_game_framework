using ALPackage;

namespace GOE
{
    /// <summary>
    /// 权益卡属性列表item
    /// </summary>
    public class GGUIWndPrivilegeCardDescContainerItem : _ATALBasicUISubWnd<GGUIMonoPrivilegeCardDescContainerItem>
    {
        public GGUIWndPrivilegeCardDescContainerItem(GGUIMonoPrivilegeCardDescContainerItem _mono) : base(_mono)
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
        public void setInfo(string _title, string _content)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTitle, _title);
            ALUGUICommon.setLabelTxt(wnd.txtContent, _content);
        }
    }
}