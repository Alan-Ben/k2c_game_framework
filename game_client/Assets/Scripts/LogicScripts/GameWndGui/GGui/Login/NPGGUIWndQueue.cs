using UnityEngine;
using UnityEngine.UI;
using ALPackage;
using NPEnum;
using NPCommon;

namespace GOE
{
    /*******************
     *登录界面的开始游戏界面
     **/
    public class NPGGUIWndQueue : _ANPGGUIBasicWnd<NPGGUIMonoQueue>
    {
        private static NPGGUIWndQueue _g_instance = new NPGGUIWndQueue();
        public static NPGGUIWndQueue instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIWndQueue();
                return _g_instance;
            }
        }

        protected NPGGUIWndQueue()
            : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPGGUIMonoQueue.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoQueue.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            if (null == wnd)
                return;

            //重置信息
            ALUGUICommon.setLabelTxt(wnd.textQueue, string.Empty);
            
            //注册回调
            GameInit_SelectServer.instance.regServerChgDelegate(_refreshCurServer);
        }

        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            ALUGUICommon.setLabelTxt(wnd.textQueue, string.Empty);
            
            //注销回调
            GameInit_SelectServer.instance.unregServerChgDelegate(_refreshCurServer);
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
            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.quitQueueBtn, _onClickQuitBtn);
        }

        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.switchServerBtn, _onClickSwitchServerBtn);
            //绑定退出按钮操作
            ALUGUICommon.combineBtnClick(wnd.quitQueueBtn, _onClickQuitBtn);
        }

        /// <summary>
        /// 显示对应的文本信息
        /// </summary>
        /// <param name="_info"></param>
        public void showInfo(string _info)
        {
            if (null == wnd)
                return;

            //重置信息
            ALUGUICommon.setLabelTxt(wnd.textQueue, _info);
        }
        
        public void showInfo(string _info, long _serverLogicId)
        {
            if (null == wnd)
                return;

            //重置信息
            ALUGUICommon.setLabelTxt(wnd.textQueue, _info);

            //设置服务器信息
            if (null != wnd.serverItem)
                wnd.serverItem.setServerInfo(CDNSetting_ServerListInfo.instance.getServerDataInfo(_serverLogicId));
        }

        /// <summary>
        /// 当点击进入游戏的处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClickQuitBtn(GameObject _go)
        {
            //发送消息退出处理
            GameInit_SelectServer.instance.quitQueue();
        }

        /// <summary>
        /// 当点击切换服务器处理
        /// </summary>
        /// <param name="_go"></param>
        protected void _onClickSwitchServerBtn(GameObject _go)
        {
            QueueMgr.instance.addNode_Login_MainUIAddWnd(NPGGUIWndServerList.instance, UINodeTagConst.C_ADD_Login_Server_List);
        }
        
        
        /// <summary>
        /// 当选择切换服务器
        /// </summary>
        protected void _refreshCurServer()
        {
            //发送消息退出处理
            GameInit_SelectServer.instance.quitQueue();
            
            //展示开始游戏界面
            QueueMgr.instance.addNode_Login_MainUIMainWnd(NPGGUIWndStartGame.instance, UINodeTagConst.C_Login_Start_Game, false, false, null ,null);

        }
    }
}