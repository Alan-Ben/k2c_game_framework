using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝选择能源item
    /// </summary>
    public class GGUIWndTreasureHuntSelectEnergyItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntSelectEnergyItem>
    {
        private NPCommonItem _m_itemData;
        
        private GGUIWndCommonSimpleItem _m_wItem;
        
        public GGUIWndTreasureHuntSelectEnergyItem(GGUIMonoTreasureHuntSelectEnergyItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<GGUIWndTreasureHuntSelectEnergyItem> onItemClick;//当item被点击
        
        public NPCommonItem itemData { get { return _m_itemData; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoItem != null)
                _m_wItem = new GGUIWndCommonSimpleItem(wnd.monoItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
            }

            onItemClick = null;
            
            _m_wItem?.discard();
            _m_wItem = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItem?.resetWnd();
        }

        public void setData(NPCommonItem _itemData)
        {
            _m_itemData = _itemData;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_itemData == null || !isShow)
                return;

            // 设置道具信息
            if (_m_wItem != null)
            {
                _m_wItem.showWnd();
                _m_wItem.setItem(_m_itemData);
            }

            long itemCount = GCommon.getItemCount(_m_itemData);
            if (itemCount <= 0)
            {
                GGameCommonInfo.grayImage(wnd.noItemNumGrayList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.noItemNumGrayList);
            }
        }

        public void setUsing(bool _using)
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.usingShowGoList, _using);
        }
        
        public void setSelected(bool _selected)
        {
            if(wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.selectedShowGoList, _selected);
        }

        private void _onClickItem(GameObject _go)
        {
            onItemClick?.Invoke(this);
        }
    }
}