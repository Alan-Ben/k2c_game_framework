using System.Collections.Generic;
using System.Linq;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 账号切换窗口
    /// </summary>
    public class NPGGUIWndAccountSwitch : _ANPGGUIBasicResBarWnd<NPGGUIMonoAccountSwitch>
    {
        private static NPGGUIWndAccountSwitch _g_instance;
        public static NPGGUIWndAccountSwitch instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndAccountSwitch();
                return _g_instance;
            }
        }

        private NPGGUIWndAccountSwitchContainer _m_wSwitchContainer;//切换账号列表
        private long _m_lShowSerialize;//窗口打开序列号

        protected NPGGUIWndAccountSwitch() : base(EALUIWndLayer.ADDITION)
        {
        }

        //获取资源所在资源加载文件名称
        protected override string _monoAssetPath { get { return NPGGUIMonoAccountSwitch.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoAccountSwitch.objName; } }

        //获取用于加载资源的管理对象
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            if (_m_wSwitchContainer != null)
                _m_wSwitchContainer.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wSwitchContainer != null)
                _m_wSwitchContainer.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wSwitchContainer != null)
                _m_wSwitchContainer.discard();
            _m_wSwitchContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSwitchContainer != null)
                _m_wSwitchContainer = new NPGGUIWndAccountSwitchContainer(wnd.monoSwitchContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //设置展示序列号
            long serialize = _m_lShowSerialize;

            //设置加载中显隐对象
            ALUGUICommon.setGameObjEnable(wnd.goLoadingShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.goLoadingHideList, false);

            //获取cdn配置的绑定渠道列表
            CDNSetting_ClientPlatformConfig.instance.requestData(_data =>
            {
                if (_m_lShowSerialize != serialize || _data == null || wnd == null || !isShow)
                    return;

                //设置加载成功显隐对象
                ALUGUICommon.setGameObjEnable(wnd.goLoadingShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goLoadingHideList, true);

                //设置可切换渠道列表
                List<NPLoginWayRefObj> loginWayRefList = new List<NPLoginWayRefObj>();
                //先添加游客登陆
                NPLoginWayRefObj guestRef = GRefdataCoreMgr.instance.loginWayList.getRef((long) ENPLoginWayType.GUEST);
                if(guestRef != null)
                    loginWayRefList.Add(guestRef);
                GRefdataCoreMgr.instance.loginWayList.dealAllRef(_loginWayRef =>
                {
                    if (_loginWayRef != null && _loginWayRef.login_type != ENPLoginWayType.GUEST && _data.login != null && _data.login.Contains(_loginWayRef.login_type.ToString().ToLowerInvariant()))
                    {
                        if(_loginWayRef.login_type == ENPLoginWayType.IOS && !SDKMgr.instance.checkCanUseAppleLogin())
                            return;

                        loginWayRefList.Add(_loginWayRef);
                    }
                });

                //设置切换账号列表
                if (_m_wSwitchContainer != null)
                {
                    _m_wSwitchContainer.showWnd();
                    _m_wSwitchContainer.setInfo(loginWayRefList);
                }
            });
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.DoUIRollBackByEscByTag(UINodeTagConst.C_ACCOUNT_SWITCH);
        }
    }
}
