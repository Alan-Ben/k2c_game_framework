using UnityEngine;
using System.Collections;
using ALPackage;
using System;

namespace GOE
{
    /************************
    * 尝试网络重联（菊花）
    **/
    public class NPGGUIAddSceneReconnecting : _ABasicAdditionUIScene_NoChild
    {
        private static NPGGUIAddSceneReconnecting _g_instance = new NPGGUIAddSceneReconnecting();
        public static NPGGUIAddSceneReconnecting instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIAddSceneReconnecting();

                return _g_instance;
            }
        }


        public NPGGUIAddSceneReconnecting()
            : base()
        {
            
        }

        protected override void _onEnterScene()
        {
            _afterTransBkAction();
        }

        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            //开启窗口加载
            NPPGUIWndReconnecting.instance.load(_onLoadingInited);
        }

        protected override void _dealQuitScene()
        {
            //卸载窗口
            NPPGUIWndReconnecting.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        protected void _onLoadingInited()
        {
            //显示窗口
            NPPGUIWndReconnecting.instance.showWnd();
        }
    }
}
