using System;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动附加排行榜界面页签
    /// </summary>
    public class GGuiWndRegularEventSubRankTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        //页签类型
        private string _m_sTabTag;
        //点击回调
        private Action<GGuiWndRegularEventSubRankTab> _m_dClickButton;

        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<GGuiWndRegularEventSubRankTab> onClickButton { get { return _m_dClickButton; } set { _m_dClickButton = value; } }
        /// <summary>
        /// 页签类型
        /// </summary>
        public string tabTag { get { return _m_sTabTag; } }

        public GGuiWndRegularEventSubRankTab(NPGGUIMonoCommonTab _wnd, string _tag) : base(_wnd)
        {
            _m_sTabTag = _tag;
            initWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_dClickButton = default(Action<GGuiWndRegularEventSubRankTab>);
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