using ALPackage;

namespace GOE
{
    /// <summary>
    /// 展示滚动文本列表item
    /// </summary>
    public class GGUIWndSubScrollNumContainerItem : _ATALBasicUISubWnd<GGUIMonoSubScrollNumContainerItem>
    {
        public GGUIWndSubScrollNumContainerItem(GGUIMonoSubScrollNumContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
        }


        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(string _info)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtContent, _info);
        }
    }
}
