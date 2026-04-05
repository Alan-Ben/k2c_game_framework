using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 背包页签
    public class GGUIWndBagSecondTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {

        // 点击事件
        private Action<GGUIWndBagSecondTab> _m_dClickEvent;

        private GGUIBagTabSecondMono _m_secondItemMono; //物品类型枚举列表

        public GGUIWndBagSecondTab(GGUIBagTabSecondMono _mono) : base(_mono.monoTab)
        {
            _m_secondItemMono = _mono;
        }


        public Action<GGUIWndBagSecondTab> onClickTab { get { return _m_dClickEvent; } set { _m_dClickEvent = value; } }

        //物品类型枚举列表
        public GGUIBagTabSecondMono secondItemMono { get { return _m_secondItemMono; } }

        protected override void _onDiscard()
        {
            base._onDiscard();

            _m_dClickEvent = default(Action<GGUIWndBagSecondTab>);
        }

        // 响应点击事件
        protected override void _onClickSelectButton(GameObject _go)
        {
            if(null != _m_dClickEvent)
            {
                _m_dClickEvent(this);
            }
        }

    }
}
