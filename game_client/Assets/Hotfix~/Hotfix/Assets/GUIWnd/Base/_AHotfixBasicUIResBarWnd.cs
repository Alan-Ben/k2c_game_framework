using System;
using ALPackage;
using GOE;

namespace Hotfix
{
    public abstract class _AHotfixBasicUIResBarWnd<T> : _AHotfixBaseWnd<T>
        where T : _AHotfixBasicUIResBarMono, new()
    {
        //显示资源栏的操作序列号
        private int _m_iIconSerialize;
        private int _m_iBarSerialize;
        
        public _AHotfixBasicUIResBarWnd(EALUIWndLayer _layer) : base(_layer)
        {
            _m_iIconSerialize = -1;
            _m_iBarSerialize = -1;
        }

        public override void showWnd(Action _delayDoneAction)
        {
            base.showWnd(()=>
            {
                if(showResBarBySelf)
                    showResBar(_delayDoneAction);
                else
                    _delayDoneAction?.Invoke();
            });
        }

        public override void hideWnd(Action _delayDoneAction)
        {
            if(showResBarBySelf)
                hideResBar();
            
            base.hideWnd(_delayDoneAction);
        }

        /// <summary>
        /// 是否通过窗口自己显示资源bar
        /// </summary>
        /// <returns></returns>
        public virtual bool showResBarBySelf { get { return true; } }

        public void showResBar(Action _complete = null)
        {
            if (hotfixWnd == null)
            {
                _complete?.Invoke();
                return;
            }

            showResBar(hotfixWnd.barResId, hotfixWnd.playerIconResId, _complete);
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
        
        public long getPlayerIconResId()
        {
            
            if (null == hotfixWnd)
            {
#if UNITY_EDITOR
                Debug.LogError($"界面还没初始化完就去取playrIcon的资源，注意查看");
#endif
                return 0;
            }

            return hotfixWnd.playerIconResId;
        }

        /// <summary>
        /// 获取资源barid
        /// </summary>
        /// <returns></returns>
        public long getBarResId()
        {
            if (null == hotfixWnd)
            {
#if UNITY_EDITOR
                Debug.LogError($"界面还没初始化完就去取bar的资源，注意查看");
#endif
                return 0;
            }

            return hotfixWnd.barResId;
        }
    }
}