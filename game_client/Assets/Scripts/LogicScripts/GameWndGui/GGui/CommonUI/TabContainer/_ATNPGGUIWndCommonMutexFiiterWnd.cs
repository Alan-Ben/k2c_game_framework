using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 通用互斥筛选窗口
    /// </summary>
    public abstract class _ATNPGGUIWndCommonMutexFiiterWnd<T, TMono> : _ANPGGUIBasicSubWnd<TMono>
        where TMono : _ATNPGGUIMonoCommonMutexFiiterWnd<T>
    {
        protected NPGGUIWndCommonFitterContainer<T> _m_tabContainer;
        protected List<NPGGUICommonFitterMono<T>> _m_lFitterTabConfigList;

        protected T _m_NowSelectType;//当前选中类型
        protected Action<NPGGUICommonFitterMono<T>> _m_aOnChgSelectTab;
        
        public T nowSelectType { get { return _m_NowSelectType; } }//当前选中的页签类型
        
        public _ATNPGGUIWndCommonMutexFiiterWnd(TMono _wnd, Action<NPGGUICommonFitterMono<T>> _onChgSelectTab) : base(_wnd)
        {
            _m_aOnChgSelectTab = _onChgSelectTab;
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.tabContainer != null)
            {
                _m_tabContainer = new NPGGUIWndCommonFitterContainer<T>(wnd.tabContainer, _onClickTabItem);
                _m_tabContainer.setDefaultSelected(_checkTabIsSelect);
            }

            if (_m_lFitterTabConfigList == null)
                _m_lFitterTabConfigList = new List<NPGGUICommonFitterMono<T>>();
            _m_lFitterTabConfigList.Clear();
            if (wnd.fitterTabConfigList != null)
            {
                _m_lFitterTabConfigList.AddRange(wnd.fitterTabConfigList);
            }
        }
        
        protected override void _onDiscard()
        {
            _m_tabContainer?.discard();
            _m_tabContainer = null;

            _m_lFitterTabConfigList?.Clear();
            _m_lFitterTabConfigList = null;
            
            _m_aOnChgSelectTab = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_tabContainer?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_tabContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_tabContainer?.resetWnd();
        }

        private bool _checkTabIsSelect(T _type)
        {
            // 比较_type和_m_eNowSelectType是否相等
            return EqualityComparer<T>.Default.Equals(_type, _m_NowSelectType);
        }
        
        // NPGGUIWndCommonFitterContainer<T>的点击回调函数
        private void _onClickTabItem(NPGGUIWndCommonFitterTab<T> _tab)
        {
            if(_tab == null || _tab.info == null)
                return;

            if (!EqualityComparer<T>.Default.Equals(_tab.info.type, _m_NowSelectType))
            {
                _chgSelectTab(_tab.info.type, true);
            }
        }

        private void _chgSelectTab(T _type, bool _needCallChgCallBack = false)
        {
            // 查找要选中的tab配置
            NPGGUICommonFitterMono<T> tabConfig = _findTabConfig(_type);
            
            // 找不到对应的tab配置时, 使用默认tab类型展示
            if (tabConfig == null)
            {
                if (wnd != null)
                {
                    _type = wnd.defaultSelectType;
                    tabConfig = _findTabConfig(_type);
                }
            }
            
            // 若还是没有找到对应tab配置, 直接返回
            if(tabConfig == null)
                return;
            
            _m_NowSelectType = _type;
            _m_tabContainer?.showList(_m_lFitterTabConfigList);
            _onChgSelectTab(tabConfig);
                
            if(_needCallChgCallBack)
                _m_aOnChgSelectTab?.Invoke(tabConfig);
        }

        protected virtual void _onChgSelectTab(NPGGUICommonFitterMono<T> _tabMono)
        {
        }

        /// <summary>
        /// 设置选中的type
        /// </summary>
        /// <param name="_type"></param>
        public void setSelectType(T _type, bool _needCallChgCallBack = false)
        {
            _chgSelectTab(_type, _needCallChgCallBack);
        }

        /// <summary>
        /// 设置选中默认类型
        /// </summary>
        public void selectDefaultType(bool _needCallChgCallBack = false)
        {
            if(wnd != null)
                _chgSelectTab(wnd.defaultSelectType, _needCallChgCallBack);
        }

        protected NPGGUICommonFitterMono<T> _findTabConfig(T _tab)
        {
            if (_m_lFitterTabConfigList == null)
                return null;
            
            // 遍历_m_lFitterTabConfigList，找到类型为_tab的元素
            foreach (var item in _m_lFitterTabConfigList)
            {
                if(item == null)
                    continue;
                
                if (EqualityComparer<T>.Default.Equals(item.type, _tab))
                    return item;
            }

            return null;
        }
    }
}