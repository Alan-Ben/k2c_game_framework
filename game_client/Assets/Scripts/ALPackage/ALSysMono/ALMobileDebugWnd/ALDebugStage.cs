﻿using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_UNITY_GUI
/**********************
* 系统信息的相关GUIstage
**/
namespace ALPackage
{
    public class ALDebugStage : _AALBaseGuiStage
    {
        private static ALDebugStage _g_instance = new ALDebugStage();
        public static ALDebugStage instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALDebugStage();

                return _g_instance;
            }
        }

        /****************
         * 窗口的定义相关枚举
         **/
        public enum EALDebugWnd
        {
            NONE,
            WND,
        }

        protected ALDebugStage()
            : base((int)0)
        {
        }
        /****************
         * 初始化本场景的函数
         **/
        protected override void _init()
        {
            //注册本窗口
            ALDebugWnd.init();
            regWnd(EALDebugWnd.WND, ALDebugWnd.instance);

            //设置本stage永久在上
            alwaysTop = true;
        }

        /****************
         * 在进入本场景时调用的事件函数
         **/
        protected override void _onEnterStage()
        {
            ALDebugWnd.instance.show();
        }

        /****************
         * 在退出本场景时调用的事件函数
         **/
        protected override void _onQuitStage()
        {
            //无退出操作
        }
    }
}

#endif
