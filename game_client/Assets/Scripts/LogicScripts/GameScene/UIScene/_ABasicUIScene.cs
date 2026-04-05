using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /***********************
     * 附加UI视图的基类对象
     **/
    public abstract class _ABasicUIScene : _AALBasicTopContainerScene
    {
        /**********
         * 退出所有附加uiscene
         **/
        public static void quitCurUIScene()
        {
            ALSceneCore.instance.quitCurScene((int)ENPSceneType.UI_SCENE);
        }

        /**************
         * 获取当前所在的场景类型
         **/
        public static _ABasicUIScene curWCGUIScene
        {
            get
            {
                if(!(ALSceneCore.instance.getCurScene((int)ENPSceneType.UI_SCENE) is _ABasicUIScene))
                    return null;

                return (_ABasicUIScene)ALSceneCore.instance.getCurScene((int)ENPSceneType.UI_SCENE);
            }
        }

        protected _ABasicUIScene()
            : base((int)ENPSceneType.UI_SCENE)
        {
        }

        protected override void _onSwitchScene(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWnd(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
            //暂不做处理
        }

        protected override void _onSwitchSceneDone(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWndDone(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
            //暂不做处理
        }

        /***************
         * 是否需要检查教程
         **/
        public abstract bool needCheckTutorial { get; }
        /***************
         * 是否允许展示提示类信息
         **/
        public abstract bool canShowNotice { get; }
    }
}
