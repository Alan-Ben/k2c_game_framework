using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /*******************
     *登录界面的开始游戏界面
     **/
    public class NPGGUIWndStartGame : _ANPGGUIBasicWnd<NPGGUIMonoStartGame>
    {
        private static NPGGUIWndStartGame _g_instance = new NPGGUIWndStartGame();
        public static NPGGUIWndStartGame instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIWndStartGame();
                return _g_instance;
            }
        }

        //定时任务
        private ALCommonEnableTaskController _m_checkTask;
        //是否自动打开选服界面
        private bool _m_bAutoOpenSelectServer;
        
        private long _m_serialize = 0;
        
        protected NPGGUIWndStartGame()
            : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPGGUIMonoStartGame.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoStartGame.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /// <summary>
        /// 是否自动打开选服界面
        /// </summary>
        public bool isAutoOpenSelectServer { get { return _m_bAutoOpenSelectServer; } set { _m_bAutoOpenSelectServer = value; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            _m_serialize = ALSerializeOpMgr.next();
            
            WinMsg.SendMsg(WinMsgType.ON_START_GAME_WND_SHOW);
            //刷新当前选中服务器信息
            refreshCurServer();
            
            //注册回调
            GameInit_SelectServer.instance.regServerChgDelegate(refreshCurServer);
            
            if(Game.instance.isUseCdn)
                _startCheck();

            //是否自动打开选服界面，如果是则直接打开
            if (_m_bAutoOpenSelectServer)
            {
                //打开选服界面后重置标记，避免重复打开
                _m_bAutoOpenSelectServer = false;
                _onClickSwitchServerBtn(null);
            }
        }

        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            _m_serialize = ALSerializeOpMgr.next();

            //注销回调
            GameInit_SelectServer.instance.unregServerChgDelegate(refreshCurServer);
            
            _stopCheck();
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
            ALUGUICommon.uncombineBtnClick(wnd.enterBtn, _onClickEnterBtn);
            ALUGUICommon.uncombineBtnClick(wnd.switchServerBtn, _onClickSwitchServerBtn);
        }

        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.enterBtn, _onClickEnterBtn);
            ALUGUICommon.combineBtnClick(wnd.switchServerBtn, _onClickSwitchServerBtn);
        }

        /// <summary>
        /// 当点击进入游戏的处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClickEnterBtn(GameObject _go)
        {
            if (wnd == null)
                return;

            //发送埋点-点击开始游戏
            GCommon.sendStepReport(TraceConst.CLICK_START_GAME);
            _m_serialize = ALSerializeOpMgr.next();

            //停止自动刷新
            _stopCheck();
            
            GameInit_SelectServer.instance.getCurSelectServerItem(serverId =>
            {
                ServerDataInfo serverInfo = CDNSetting_ServerListInfo.instance.getServerDataInfo(serverId);
                EServerOnlineState onlineState = serverInfo != null ? (EServerOnlineState) serverInfo.onlineState: EServerOnlineState.OPEN;
                if (!GameSetting.instance.isWhite && onlineState != EServerOnlineState.OPEN)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.server_isUnderMaintenance_none);//服务器维护中
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.loginShowGoList, true);
                    ALUGUICommon.setGameObjEnable(wnd.loginHideGoList, false);

                    //调用请求进入服务器处理
                    GameInit_SelectServer.instance.reqEnterUs();
                }
            });
        }

        /// <summary>
        /// 当点击切换账号按钮时的处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClickSwitchServerBtn(GameObject _go)
        {
            QueueMgr.instance.addNode_Login_MainUIAddWnd(NPGGUIWndServerList.instance, UINodeTagConst.C_ADD_Login_Server_List);
        }

        /// <summary>
        /// 刷新当前选中的服务器
        /// </summary>
        public void refreshCurServer()
        {
            //发送埋点-点击开始游戏
            GCommon.sendStepReport(TraceConst.GET_RECOMMOND_SERVER);
            
            GameInit_SelectServer.instance.getCurSelectServerItem((serverId) =>
            {
                GCommon.sendStepReport(TraceConst.GET_RECOMMOND_SERVER_DONE.setMarkParam(serverId));

                if (wnd == null)
                    return;

                //刷新显示
                if (null != wnd.serverItem)
                    wnd.serverItem.setServerInfo(CDNSetting_ServerListInfo.instance.getServerDataInfo(serverId));
            
                ALUGUICommon.setGameObjEnable(wnd.loginShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.loginHideGoList, true);

                //如果是白名单账号则直接显示进入按钮
                if (GameSetting.instance.isWhite)
                    ALUGUICommon.setGameObjEnable(wnd.enterBtn, true);
            });
        }
        
        /// <summary>
        /// 开启定时检查
        /// </summary>
        private void _startCheck()
        {
            _m_checkTask.setDisable();
            _m_checkTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCheck, 10.0f);
        }

        /// <summary>
        /// 关闭定时检查
        /// </summary>
        private void _stopCheck()
        {
            _m_checkTask.setDisable();
        }
        
        /// <summary>
        /// 定时检查函数
        /// </summary>
        private void _tickCheck()
        {
            _m_serialize = ALSerializeOpMgr.next();
            long serialize = _m_serialize;
            //5秒重新获取1次服务器状态
            CDNSetting_ServerListInfo.instance.clearData();
            CDNSetting_ServerListInfo.instance.commonDownload(() =>
            {
                if (serialize != _m_serialize)
                    return;
                
                refreshCurServer();
            });
        }
    }
}