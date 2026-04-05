using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{

    /// <summary>
    /// 好友系统
    /// </summary>
    public class GMainGUIAddSceneFriends : _ANPBasicAddContainerUIScene
    {
        private static GMainGUIAddSceneFriends _g_instance = new GMainGUIAddSceneFriends();
        public static GMainGUIAddSceneFriends instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GMainGUIAddSceneFriends();

                return _g_instance;
            }
        }

        protected override void _onEnterScene()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(setSceneInited);

            GGUIWndFriendsMain.instance.load(stepCounter.addDoneStepCount);
        }

        protected override void _dealQuitScene()
        {
            GGUIWndFriendsMain.instance.discard();

        }

        protected override void _onSceneInited() { }


        public override void _dealHideScene(Action _delegate)
        {
            GGUIWndFriendsMain.instance.hideWnd();

            if (null != _delegate)
                _delegate();
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndFriendsMain.instance.showWnd();

            if (null != _delegate)
                _delegate();
        }

        /// <summary>
        /// 离开视图时的处理
        /// </summary>
        /// <param name="_view"></param>
        public override void onSwitchHideScene()
        {

        }
    }
}
