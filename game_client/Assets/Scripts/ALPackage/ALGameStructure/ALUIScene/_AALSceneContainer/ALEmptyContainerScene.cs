using System;
using System.Collections.Generic;

/************************
 * 可在Container框架内被包含或被容纳的scene对象
 **/
namespace ALPackage
{
    /// <summary>
    /// 一个空的Scene容器
    /// 这样的容器可以作为任意一个对象的子对象，对某一些局部对象进行管理
    /// 比如某个窗口下的多个页签
    /// </summary>
    public class ALEmptyContainerScene : _AALBasicSubContainerScene
    {
        //是否在切换的时候需要释放
        private bool _m_bNeedDiscardOnSwitch;

        public ALEmptyContainerScene()
            : base(0)
        {
            _m_bNeedDiscardOnSwitch = true;
        }
        public ALEmptyContainerScene(int _sceneTypeId)
            : base(_sceneTypeId)
        {
            _m_bNeedDiscardOnSwitch = true;
        }
        public ALEmptyContainerScene(int _sceneTypeId, bool _needDiscardOnSwitch)
            : base(_sceneTypeId)
        {
            _m_bNeedDiscardOnSwitch = _needDiscardOnSwitch;
        }

        /// <summary>
        /// 是否在切换的时候会被释放
        /// </summary>
        /// <returns></returns>
        public override bool needDiscardOnSwitch { get { return _m_bNeedDiscardOnSwitch; } }

        /** 在进入本场景时调用的事件函数 */
        protected override void _onEnterScene()
        {
            //本场景是空的，因此直接设置完成
            setSceneInited();
        }


        /** 在初始化本场景完成后调用的事件函数 */
        protected override void _onSceneInited()
        {
        }

        /// <summary>
        /// 由于本类重载之后需要处理的退出scene的处理
        /// </summary>
        protected override void _dealQuitScene()
        {
        }

        /// <summary>
        /// 初始化的显示窗口操作，显示完成则调用回调
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
        }

        /// <summary>
        /// 隐藏本scene对象的操作
        /// </summary>
        /// <param name="_delegate"></param>
        public override void _dealHideScene(Action _delegate)
        {
        }

        /// <summary>
        /// 离开本附加Scene时的处理
        /// </summary>
        /// <param name="_view"></param>
        public override void onSwitchHideScene()
        {
        }

        /// <summary>
        /// 在尝试切换Scene的时候触发的函数
        /// </summary>
        protected override void _onSwitchScene(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWnd(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
        }

        /// <summary>
        /// 在切换Scene完成的时候触发的函数
        /// </summary>
        protected override void _onSwitchSceneDone(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWndDone(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
        }
    }
}

