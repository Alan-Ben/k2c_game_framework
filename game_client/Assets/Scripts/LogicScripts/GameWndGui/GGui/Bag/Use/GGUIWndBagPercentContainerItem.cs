using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 背包概率物品
    public class GGUIWndBagPercentContainerItem : _ATALBasicUISubWnd<GGUIMonoBagPercentContainerItem>
    {
        private NPGGUIWndCommonItem _m_itemWnd;
        public GGUIWndBagPercentContainerItem(GGUIMonoBagPercentContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }


        // 初始化
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (null != _m_monoWnd)
                _m_itemWnd = new NPGGUIWndCommonItem(wnd.itemWnd);
        }
        protected override void _onShowWnd()
        {

        }
        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (null != _m_itemWnd)
                _m_itemWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_itemWnd)
                _m_itemWnd.discard();
            _m_itemWnd = null;
        }



        // 刷新item
        public void refreshItem(CommonItemData _item, int percent, int _totalWeight)
        {
            if(null == wnd)
                return;
            
            if (_item == null)
                return;
            if (null != _m_itemWnd)
                _m_itemWnd.setItem(_item);
            //设置概率值
            if (null != wnd.itemWnd)
                ALUGUICommon.setLabelTxt(wnd.percentTxt, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, 1.0f * percent / _totalWeight * 100));
        }
    }
}
