using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 带消耗使用物品按钮
    public class GGUIWndBagCostUseBtn : _ATALBasicUISubWnd<GGUIMonoBagCostUseBtn>
    {
        //消耗展示
        private NPGGUIWndCommonItem _m_commonItemWnd;

        private NPCommonCostItem _m_costItem;

        //点击使用回调
        private Action _m_useDelegate;
        public GGUIWndBagCostUseBtn(GGUIMonoBagCostUseBtn _wnd,Action _useDelegate)
            : base(_wnd)
        {
            _m_useDelegate = _useDelegate;
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null != wnd.costItem)
                _m_commonItemWnd = new NPGGUIWndCommonItem(wnd.costItem);

            ALUGUICommon.combineBtnClick(wnd.useBtn, _onUseBtnClick);

            WinMsg.RegisterMsgAct(WinMsgType.ON_BAG_ITEM_USE, _refresh);
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        { 
        }
        protected override void _onDiscard()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.discard();
            _m_commonItemWnd = null;

            WinMsg.UnregisterMsgAct(WinMsgType.ON_BAG_ITEM_USE, _refresh);

            ALUGUICommon.uncombineBtnClick(wnd.useBtn, _onUseBtnClick);

            _m_useDelegate = null;
        }

        public void setItem(List<NPCommonCostItem> _costItemList)
        {
            if (null == _m_commonItemWnd)
                return;

            bool isShowCost = false;
            if(null != _costItemList && _costItemList.Count > 0)
            {
                isShowCost = true;
                _m_costItem = _costItemList[0];
                _m_commonItemWnd.setItem(_m_costItem);
            }
            ALUGUICommon.setGameObjEnable(_m_commonItemWnd.getGameObj(), isShowCost);
            ALUGUICommon.setGameObjEnable(wnd.costHideGoList, !isShowCost);
        }

        public void setItem(NPCommonCostItem _costItem)
        {
            if (null == _costItem)
                return;

            List<NPCommonCostItem> list = new List<NPCommonCostItem>();
            list.Add(_costItem);
            setItem(list);
        }

        public void setSelectCount(long _selectCount)
        {
            if(null != _m_costItem && null != _m_commonItemWnd)
            {
                NPCommonCostItem countCostItem = new NPCommonCostItem(_m_costItem.item, _selectCount * _m_costItem.count);
                _m_commonItemWnd.setItem(countCostItem);
            }
        }

        private void _refresh()
        {
            _m_commonItemWnd.setItem(_m_costItem);
        }
        private void _onUseBtnClick(GameObject _go)
        {
            if (null != _m_useDelegate)
                _m_useDelegate();
        }
        
        /// <summary>
        /// 模拟使用按钮点击
        /// </summary>
        public void simulateClickUseBtn()
        {
            _onUseBtnClick(null);
        }
        
        public RectTransform getUseBtnRectTransform()
        {
            return null == wnd || wnd.useBtn == null ? null : (RectTransform)wnd.useBtn.transform;
        }
    }
}
