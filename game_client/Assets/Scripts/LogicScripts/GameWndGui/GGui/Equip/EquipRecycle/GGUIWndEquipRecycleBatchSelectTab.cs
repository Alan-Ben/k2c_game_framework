using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品分解选择页签
    /// </summary>
    public class GGUIWndEquipRecycleBatchSelectTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        //属性
        public Action<GGUIWndEquipRecycleBatchSelectTab> onClickButton { get { return _m_dClickButton; } set { _m_dClickButton = value; } }
        public EEquipRecycleBatchSelectTabType tabType { get { return _m_eTabType; } }

        //成员变量
        private EEquipRecycleBatchSelectTabType _m_eTabType;
        private Action<GGUIWndEquipRecycleBatchSelectTab> _m_dClickButton;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_wnd">窗口mono</param>
        /// <param name="_tabType">页签类型</param>
        public GGUIWndEquipRecycleBatchSelectTab(NPGGUIMonoCommonTab _wnd, EEquipRecycleBatchSelectTabType _tabType)
            : base(_wnd)
        {
            _m_eTabType = _tabType;
            _m_dClickButton = default(Action<GGUIWndEquipRecycleBatchSelectTab>);
            initWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_dClickButton = default(Action<GGUIWndEquipRecycleBatchSelectTab>);
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
