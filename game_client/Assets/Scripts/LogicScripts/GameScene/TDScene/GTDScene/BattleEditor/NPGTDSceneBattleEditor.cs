using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class NPGTDSceneBattleEditor : _ABasicTDScene
    {
        private static NPGTDSceneBattleEditor _g_instance;
        public static NPGTDSceneBattleEditor instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGTDSceneBattleEditor();

                return _g_instance;
            }
        }

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
