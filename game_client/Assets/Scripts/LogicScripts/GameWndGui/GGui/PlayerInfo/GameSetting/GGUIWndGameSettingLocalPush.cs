using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 本地推送设置窗口
    /// </summary>
    public class GGUIWndGameSettingLocalPush : _ATALBasicUIWnd<GGUIMonoGameSettingLocalPush>
    {
        private static GGUIWndGameSettingLocalPush _g_instance;
        public static GGUIWndGameSettingLocalPush instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndGameSettingLocalPush();
                return _g_instance;
            }
        }

        //总开关
        private NPGGUIWndCommonTab _m_wMainSwitch;
        //开关列表
        private GGUIWndGameSettingLocalPushContainer _m_wSwitchContainer;

        protected GGUIWndGameSettingLocalPush() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoGameSettingLocalPush.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGameSettingLocalPush.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        


        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wSwitchContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSwitchContainer?.resetWnd();
            _m_wMainSwitch?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSwitchContainer?.discard();
            _m_wSwitchContainer = null;
            _m_wMainSwitch?.discard();
            _m_wMainSwitch = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGameSettingLocalPushContainer != null)
                _m_wSwitchContainer = new GGUIWndGameSettingLocalPushContainer(wnd.monoGameSettingLocalPushContainer);

            if (wnd.monoMainSwitch != null)
            {
                _m_wMainSwitch = new NPGGUIWndCommonTab(wnd.monoMainSwitch);
                _m_wMainSwitch.clickDelegate += _onClickMainSwitch;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (_m_wSwitchContainer != null)
            {
                _m_wSwitchContainer.showWnd();
                _m_wSwitchContainer.showItemList();
            }

            _m_wMainSwitch?.setSelected(GameSetting.instance.getlocalPushMainSwitchIsOpen());
        }

        //点击总开关
        private void _onClickMainSwitch(bool _isOn)
        {
            _m_wMainSwitch?.setSelected(_isOn);
            GameSetting.instance.setlocalPushMainSwitch(_isOn);
            _m_wSwitchContainer?.refreshAllState();

            if (_isOn)
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.setting_openMainLocalPush_none);//您已开启信息推送功能
            else
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.setting_closeMainLocalPush_none);//您已关闭信息推送功能
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SETTING_LOCAL_PUSH);
        }
    }
}
