using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /************************
    * 游戏版本号信息：客户端版本号 服务端版本号 资源版本号
    **/
    public class PUIAddSceneVersion : _ABasicAdditionUIScene_NoChild
    {
        private static PUIAddSceneVersion _g_instance = new PUIAddSceneVersion();
        public static PUIAddSceneVersion instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new PUIAddSceneVersion();

                return _g_instance;
            }
        }

        public PUIAddSceneVersion()
            : base()
        {
        }

        protected override void _onEnterScene()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_START_GAME_WND_SHOW, onNeedRefreshVersion);
            WinMsg.RegisterMsg(WinMsgType.ON_US_ENTER_DONE, onNeedRefreshVersion);

            //开启窗口加载
            NPPGUIWndVersion.instance.load(setSceneInited);
        }

        protected override void _dealQuitScene()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_START_GAME_WND_SHOW, onNeedRefreshVersion);
            WinMsg.UnregisterMsg(WinMsgType.ON_US_ENTER_DONE, onNeedRefreshVersion);

            //卸载窗口
            NPPGUIWndVersion.instance.discard();
        }

        protected override void _onSceneInited()
        {
            _refreshShow();
        }

        private void onNeedRefreshVersion(object[] _objs)
        {
            _refreshShow();
        }
        
        private void _refreshShow()
        {
            //显示窗口
            NPPGUIWndVersion.instance.showWnd();

            //设置客户端版本号
            NPPGUIWndVersion.instance.setClientVersion(ClientVersionSetting.instance.ClientVersionInfo);
            
            //设置服务端版本号 默认还没有获取到值就不显示
            NPPGUIWndVersion.instance.setServerVersion(GameResCore.instance.serverVersion);
            //设置客户端资源版本号 默认还没有获取到值就不显示
            NPPGUIWndVersion.instance.setAssetsVersion(GameResCore.instance.remoteVersionNum.ToString());
            //设置配表资源
            NPPGUIWndVersion.instance.setRefdataVersion(RefdataResCore.instance.remoteVersionNum.ToString());
            //设置平台资源版本号
            NPPGUIWndVersion.instance.setPlatVersion(PlatResCore.instance.localVersionNum.ToString());
            //设置IF热更版本号
            NPPGUIWndVersion.instance.setInjectFixVersion(GameSetting.instance.getLastInjectFixVersion());
            //设置Hotfix热更版本号
            NPPGUIWndVersion.instance.setHotfixVersion(ALHotfixMgr_ILRuntime_Global.instance.version.versionStrNum);
        }
    }
}
