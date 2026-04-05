using System;
using System.Collections.Generic;
using UnityEngine;

using ALPackage;

namespace GOE
{
    /*****************
     * 正常的一个3D场景的加载，无特殊处理类
     **/
    public class PTDSceneNormal : _ABasicTDScene
    {
        private static PTDSceneNormal _g_instance;
        public static PTDSceneNormal instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new PTDSceneNormal();

                return _g_instance;
            }
        }

        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

        protected override void _onSceneInited()
        {
            //设置摄像头位置
            CameraController.instance.refreshByCameraSetting(PLoginCommonInfo.instance.obj.cameraSettingInfo);
        }

        /** 推出场景的函数 */
        protected override void _onQuitTDScene()
        {
        }

        /// <summary>
        /// 在尝试切换Scene的时候触发的函数
        /// </summary>
        protected override void _onSwitchScene(_AALBasicSubContainerScene_NoChild _tarScene)
        {
            //暂不做处理
        }
        protected override void _onSwitchWnd(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
            //暂不做处理
        }

        /// <summary>
        /// 在切换Scene完成的时候触发的函数
        /// </summary>
        protected override void _onSwitchSceneDone(_AALBasicSubContainerScene_NoChild _tarScene)
        {
            //暂不做处理
        }
        protected override void _onSwitchWndDone(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
            //暂不做处理
        }
    }
}
