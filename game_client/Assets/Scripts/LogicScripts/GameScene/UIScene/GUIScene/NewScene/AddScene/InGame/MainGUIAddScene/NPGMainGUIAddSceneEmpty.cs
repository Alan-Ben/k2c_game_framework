using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    public class NPGMainGUIAddSceneEmpty : _ANPBasicAddContainerUIScene
    {
        private static NPGMainGUIAddSceneEmpty _g_instance = new NPGMainGUIAddSceneEmpty();
        public static NPGMainGUIAddSceneEmpty instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGMainGUIAddSceneEmpty();

                return _g_instance;
            }
        }

        protected NPGMainGUIAddSceneEmpty()
            : base(false)
        {
        }

        protected override void _onEnterScene()
        {
            setSceneInited();
        }

        protected override void _dealQuitScene()
        {
        }

        protected override void _onSceneInited()
        {
        }

        public override void _dealHideScene(Action _delegate)
        {
            if(!isEntered)
            {
                if(_delegate != null)
                    _delegate();

                return;
            }

            if(_delegate != null)
                _delegate();
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            if (null != _delegate)
                _delegate();
        }
    }
}
