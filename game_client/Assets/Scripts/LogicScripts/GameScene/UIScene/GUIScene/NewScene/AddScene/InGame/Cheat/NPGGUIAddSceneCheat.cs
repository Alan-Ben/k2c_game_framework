using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ALPackage;
using UnityEngine.UI;



namespace GOE
{
    /// <summary>
    /// 作弊命令输入窗口
    /// </summary>
    public class NPGGUIAddSceneCheat : _ANPBasicAddContainerUIScene
    {
        private static NPGGUIAddSceneCheat _g_instance = new NPGGUIAddSceneCheat();
        public static NPGGUIAddSceneCheat instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIAddSceneCheat();

                return _g_instance;
            }
        }

        public NPGGUIAddSceneCheat()
            : base()
        {

        }

        protected override void _onEnterScene()
        {
            //开启窗口加载
            NPGGUIWndCheat.instance.load(setSceneInited);
        }

        protected override void _dealQuitScene()
        {
            //卸载窗口
            NPGGUIWndCheat.instance.discard();
        }

        protected override void _onSceneInited()
        {
        }

        public override void _dealHideScene(Action _delegate)
        {
            NPGGUIWndCheat.instance.hideWnd();

            if(_delegate != null)
                _delegate();
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            NPGGUIWndCheat.instance.showWnd();
            GCommon.moveTransformToLastAndRefreshLayer(NPGGUIWndCheat.instance.wnd);
            if (null != _delegate)
                _delegate();
        }
    }
}
