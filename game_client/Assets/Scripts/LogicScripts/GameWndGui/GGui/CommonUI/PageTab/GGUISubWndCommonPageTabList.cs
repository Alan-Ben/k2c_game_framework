using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public abstract class GGUISubWndCommonPageTabList<T, Y, U> : _ANPGGUIBasicSubWnd<Y> 
        where T : Enum
        where Y : GGUIMonoCommonPageTabList<T, U>
        where U : GGUIMonoCommonPageTabItem<T>
    {
        [ItemNotNull][NotNull] private readonly List<GGUISubWndCommonPageTabItem<T>> _m_tabItemList;
        private GGUISubWndCommonPageTabItem<T> _m_curSelectTab;
        private bool _m_bInitSelectDefault;

        protected GGUISubWndCommonPageTabList(Y _wnd, bool _initSelectDefault = true) : base(_wnd)
        {
            _m_tabItemList = new List<GGUISubWndCommonPageTabItem<T>>();
            _m_bInitSelectDefault = _initSelectDefault;
        }

        protected override void _onShowWnd()
        {
            foreach (GGUISubWndCommonPageTabItem<T> tabItem in _m_tabItemList)
            {
                tabItem.showWnd();
            }
        }

        protected override void _onHideWnd()
        {
            foreach (GGUISubWndCommonPageTabItem<T> tabItem in _m_tabItemList)
            {
                tabItem.hideWnd();
            }
        }

        protected override void _onReset()
        {
            foreach (GGUISubWndCommonPageTabItem<T> tabItem in _m_tabItemList)
            {
                tabItem.resetWnd();
            }
        }

        protected override void _onDiscard()
        {
            foreach (GGUISubWndCommonPageTabItem<T> tabItem in _m_tabItemList)
            {
                tabItem.discard();
            }
            _m_tabItemList.Clear();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            GGUISubWndCommonPageTabItem<T> defaultSelectWnd = null;
            if (wnd.pageTabs != null)
            {
                foreach (U pageTabMono in wnd.pageTabs)
                {
                    if (pageTabMono == null)
                        continue;
                    
                    GGUISubWndCommonPageTabItem<T> tabItemWnd = new GGUISubWndCommonPageTabItem<T>(pageTabMono, _tryToCreatePageWnd);
                    tabItemWnd.onClick += _onSelectTab;
                    if (defaultSelectWnd == null || pageTabMono.tabType.Equals(wnd.defaultTabType))
                    {
                        defaultSelectWnd = tabItemWnd;
                    }
                    _m_tabItemList.Add(tabItemWnd);
                }
            }

            if(_m_bInitSelectDefault)
                _onSelectTab(defaultSelectWnd);
        }

        public void setSelectTab(T _type)
        {
            foreach (GGUISubWndCommonPageTabItem<T> pageTabItem in _m_tabItemList)
            {
                if (pageTabItem.wnd.tabType.Equals(_type))
                {
                    _onSelectTab(pageTabItem);
                    return;
                }
            }
        }
        
        /// <summary>
        /// 选中默认Tab
        /// </summary>
        public void selectDefaultTab()
        {
            if (wnd == null)
                return;
            
            setSelectTab(wnd.defaultTabType);
        }
        public _AALBasicLoadUIWndBasicClass getPageWnd(T _type)
        {
            foreach (GGUISubWndCommonPageTabItem<T> pageTabItem in _m_tabItemList)
            {
                if (pageTabItem.wnd.tabType.Equals(_type))
                    return pageTabItem.getPageWnd();
            }

            return null;
        }
        public void actionForAllLoadedPageWndSync(Action<_AALBasicLoadUIWndBasicClass> _action)
        {
            if (_action == null)
                return;

            foreach (GGUISubWndCommonPageTabItem<T> pageTabItem in _m_tabItemList)
            {
                _action.Invoke(pageTabItem.getPageWnd());
            }
        }
        /// <summary>
        /// 设置红点显示
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_needShow"></param>
        public void setTabRedTipShow(T _type, bool _needShow)
        {
            foreach (GGUISubWndCommonPageTabItem<T> pageTabItem in _m_tabItemList)
            {
                if(pageTabItem != null && pageTabItem.wnd != null && pageTabItem.wnd.tabType.Equals(_type))
                    pageTabItem.setRedTipShow(_needShow);
            }
        }
        /// <summary>
        /// 获取当前选中页签类型
        /// </summary>
        /// <returns></returns>
        public T getCurSelectType()
        {
            if (_m_curSelectTab == null || _m_curSelectTab.wnd == null)
                return default(T);

            return _m_curSelectTab.wnd.tabType;
        }

        private void _onSelectTab(GGUISubWndCommonPageTabItem<T> _wnd)
        {
            if (_wnd == null || _wnd == _m_curSelectTab)
                return;

            _m_curSelectTab?.setSelectShow(GGUIMonoCommonPageTabItemShowState.UNSELECTED);
            _m_curSelectTab = _wnd;
            _m_curSelectTab.setSelectShow(GGUIMonoCommonPageTabItemShowState.SELECTED);

            if(_m_curSelectTab.wnd != null)
                _onAfterClickSelectTab(_m_curSelectTab.wnd.tabType);
        }

        private _AALBasicLoadUIWndBasicClass _tryToCreatePageWnd(T _type)
        {
            return _createPageWnd(_type, wnd == null ? null : wnd.transPageParent);
        }
        
        // 根据T类型获取U
        protected U _getPageTabItem(T _type)
        {
            if (wnd == null || wnd.pageTabs == null)
                return null;
            
            // 直接通过wnd中的pageTabs获取
            foreach (U pageTabItem in wnd.pageTabs)
            {
                if (pageTabItem != null && pageTabItem.tabType != null && pageTabItem.tabType.Equals(_type))
                    return pageTabItem;
            }

            return null;
        }

        /// <summary>
        /// 点击选中页签之后事件
        /// </summary>
        /// <param name="_type"></param>
        protected virtual void _onAfterClickSelectTab(T _type) { }
        /// <summary>
        /// 创建页面窗口
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_wndParent"></param>
        /// <returns></returns>
        protected abstract _AALBasicLoadUIWndBasicClass _createPageWnd(T _type, Transform _wndParent);
    }
}