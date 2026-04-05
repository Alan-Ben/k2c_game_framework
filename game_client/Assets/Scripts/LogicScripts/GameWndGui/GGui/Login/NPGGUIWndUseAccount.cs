using UnityEngine;
using UnityEngine.UI;
using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// editor下登入ls用的数据对下
    /// </summary>
    public class NPGCheatLSLoginServerInfo : _ILSLoginServerInfo
    {
        private string _m_defaultIP;
        private int _m_defaultPort;

        public NPGCheatLSLoginServerInfo(string _defaultIP, int _defaultPort)
        {
            _m_defaultIP = _defaultIP;
            _m_defaultPort = _defaultPort;
        }
        
        public string defaultIP
        {
            get { return _m_defaultIP; }
        }

        public int defaultPort
        {
            get { return _m_defaultPort; }
        }

        public float defaultDelayLoginTime
        {
            get { return 0; }
        }

        public List<string> loopIPList { get { return null; } }

        public List<int> loopPortList { get { return null; } }
        public List<float> loopDelayLoginTimeList { get { return null; } }
        
        //异步初始化获取数据
        public void reqDataDone(Action _action)
        {
            if (null != _action)
                _action();
        }
    }
    
    
    /*******************
     *账号密码登录窗口对象
     **/
    public class NPGGUIWndUseAccount : _ANPGGUIBasicWnd<NPGGUIMonoUseAccount>
    {
        private static NPGGUIWndUseAccount _g_instance = new NPGGUIWndUseAccount();
        public static NPGGUIWndUseAccount instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndUseAccount();
                return _g_instance;
            }
        }

        //点击进入的回调窗口
        private Action _m_dEnterDelegate;

        protected NPGGUIWndUseAccount()
            : base(EALUIWndLayer.NORMAL)
        {
            _m_dEnterDelegate = null;
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPGGUIMonoUseAccount.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoUseAccount.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            _setServerIdByUid(String.Empty);
            ALUGUICommon.setInputTxt(wnd.userServerIP, CDNSetting_LoginServerUrlInfo.instance.defaultIP);
            ALUGUICommon.setInputTxt(wnd.userServerPort, CDNSetting_LoginServerUrlInfo.instance.defaultPort.ToString());
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.enterBtn, _onClickEnterBtn);
            ALUGUICommon.combineBtnClick(wnd.moreUserName, _onClickMoreUserNameBtn);
        }

        /// <summary>
        /// 设置进入的回调处理
        /// </summary>
        /// <param name="_delegate"></param>
        public void setEnterDelegate(Action _delegate)
        {
            _m_dEnterDelegate = _delegate;
        }

        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="_username"></param>
        public void setUserName(string _username)
        {
            if (null == _username)
                return;

            ALUGUICommon.setInputTxt(wnd.userName, _username);
        }
        /// <summary>
        /// 设置账号信息
        /// </summary>
        /// <param name="_username"></param>
        public void setAccount(InternalAccountInfo _accountInfo)
        {
            if(null == wnd)
                return;
            
            if (null == _accountInfo)
                return;

            ALUGUICommon.setInputTxt(wnd.userName, _accountInfo.userName);
            ALUGUICommon.setInputTxt(wnd.passWord, _accountInfo.password);
            
            ALUGUICommon.setInputTxt(wnd.userServerIP, CDNSetting_LoginServerUrlInfo.instance.defaultIP);
            ALUGUICommon.setInputTxt(wnd.userServerPort, CDNSetting_LoginServerUrlInfo.instance.defaultPort.ToString());
            _setServerIdByUid(_accountInfo.userName);
        }

        /**********************
         * 当点击进入按钮时的处理
         **/
        protected void _onClickEnterBtn(GameObject _go)
        {
            if(wnd.enterBtn == null || wnd.userName == null || wnd.passWord == null || wnd.userServerIP == null || wnd.userServerPort == null || wnd.userServerID == null)
            {
                Debug.LogError("控件有问题");
                return;
            }
            if(string.IsNullOrEmpty(wnd.userName.text))
            {//Please enter your username
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_username_empty_tip_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }
            if(string.IsNullOrEmpty(wnd.passWord.text))
            { //"Please enter your password",
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_password_empty_tip_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }
            if (null != wnd.userServerIP && string.IsNullOrEmpty(wnd.userServerIP.text))
            {//Please enter serverIP
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_serverId_empty_tip_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }
            if (null != wnd.userServerPort && string.IsNullOrEmpty(wnd.userServerPort.text))
            {//Please enter serverPort
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_serverPort_empty_tip_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }
            if(string.IsNullOrEmpty(wnd.userServerID.text))
            { //"Please enter your serverID",
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_serverID_number_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }

            int serverPort = 0;
            if (null != wnd.userServerPort && !int.TryParse(wnd.userServerPort.text, out serverPort))
            {//Please enter a serverPort number
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_serverPort_number_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }
            
            int serverID = 0;
            if (null != wnd.userServerID && !int.TryParse(wnd.userServerID.text, out serverID))
            {   //Please enter a serverID number
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_serverID_number_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }
            //设置当前选中登入的服务器id
            GameInit_SelectServer.instance.cheatLoginServerId = serverID;

            NPGCheatLSLoginServerInfo lsLoginServerInfo = new NPGCheatLSLoginServerInfo(wnd.userServerIP.text, serverPort);
            //进行用户登录操作
            GameInit_LoginProcess.instance.loginLS(ENPLSGameClientType.CHEAT, lsLoginServerInfo,wnd.userName.text, wnd.passWord.text,"");

            //调用回调
            if (null != _m_dEnterDelegate)
                _m_dEnterDelegate();
            _m_dEnterDelegate = null;
        }

        /**********************
        * 当点击进入按钮时的处理
        **/
        protected void _onClickMoreUserNameBtn(GameObject _go)
        {
            //判断是否有记录游戏登录名称
            if(InternalAccountMgr.instance.accountCount <= 0)
            {
                //展示提示
                Game.instance.UIC_showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.account_no_record_none));//#ui_err_no_record
                return;
            }

            //开启更多用户名处理
            QueueMgr.instance.AddNode(new LAddNodeLoginUserNameList(_onSelectPreName));
        }

        /// <summary>
        /// 选中之前用户名的时候的处理
        /// </summary>
        /// <param name="_name"></param>
        protected void _onSelectPreName(string _name)
        {
            //设置名称
            setUserName(_name);

            //获取数据
            InternalAccountInfo accInfo = InternalAccountMgr.instance.getAccountInfo(_name);
            if(null != accInfo)
            {
                //设置密码
                ALUGUICommon.setInputTxt(wnd.passWord, accInfo.password);
                
                //设置登入服务器ID
                _setServerIdByUid(_name);
            }
        }

        //设置登入服务器ID
        private void _setServerIdByUid(string _uid)
        {
            if(null == wnd)
                return;

            //找不到uid对应的服务器ID默认显示0
            string serverId = "0";
            
#if UNITY_EDITOR
            //编辑器下如果服务器id大于0则使用
            if (Game.instance.mainCamera.platInfo.serverId > 0)
            {
                serverId = Game.instance.mainCamera.platInfo.serverId.ToString();
            }
#endif
            
            int settingServerId = LoginTokenSetting.instance.getUidServerId(_uid);
            if (settingServerId > 0)
                serverId = settingServerId.ToString();
            
            //登入服务器ID
            ALUGUICommon.setInputTxt(wnd.userServerID, serverId);
        }
    }
}
