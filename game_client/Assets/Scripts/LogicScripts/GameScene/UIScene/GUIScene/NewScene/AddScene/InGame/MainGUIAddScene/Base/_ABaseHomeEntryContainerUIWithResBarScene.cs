using System;
using ALPackage;

namespace GOE
{
    public abstract class _ABaseHomeEntryContainerUIWithResBarScene : _ABaseHomeEntryContainerUIScene
    {
        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;
        
        protected _ABaseHomeEntryContainerUIWithResBarScene(bool _needDiscardOnSwitch) : base(_needDiscardOnSwitch)
        {
            _m_iIconSerialize = -1;
            _m_iBarSerialize = -1;
        }
        
        public void showResBar(long _barResId, long _playerIconResId, Action _complete = null)
        {
            hideResBar();
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_complete);

            _m_iIconSerialize = NPCommonPlayerResBarWndMgr.instance.showBar(_playerIconResId, stepCounter.addDoneStepCount);
            _m_iBarSerialize = NPCommonBarWndMgr.instance.showBar(_barResId, stepCounter.addDoneStepCount);

            return;
        }

        public void hideResBar()
        {
            if (_m_iBarSerialize > 0) 
                NPCommonBarWndMgr.instance.hideCurBar(_m_iBarSerialize);
            if (_m_iIconSerialize > 0) 
                NPCommonPlayerResBarWndMgr.instance.hideCurBar(_m_iIconSerialize);

            _m_iBarSerialize = -1;
            _m_iIconSerialize = -1;
        }

        protected override void _dealQuitScene()
        {
            //隐藏资源窗口
            hideResBar();

            base._dealQuitScene();
        }

        public override void _dealHideScene(Action _delegate)
        {
            //隐藏资源窗口
            hideResBar();

            base._dealHideScene(_delegate);
        }
        
        /// <summary>
        /// 显示一个带资源bar的主窗口
        /// </summary>
        /// <param name="_delegate"></param>
        public void showMainWndWithResBar<T>(T _wnd, Action _delegate) where T : _AALBasicLoadUIWndBasicClass, _INPGGUIBasicWndResBar
        {
            if (_wnd == null)
            {
                _delegate?.Invoke();
                return;
            }
            
            showMainWnd(_wnd, () =>
            {
                showResBar(_wnd.getBarResId(), _wnd.getPlayerIconResId(), _delegate);
            });
        }

        /// <summary>
        /// 显示一个不带资源bar的主窗口
        /// </summary>
        public void showMainWndWithOutResBar(_AALBasicLoadUIWndBasicClass _wnd, Action _delegate)
        {
            hideResBar();//隐藏资源bar
            showMainWnd(_wnd, _delegate);
        }
    }
}