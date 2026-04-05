using System;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动消耗商店界面页签
    /// </summary>
    public class GGuiWndRegularEventShopTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        //页签类型
        private string _m_sTabTag;
        //点击回调
        private Action<GGuiWndRegularEventShopTab> _m_dClickButton;

        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<GGuiWndRegularEventShopTab> onClickButton { get { return _m_dClickButton; } set { _m_dClickButton = value; } }
        /// <summary>
        /// 页签类型
        /// </summary>
        public string tabTag { get { return _m_sTabTag; } }

        public GGuiWndRegularEventShopTab(NPGGUIMonoCommonTab _wnd,string _tag) : base(_wnd)
        {
            _m_sTabTag = _tag;
            initWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_dClickButton = default(Action<GGuiWndRegularEventShopTab>);
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