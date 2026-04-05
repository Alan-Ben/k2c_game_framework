using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 骑士排序按钮子窗口页签
    /// </summary>
    public class GGUIWndHeroMainSortTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        //属性
        public Action<GGUIWndHeroMainSortTab> onClickButton { get { return _m_dClickButton; } set { _m_dClickButton = value; } }
        public EHeroListSortTabType tabType { get { return _m_eTabType; } }

        //成员变量
        private EHeroListSortTabType _m_eTabType;
        private Action<GGUIWndHeroMainSortTab> _m_dClickButton;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_wnd">窗口mono</param>
        /// <param name="_tabType">页签类型</param>
        public GGUIWndHeroMainSortTab(NPGGUIMonoCommonTab _wnd, EHeroListSortTabType _tabType)
            : base(_wnd)
        {
            _m_eTabType = _tabType;
            _m_dClickButton = default(Action<GGUIWndHeroMainSortTab>);
            initWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_dClickButton = default(Action<GGUIWndHeroMainSortTab>);
        }

        protected override void _onClickSelectButton(GameObject _go)
        {
            if (null != _m_dClickButton)
            {
                _m_dClickButton(this);
            }
        }
    }
}
