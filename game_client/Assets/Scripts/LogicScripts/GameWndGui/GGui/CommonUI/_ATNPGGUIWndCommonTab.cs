using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用页签
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class _ATNPGGUIWndCommonTab<T, Y> : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
        where Y : _ATNPGGUIWndCommonTab<T, Y>
    {
        public Action<Y> onClickTab { get { return _m_dClickEvent; } set { _m_dClickEvent = value; } }
        public T tabType { get { return _m_eTabType; } }

        // 点击事件
        private Action<Y> _m_dClickEvent;
        private T _m_eTabType;

        public _ATNPGGUIWndCommonTab(NPGGUIMonoCommonTab _wnd, T _bagItemType) : base(_wnd)
        {
            _m_eTabType = _bagItemType;
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            _m_dClickEvent = default(Action<Y>);
        }

        // 响应点击事件
        protected override void _onClickSelectButton(GameObject _go)
        {
            if(null != _m_dClickEvent)
            {
                _m_dClickEvent((Y) this);
            }
        }
    }
    
    /// <summary>
    /// 通用页签
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class _ATNPGGUIWndCommonTab<T, T_TabMono, T_TabWnd> : _ATNPGGUIWndTabItem<T_TabMono>
        where T_TabMono : NPGGUIMonoCommonTab
        where T_TabWnd : _ATNPGGUIWndCommonTab<T, T_TabMono, T_TabWnd>
    {
        public Action<T_TabWnd> onClickTab { get { return _m_dClickEvent; } set { _m_dClickEvent = value; } }
        public T tabType { get { return _m_eTabType; } }

        // 点击事件
        private Action<T_TabWnd> _m_dClickEvent;
        private T _m_eTabType;

        public _ATNPGGUIWndCommonTab(T_TabMono _wnd, T _bagItemType) : base(_wnd)
        {
            _m_eTabType = _bagItemType;
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            _m_dClickEvent = default(Action<T_TabWnd>);
        }

        // 响应点击事件
        protected override void _onClickSelectButton(GameObject _go)
        {
            if(null != _m_dClickEvent)
            {
                _m_dClickEvent((T_TabWnd) this);
            }
        }
    }
}
