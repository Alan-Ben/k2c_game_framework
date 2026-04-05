using System;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴相性筛选按钮子窗口页签
    /// </summary>
    public class GGUIWndHeroMainFilterTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        //属性
        public Action<GGUIWndHeroMainFilterTab> onClickButton
        {
            get { return _m_dClickButton; }
            set { _m_dClickButton = value; }
        }

        public ESpecAttrType tabType { get { return _m_eTabType; } }

        //成员变量
        private ESpecAttrType _m_eTabType;
        private GameObject _m_goTag;
        private Action<GGUIWndHeroMainFilterTab> _m_dClickButton;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_wnd">窗口mono</param>
        /// <param name="_tabType">页签类型</param>
        public GGUIWndHeroMainFilterTab(NPGGUIMonoCommonTab _wnd, ESpecAttrType _tabType, GameObject _goTag)
            : base(_wnd)
        {
            _m_eTabType = _tabType;
            _m_goTag = _goTag;
            _m_dClickButton = default(Action<GGUIWndHeroMainFilterTab>);
            initWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            _m_dClickButton = default(Action<GGUIWndHeroMainFilterTab>);
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            setShowTag(false);
        }

        protected override void _onClickSelectButton(GameObject _go)
        {
            if (null != _m_dClickButton)
            {
                _m_dClickButton(this);
            }
        }

        /// <summary>
        /// 设置标签显隐
        /// </summary>
        /// <param name="_isShow"></param>
        public void setShowTag(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(_m_goTag, _isShow);
        }
    }
}
