using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIWndBagSelectItemContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoBagSelectItemContainerItem>
    {
        public GGUIWndBagSelectItemContainerItem(GGUIMonoBagSelectItemContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        private NPGGUIWndCommonItem _m_wItem;//物品

        private NPCommonCostItem _m_costItem;

        private int _m_iIndex;
        public int Index { get { return _m_iIndex; } set { _m_iIndex = value; } }


        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if(_m_wItem != null)
                _m_wItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(_m_wItem != null)
                _m_wItem.discard();
            _m_wItem = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if(wnd.monoItem != null)
                _m_wItem = new NPGGUIWndCommonItem(wnd.monoItem);

            _m_iIndex = -1;

            GameObject temp = null;
            for (int i = 0; i < wnd.btnSelectList.Count; i++)
            {
                temp = wnd.btnSelectList[i];
                if (null == temp)
                    continue;

                ALUGUICommon.combineBtnClick(temp, _onClickSelectButton);
            }
        }

        //选中按钮
        private void _onClickSelectButton(GameObject _go)
        {
            //设置显示处理
            GGUIWndBagSelectItemContainer.instance.chgIndex(_m_iIndex);
        }

        //设置选中状态
        public void setSelected(bool _isSelect)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goSelected, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.selectAllCountTxt, _isSelect);

        }

        //设置物品信息
        public void setItem(NPCommonCostItem _info)
        {
            if (null == _info)
                return;

            _m_costItem = _info;

            _m_wItem.setItem(_info.toCommonItemData());

        }

        //刷新数量显示
        public void setSelectCount(int _selectCount)
        {
            if (null == _m_costItem)
                return;

            ALUGUICommon.setLabelTxt(wnd.selectAllCountTxt, "x" + _m_costItem.count * _selectCount);

        }
    }
}
