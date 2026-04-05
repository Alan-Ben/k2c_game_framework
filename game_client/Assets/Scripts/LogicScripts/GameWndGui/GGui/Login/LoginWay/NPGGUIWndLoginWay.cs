using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 登录方式选择窗口对象
    /// </summary>
    public class NPGGUIWndLoginWay : _ANPGGUIBasicWnd<NPGGUIMonoLoginWay>
    {
        private static NPGGUIWndLoginWay _g_instance = new NPGGUIWndLoginWay();
        public static NPGGUIWndLoginWay instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndLoginWay();
                return _g_instance;
            }
        }

        //登录方式容器
        private NPGGUIWndLoginWayContainer _m_wcWayContainer;
        //关闭回调
        private Action _m_aOnClose;
        //是否需要检查登录类型
        private bool _m_bNeedCheckLoginType;
        //是否正在登录
        private bool _m_bIsLoggingIn;

        protected NPGGUIWndLoginWay()
           : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
        * 获取资源所在资源加载文件名称
        **/
        protected override string _monoAssetPath { get { return NPGGUIMonoLoginWay.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoLoginWay.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            //默认隐藏登录中GO
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goLoginShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goLoginHideList, true);
            }
            clearAllItem();
            //此时直接显示所有登录方式
            showAllWay();
            _m_bIsLoggingIn = false;
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            if (_m_aOnClose != null)
                _m_aOnClose();
            _m_aOnClose = null;
        }

        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            if(null != _m_wcWayContainer)
                _m_wcWayContainer.clearAll();
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_aOnClose = null;

            if (null != _m_wcWayContainer)
                _m_wcWayContainer.discard();
            _m_wcWayContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        /*************
        * 窗口初始化完成调用的函数
        * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (null != wnd.loginWayContainer)
            {
                _m_wcWayContainer = new NPGGUIWndLoginWayContainer(wnd.loginWayContainer);
                _m_wcWayContainer.onClickItem += onClickLoginWay;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onClose">关闭回调</param>
        /// <param name="_needCheckLoginType">是否需要检查已登录类型</param>
        public void setInfo(Action _onClose,bool _needCheckLoginType)
        {
            _m_aOnClose = _onClose;
            _m_bNeedCheckLoginType = _needCheckLoginType;
        }

        /// <summary>
        /// 显示所有登录方式
        /// </summary>
        public void clearAllItem()
        {
            if(null != _m_wcWayContainer)
                _m_wcWayContainer.clearAll();
        }

        /// <summary>
        /// 添加一个显示方式
        /// </summary>
        public void showAllWay()
        {
            //刷新所有登录方式
            if(null != _m_wcWayContainer)
                _m_wcWayContainer.showAllWay(_m_bNeedCheckLoginType);
        }

        /// <summary>
        /// 点击了某个登录方式的处理
        /// </summary>
        /// <param name="_loginWay"></param>
        public void onClickLoginWay(NPLoginWayRefObj _loginWay)
        {
            if (null == _loginWay || wnd == null)
                return;

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"使用登录方式： {_loginWay.login_type}");
            }

            if (SDKMgr.instance.isUseSDK)
            {
                if (_loginWay.login_type == SDKLoginSetting.instance.getSDKLoginType() && _m_bNeedCheckLoginType)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.account_isCurLoginType_none);//已是当前登录方式
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Login_AllWay);
                    return;
                }

                _m_bIsLoggingIn = true;
                int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                //展示正在登录中GO
                ALUGUICommon.setGameObjEnable(wnd.goLoginShowList,true);
                ALUGUICommon.setGameObjEnable(wnd.goLoginHideList,false);
                //先登出操作
                SDKMgr.instance.logout();
                //开始登录指定类型
                SDKMgr.instance.login(_loginWay.login_type, _tokenData =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                    _m_bIsLoggingIn = false;
                    _m_aOnClose = null;
                    //隐藏正在登录中GO
                    ALUGUICommon.setGameObjEnable(wnd.goLoginShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goLoginHideList, true);
                    //退出窗口
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Login_AllWay);
                    //sdk登录成功，游戏开始重登
                    Game.instance.relogin();
                }, (_code, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                    _m_bIsLoggingIn = false;
                    //隐藏正在登录中GO
                    ALUGUICommon.setGameObjEnable(wnd.goLoginShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goLoginHideList, true);
                    //上浮提示
                    //切换账号失败（{0}）
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.account_switchFailedCode_num, _code));
                });
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            //如果正在登录中不能关闭
            if (_m_bIsLoggingIn)
                return;

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Login_AllWay);
        }
    }
}
