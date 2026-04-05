using ALPackage;

namespace GOE
{
    /// <summary>
    /// 本地推送设置列表item
    /// </summary>
    public class GGUIWndGameSettingLocalPushContainerItem : _ATALBasicUISubWnd<GGUIMonoGameSettingLocalPushContainerItem>
    {
        //配置数据
        private LocalPushRefObj _m_localPushRef;
        //开关
        private NPGGUIWndCommonTab _m_wSwitch;

        public GGUIWndGameSettingLocalPushContainerItem(GGUIMonoGameSettingLocalPushContainerItem _mono) : base(_mono)
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
            _m_wSwitch?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSwitch?.discard();
            _m_wSwitch = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoMainSwitch != null)
            {
                _m_wSwitch = new NPGGUIWndCommonTab(wnd.monoMainSwitch);
                _m_wSwitch.clickDelegate += _onClickSwitch;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        public void setInfo(LocalPushRefObj _refObj)
        {
            if (_refObj == null)
                return;

            _m_localPushRef = _refObj;
            refreshWnd();
        }

        //刷新窗口
        public void refreshWnd()
        {
            if(wnd == null || _m_localPushRef == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_localPushRef.switch_show_name));
            _m_wSwitch?.setSelected(GameSetting.instance.getLocalPushSwitchIsOpen(_m_localPushRef.type));
        }

        //点击开关
        private void _onClickSwitch(bool _isOn)
        {
            if (_m_localPushRef == null)
                return;

            //如果总开关没打开，这里不能打开
            if (!GameSetting.instance.getlocalPushMainSwitchIsOpen())
            {
                _m_wSwitch?.setSelected(false);
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.setting_canNotOpenLocalPushTip_none);//开启该通知需要先开启所有信息推送功能
                return;
            }

            _m_wSwitch?.setSelected(_isOn);
            GameSetting.instance.setLocalPushSwitch(_m_localPushRef.type, _isOn);

            if (_isOn)
            {
                //您已开启{0}通知
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.setting_openLocalPushItem_name,TextTranslate.instance.getLanguage(_m_localPushRef.switch_show_name)));
            }
            else
            {
                //您已关闭{0}通知
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.setting_closeLocalPushItem_name, TextTranslate.instance.getLanguage(_m_localPushRef.switch_show_name)));
            }
        }
    }
}
