using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /*******************
     * 游戏版本号显示 客户端版本号 服务端版本号 资源版本号
     **/
    public class NPPGUIWndVersion : _ANPGGUIBasicWnd<NPPGUIMonoVersion>
    {
        private static NPPGUIWndVersion _g_instance = new NPPGUIWndVersion();
        public static NPPGUIWndVersion instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPPGUIWndVersion();
                return _g_instance;
            }
        }

        protected NPPGUIWndVersion()
            : base(EALUIWndLayer.TOP)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoVersion.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoVersion.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
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
        }

        /**************
         * 设置客户端版本号
         **/
        public void setClientVersion(WCGClientInfo _clientVersionInfo)
        {
            if(null == wnd)
                return;

            string version = "";
            if(null != _clientVersionInfo)
                version = String.Format("{0} V{1}.{2}.{3}.{4}", Game.instance.mainCamera.curPlatName, _clientVersionInfo.majorVersion, _clientVersionInfo.minorVersion, _clientVersionInfo.revisionVersion, _clientVersionInfo.buildVersion);
            
            ALUGUICommon.setLabelTxt(wnd.ClientVersion, version);
        }

        /**************
         * 设置服务端版本号
         **/
        public void setServerVersion(string _serverVersion)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.ServerVersion,$"Server V:{_serverVersion}");
        }

        /**************
         * 设置资源版本号
         **/
        public void setAssetsVersion(string _assetsVersion)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.AssetsVersion,$"Res V:{_assetsVersion}");
        }

        /**************
         * 设置区域平台资源版本号
         **/
        public void setRefdataVersion(string _assetsVersion)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.refDataVersion, $"Refdata V:{_assetsVersion}");
        }
        
        /**************
         * 设置平台资源版本号
         **/
        public void setPlatVersion(string _platVersion)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.platVersion, $"Plat V:{_platVersion}");
        }
        
        /**************
         * 设置IF热更版本号
         **/
        public void setInjectFixVersion(string _injectFixVersion)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.InjectFixVersion, $"IF V:{_injectFixVersion}");
        }
        
        /**************
         * 设置Hotfix热更版本号
         **/
        public void setHotfixVersion(string _hotfixVersion)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.HotfixVersion, $"Hotfix V:{_hotfixVersion}");
        }
    }
}
