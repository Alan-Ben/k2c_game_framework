using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /***********************
     * 附加UI视图的基类对象
     **/
    public abstract class _ABasicAdditionUIScene_NoChild : _AALBasicSubContainerScene_NoChild
    {
        protected _ABasicAdditionUIScene_NoChild()
            : base((int)ENPSceneType.UI_SCENE)
        {
        }

        /// <summary>
        /// 是否在切换的时候会被释放
        /// </summary>
        /// <returns></returns>
        public override bool needDiscardOnSwitch { get { return true; } }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            if (null != _delegate)
                _delegate();
        }

        /// <summary>
        /// 隐藏本scene对象的操作
        /// </summary>
        /// <param name="_delegate"></param>
        public override void _dealHideScene(Action _delegate)
        {
            if (null != _delegate)
                _delegate();
        }

        /// <summary>
        /// 由于本类重载之后需要处理的退出scene的处理
        /// </summary>
        protected override void _dealQuitScene() { }

        /// <summary>
        /// 离开本附加Scene时的处理
        /// </summary>
        /// <param name="_view"></param>
        public override void onSwitchHideScene() { }
    }

    /// <summary>
    /// AddScene容器类
    /// </summary>
    public abstract class _ANPBasicAddContainerUIScene : _AALBasicSubContainerScene
    {
        //是否在切换的时候需要释放
        private bool _m_bNeedDiscardOnSwitch;

        protected _ANPBasicAddContainerUIScene()
            : base((int)ENPSceneType.UI_SCENE)
        {
            _m_bNeedDiscardOnSwitch = true;
        }
        protected _ANPBasicAddContainerUIScene(bool _needDiscardOnSwitch)
            : base((int)ENPSceneType.UI_SCENE)
        {
            _m_bNeedDiscardOnSwitch = _needDiscardOnSwitch;
        }

        protected override void _onSwitchScene(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWnd(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
        }

        protected override void _onSwitchSceneDone(_AALBasicSubContainerScene_NoChild _tarScene)
        {
        }
        protected override void _onSwitchWndDone(_AALBasicLoadUIWndBasicClass _tarWnd)
        {
        }

        /// <summary>
        /// 离开视图时的处理
        /// </summary>
        /// <param name="_view"></param>
        public override void onSwitchHideScene()
        {
        }

        /// <summary>
        /// 是否在切换的时候会被释放
        /// </summary>
        /// <returns></returns>
        public override bool needDiscardOnSwitch { get { return _m_bNeedDiscardOnSwitch; } }
    }
}
