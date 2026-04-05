using System;
using System.Collections.Generic;

/// <summary>
/// Container架构中不存在子节点的根节点对象
/// </summary>
namespace ALPackage
{
    public abstract class _AALBasicSubContainerScene_NoChild : _AALBasicAdditionScene
    {
        //是否展示本Scene
        private bool _m_bIsShow;

        public _AALBasicSubContainerScene_NoChild(int _sceneTypeId) : base(_sceneTypeId)
        {
            _m_bIsShow = false;
        }

        public bool isShow { get { return _m_bIsShow; } }

        /// <summary>
        /// 在离开本Scene时触发的处理，禁止子类继承。对应处理可以重载_dealQuitScene函数进行处理
        /// </summary>
        protected override void _onQuitScene()
        {
            //统一处理退出Scene的操作
            _doQuitScene();
        }

        /// <summary>
        /// 默认简化调用接口
        /// </summary>
        public virtual void hideScene()
        {
            //判断是否已经进入，如果还未进入则直接调用回调
            //如果还未展示也先不处理
            if (!isEntered || !_m_bIsShow)
            {
                return;
            }

            _m_bIsShow = false;

            _dealHideScene(null);
        }
        public virtual void hideScene(Action _delegate)
        {
            //判断是否已经进入，如果还未进入则直接调用回调
            if (!isEntered || !_m_bIsShow)
            {
                if (_delegate != null)
                    _delegate();
                return;
            }

            _m_bIsShow = false;

            _dealHideScene(_delegate);
        }

        /// <summary>
        /// 显示本Scene
        /// </summary>
        public virtual void showScene()
        {
            //判断是否已经进入，如果还未进入则直接调用回调
            //如果已经展示也先不处理
            if (!isEntered || _m_bIsShow)
            {
                return;
            }

            _m_bIsShow = true;

            _dealShowScene(null);
        }
        public virtual void showScene(Action _delegate)
        {
            //判断是否已经进入，如果还未进入则直接调用进入和回调
            if (!isEntered || _m_bIsShow)
            {
                if (_delegate != null)
                    _delegate();
                return;
            }

            _m_bIsShow = true;

            _dealShowScene(_delegate);
        }

        /// <summary>
        /// 尝试进入并显示本Scene
        /// </summary>
        public virtual void enterAndShowScene()
        {
            //判断是否已经进入，如果还未进入则直接先处理进入
            if(!isEntered)
            {
                enterScene();
                regInitDelegate(() => { showScene(); });

                return;
            }

            //如果已经展示也先不处理
            if (_m_bIsShow)
            {
                return;
            }

            _m_bIsShow = true;

            _dealShowScene(null);
        }
        public virtual void enterAndShowScene(Action _delegate)
        {
            //判断是否已经进入，如果还未进入则直接先处理进入
            if (!isEntered)
            {
                enterScene();
                regInitDelegate(() => { showScene(_delegate); });

                return;
            }

            //判断是否已经进入，如果还未进入则直接调用进入和回调
            if (_m_bIsShow)
            {
                if (_delegate != null)
                    _delegate();
                return;
            }

            _m_bIsShow = true;

            _dealShowScene(_delegate);
        }

        /// <summary>
        /// 处理本UIScene的退出处理
        /// </summary>
        protected void _doQuitScene()
        {
            //调用子类处理
            //先调用隐藏才调用退出处理
            hideScene(_dealQuitScene);
        }

        /// <summary>
        /// 是否在切换的时候会被释放
        /// </summary>
        /// <returns></returns>
        public abstract bool needDiscardOnSwitch { get; }

        /// <summary>
        /// 由于本类重载之后需要处理的退出scene的处理
        /// </summary>
        protected abstract void _dealQuitScene();

        /// <summary>
        /// 初始化的显示窗口操作，显示完成则调用回调
        /// </summary>
        public abstract void _dealShowScene(Action _delegate);

        /// <summary>
        /// 隐藏本scene对象的操作
        /// </summary>
        /// <param name="_delegate"></param>
        public abstract void _dealHideScene(Action _delegate);

        /// <summary>
        /// 离开本附加Scene时的处理
        /// </summary>
        /// <param name="_view"></param>
        public abstract void onSwitchHideScene();
    }
}
