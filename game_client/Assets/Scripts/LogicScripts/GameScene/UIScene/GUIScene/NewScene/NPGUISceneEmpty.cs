using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 战斗匹配
    /// </summary>
    public class NPGUISceneEmpty : _ABasicUIScene
    {
        private static NPGUISceneEmpty _g_instance = new NPGUISceneEmpty();
        public static NPGUISceneEmpty instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGUISceneEmpty();

                return _g_instance;
            }
        }

        public NPGUISceneEmpty()
            : base()
        {
        }



        protected override void _onEnterScene()
        {
            //开启Scene所有窗口加载
            setSceneInited();
        }

        protected override void _dealQuitScene()
        {
        }

        protected override void _onSceneInited()
        {
        }

        /***************
         * 是否需要检查教程
         **/
        public override bool needCheckTutorial { get { return false; } }
        /***************
         * 是否允许展示获取物品信息
         **/
        public override bool canShowNotice { get { return false; } }
    }
}
