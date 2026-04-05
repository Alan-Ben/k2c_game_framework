using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 账号绑定窗口
    /// </summary>
    public class NPGGUIWndAccountBind : _ANPGGUIBasicResBarWnd<NPGGUIMonoAccountBind>
    {
        private static NPGGUIWndAccountBind _g_instance;
        public static NPGGUIWndAccountBind instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndAccountBind();
                return _g_instance;
            }
        }

        private NPGGUIWndAccountBindContainer _m_wBindContainer;//绑定方式列表
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;//玩家头像
        private NPGGuiWndTexture _m_wTexLoginType;//当前登录方式
        private long _m_lShowSerialize;//窗口打开序列号

        protected NPGGUIWndAccountBind() : base(EALUIWndLayer.ADDITION)
        {
        }

        //获取资源所在资源加载文件名称
        protected override string _monoAssetPath { get { return NPGGUIMonoAccountBind.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoAccountBind.objName; } }

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

            if (_m_wBindContainer != null)
                _m_wBindContainer.hideWnd();

            if (_m_wPlayerIcon != null)
                _m_wPlayerIcon.hideWnd();

            if(_m_wTexLoginType != null)
                _m_wTexLoginType.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wBindContainer != null)
                _m_wBindContainer.resetWnd();

            if (_m_wPlayerIcon != null)
                _m_wPlayerIcon.resetWnd();

            if(_m_wTexLoginType != null)
                _m_wTexLoginType.discardTexture();
        }
        protected override void _onDiscard()
        {
            if (_m_wBindContainer != null)
                _m_wBindContainer.discard();
            _m_wBindContainer = null;

            if (_m_wPlayerIcon != null)
                _m_wPlayerIcon.discard();
            _m_wPlayerIcon = null;

            if(_m_wTexLoginType != null)
                _m_wTexLoginType.discard();
            _m_wTexLoginType = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.uncombineBtnClick(wnd.btnSwitchAccount, _onClickSwitch);//点击切换账号
            ALUGUICommon.uncombineBtnClick(wnd.btnCopy, _onClickCopyBtn);//点击复制cid
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoBindContainer != null)
                _m_wBindContainer = new NPGGUIWndAccountBindContainer(wnd.monoBindContainer);

            if (wnd.monoPlayerIcon != null)
            {
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
                _m_wPlayerIcon.onClickAction += _onClickPlayerIcon;
            }

            if (wnd.imgLoginType != null)
                _m_wTexLoginType = new NPGGuiWndTexture(wnd.imgLoginType);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.combineBtnClick(wnd.btnSwitchAccount, _onClickSwitch);//点击打开切换账号窗口
            ALUGUICommon.combineBtnClick(wnd.btnCopy, _onClickCopyBtn);//点击复制cid
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshPlayerInfo();
            _refreshBindList();
        }

        //刷新玩家头像
        private void _refreshPlayerInfo()
        {
            if (wnd == null)
                return;

            //设置玩家头像
            if (_m_wPlayerIcon != null)
            {
                _m_wPlayerIcon.showWnd();
                _m_wPlayerIcon.setSelfInfo();
            }

            //设置当前登录方式图标
            ENPLoginWayType loginWay = SDKLoginSetting.instance.getSDKLoginType();
            NPLoginWayRefObj loginWayRef = GRefdataCoreMgr.instance.loginWayList.getRef((int) loginWay);
            if (_m_wTexLoginType != null && loginWayRef != null)
            {
                _m_wTexLoginType.showWnd();
                _m_wTexLoginType.setTexture(loginWayRef.icon_index);
            }

            //设置当前所在服务器名
            ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, ""));//默认展示空
            GameCDNServerListMgr.instance.getCurServerInfo(serverInfo =>
            {
                if(serverInfo != null)
                    ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, serverInfo.serverName));//服务器：{0}
            });
        }

        //刷新绑定列表
        private void _refreshBindList()
        {
            if (wnd == null)
                return;

            //设置加载中显隐对象
            ALUGUICommon.setGameObjEnable(wnd.goLoadingShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.goLoadingHideList, false);

            //检查SDK登录状态，获取CDN后台绑定列表
            _checkLoginAndGetData((_data) =>
            {
                if (_data == null)
                {
                    Debug.LogError($"NPGGUIWndAccountBind 未获取到可绑定列表数据");
                    return;
                }

                if (wnd == null)
                    return;

                //设置加载成功显隐对象
                ALUGUICommon.setGameObjEnable(wnd.goLoadingShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goLoadingHideList, true);

                //设置可绑定渠道列表
                List<NPLoginWayRefObj> loginWayRefList = new List<NPLoginWayRefObj>();
                GRefdataCoreMgr.instance.loginWayList.dealAllRef(_loginWayRef =>
                {
                    if (_loginWayRef != null && _data.login != null && _data.login.Contains(_loginWayRef.login_type.ToString().ToLowerInvariant()))
                    {
                        //绑定方式不需要游客
                        if(_loginWayRef.login_type != ENPLoginWayType.GUEST)
                            loginWayRefList.Add(_loginWayRef);
                    }
                });

                //设置绑定列表
                if (_m_wBindContainer != null)
                {
                    _m_wBindContainer.showWnd();
                    _m_wBindContainer.setInfo(loginWayRefList);
                }

            });
        }

        /// <summary>
        /// 检查SDK登录状态，获取CDN后台绑定列表
        /// </summary>
        /// <param name="_onComplete"></param>
        /// <param name="_retryCount">重试次数</param>
        private void _checkLoginAndGetData(Action<ClientPlatformConfig> _onComplete, long _retryCount = 3)
        {
            if (_retryCount == 0)
                return;

            //设置展示序列号
            long serialize = _m_lShowSerialize;

            if (SDKMgr.instance.isUseSDK)
            {
                //检查SDK登录，没有登录的话登录一次获取已绑定列表
                SDKMgr.instance.quickLogin(_tokenData =>
                {
                    if (_m_lShowSerialize != serialize || wnd == null || !isShow)
                        return;

                    //获取cdn配置的绑定渠道列表
                    CDNSetting_ClientPlatformConfig.instance.requestData(_data =>
                    {
                        if (_m_lShowSerialize != serialize || wnd == null || !isShow)
                            return;

                        if (_onComplete != null)
                            _onComplete(_data);
                    });

                }, (_code, _msg) =>
                {
                    Debug.LogError($"NPGGUIWndAccountBind SDK登录失败，code:{_code},msg:{_msg}");
                    _checkLoginAndGetData(_onComplete, _retryCount - 1);
                });
            }
            else
            {
                //获取cdn配置的绑定渠道列表
                CDNSetting_ClientPlatformConfig.instance.requestData(_data =>
                {
                    if (_m_lShowSerialize != serialize || wnd == null || !isShow)
                        return;

                    if (_onComplete != null)
                        _onComplete(_data);
                });
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.DoUIRollBackByEscByTag(UINodeTagConst.C_ACCOUNT_BIND);
        }

        //点击打开切换账号窗口
        private void _onClickSwitch(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndAccountSwitch.instance,
                NPGGUIWndAccountSwitch.instance.showWnd,
                EUIQueueStageType.MAIN,
                UINodeTagConst.C_ACCOUNT_SWITCH,
                false,
                true);
        }

        //点击玩家头像
        private void _onClickPlayerIcon()
        {
            NPCommonSimplePlayerInfo tempInfo = new NPCommonSimplePlayerInfo(NPPlayer.instance.playerInfo, NPPlayer.instance.titleComp.curTitle, true,GCommon.getItemCount(ENPItemType.CURRENCY,(long)ECurrency.VIP_EXP));
            GCommon.showPlayerInfoWnd(tempInfo);
        }

        //点击复制cid
        private void _onClickCopyBtn(GameObject _go)
        {
            GUIUtility.systemCopyBuffer = NPPlayer.instance.playerInfo.CID.ToString();
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_copySuc_str));
        }
    }
}
