using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 带主城跟随的主界面入口scene基类
    /// </summary>
    public abstract class _ABaseHomeEntryContainerUIScene : _ANPBasicAddContainerUIScene
    {
        //展示完成回调
        private Action _m_aOnShowDone;
        protected _ABaseHomeEntryContainerUIScene(bool _needDiscardOnSwitch)
            : base(_needDiscardOnSwitch)
        {
        }
        
        protected override void _onEnterScene()
        {
            GGUIWndHomeEntryFollow.instance.load(_onEnterSceneEx);
        }

        protected override void _dealQuitScene()
        {
            GGUIWndHomeEntryFollow.instance.discard();
            
            _dealQuitSceneEx();
        }
        
        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            GGUIWndHomeEntryFollow.instance.showWnd(() =>
            {
                _dealShowSceneEx(()=>
                {
                    _delegate?.Invoke();

                    //展示完成回调
                    _m_aOnShowDone?.Invoke();
                    _m_aOnShowDone = null;
                });
            });
        }

        public override void _dealHideScene(Action _delegate)
        {
            GGUIWndHomeEntryFollow.instance.hideWnd(() =>
            {
                _dealHideSceneEx(_delegate);
            });
        } 
        
        /// <summary>
        /// 注册显示完成回调
        /// </summary>
        /// <param name="_action"></param>
        public void regShowDone(Action _action)
        {
            if (_action == null)
                return;

            if (isShow)
                _action();
            else
            {
                if (_m_aOnShowDone == null)
                    _m_aOnShowDone = _action;
                else
                    _m_aOnShowDone += _action;
            }
        }


        protected abstract void _onEnterSceneEx();
        
        protected abstract void _dealQuitSceneEx();
        
        protected abstract void _dealShowSceneEx(Action _delegate);
        
        protected abstract void _dealHideSceneEx(Action _delegate);

    }
}