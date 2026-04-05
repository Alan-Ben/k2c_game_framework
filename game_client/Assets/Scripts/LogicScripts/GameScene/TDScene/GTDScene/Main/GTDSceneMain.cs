using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GTDSceneMain : _ABasicTDScene
    {
        private static GTDSceneMain _g_instance;
        [NotNull]
        public static GTDSceneMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GTDSceneMain();

                return _g_instance;
            }
        }

        //返回数据源
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onSceneInited()
        {
          
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
            // //此处确认对应的scene是否有效，如果无效则需要释放
            // if (_tarScene is NPMainAdditionCityTDScene)
            // {
            //     NPMainAdditionCityTDScene cityScene = (NPMainAdditionCityTDScene)_tarScene;
            //     if (null != cityScene)
            //     {
            //         //如果需要重载则强制退出
            //         if (cityScene.checkNeedReload())
            //         {
            //             cityScene.forceQuitAddScene();
            //         }
            //     }
            // }
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
            if(null == _tarScene)
                return;
         
#if UWA_GPM
            //上报场景切换
            // GameInit_UWA_GPM.instance.changeScene(_tarScene.GetType().Name);
#endif
        }
        protected override void _onSwitchWndDone(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
            //暂不做处理
        }
    }
}
