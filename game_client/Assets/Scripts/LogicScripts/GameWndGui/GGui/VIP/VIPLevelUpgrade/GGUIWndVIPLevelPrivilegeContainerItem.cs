using ALPackage;

namespace GOE
{
    /// <summary>
    /// VIP等级特权描述列表item
    /// </summary>
    public class GGUIWndVIPLevelPrivilegeContainerItem : _ATALBasicUISubWnd<GGUIMonoVIPLevelPrivilegeContainerItem>
    {
        public GGUIWndVIPLevelPrivilegeContainerItem(GGUIMonoVIPLevelPrivilegeContainerItem  _mono) : base(_mono)
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
        public void setInfo(VIPUpgradePrivilegeShowData _showData)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDesc, _showData.privilegeDesc);
            ALUGUICommon.setLabelTxt(wnd.txtCurValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _showData.curValue));
            ALUGUICommon.setLabelTxt(wnd.txtLastValue, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _showData.lastValue));
        }
    }

    public class VIPUpgradePrivilegeShowData
    {
        public string privilegeDesc;//特权描述
        public long lastValue;//上个值
        public long curValue;//当前值
    }
}