using System;
using ALPackage;

namespace GOE
{
    public abstract class _AGGUIEntryPointFollowItemBaseController<T,W> : _ATALGGUICommonFollowItemController<T, W>
        where T : _AGGUIMonoEntryPointFollowItemBase
        where W : _AGGUIWndEntryPointFollowItemBase<T>
    {
        private GResPathIndex _m_index;

        protected _AGGUIEntryPointFollowItemBaseController(int _resId) : base()
        {          
            _m_index = new GResPathIndex(_resId);
        }
        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }

        //设置资源样式id
        public void initUIResId(int _resId)
        {
            if(0 == _resId)
                return;
            
            _m_index =  new GResPathIndex(_resId);
        }
        
        public void setShowData(EntryPointRefObj _entryPointRefObj)
        {
            regItemWndLoadDoneDelegate(() =>
            {
                wnd?.showWnd(_entryPointRefObj);
            });
        }

        /// <summary>
        /// 播放解锁动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playUnlockAni(Action _onPlayDone)
        {
            if (null == wnd)
            {
                if (_onPlayDone != null) 
                    _onPlayDone();
                return;
            }
            
            wnd.playUnlockAni(_onPlayDone);
        }

        /// <summary>
        /// 设置解锁动画状态
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_normalizeTime"></param>
        public void setUnlockAniSample(EEntryPointAniType _type, long _normalizeTime)
        {
            if (wnd == null)
                return;

            wnd.setUnlockAniSample(_type, _normalizeTime);
        }

        /// <summary>
        /// 播放屏幕隐藏显示动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playScreenShowHideAni(bool _isShow, Action _onPlayDone)
        {
            if (null == wnd)
            {
                if (_onPlayDone != null) 
                    _onPlayDone();
                return;
            }
            
            wnd.playScreenShowHideAni(_isShow, _onPlayDone);
        }
    }
}