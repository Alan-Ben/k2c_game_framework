using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using ALPackage;
using JetBrains.Annotations;


namespace GOE
{
    /// <summary>
    /// 游戏UI场景的管理对象，不同的视图切换只是在本UI管理器内进行的子类型切换
    /// </summary>
    public class GUISceneMain : _ABasicUIScene
    {
        private static GUISceneMain _g_instance = new GUISceneMain();
        [NotNull]
        public static GUISceneMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GUISceneMain();

                return _g_instance;
            }
        }

        /** 当前显示对象的状态 */
        private bool _m_bCanShowItemNotice = true;

        protected override void _onEnterScene()
        {
            GGUIWndMain.instance.load(setSceneInited);
        }

        protected override void _onSceneInited()
        {
            //注册消息
            //WinMsg.RegisterMsgAct(WinMsgType.ENTER_BOSS_BATTLE, _onEnterBossBattle);
            //WinMsg.RegisterMsgAct(WinMsgType.QUIT_BOSS_BATTLE, _onQuitBossBattle);
        }

        protected override void _dealQuitScene()
        {
            GGUIWndMain.instance.discard();
            //注销消息
            //WinMsg.UnregisterMsgAct(WinMsgType.ENTER_BOSS_BATTLE, _onEnterBossBattle);
            //WinMsg.UnregisterMsgAct(WinMsgType.QUIT_BOSS_BATTLE, _onQuitBossBattle);
        }

        /***************
         * 是否需要检查教程
         **/
        public override bool needCheckTutorial { get { return true; } }
        /***************
         * 是否允许展示获取物品信息
         **/
        public override bool canShowNotice { get { return _m_bCanShowItemNotice; } }

        /** 设置不需要展示物品信息 */
        public void setCanNotShowItemNotice()
        {
            _m_bCanShowItemNotice = false;
        }
        public void setNeedShowItemNotice()
        {
            if(_m_bCanShowItemNotice)
                return;

            _m_bCanShowItemNotice = true;
        }
    }
}
