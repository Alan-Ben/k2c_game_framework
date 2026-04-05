using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会玩家交互item容器
    /// </summary>
    public class GGUIWndDinnerInteractItemGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoDinnerInteractItem,GGUIMonoDinnerInteractItemGrid,GGUIWndDinnerInteractItem>
    {
        private List<Dinner_JoinerLogList> _m_itemDataList;
        
        public GGUIWndDinnerInteractItemGrid(GGUIMonoDinnerInteractItemGrid _wnd) : base(_wnd)
        {
            initWnd();
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
        }

        protected override void _onWndInitDone()
        {
        }

        protected override GGUIWndDinnerInteractItem _createItemWnd(GGUIMonoDinnerInteractItem _itemMono)
        {
            GGUIWndDinnerInteractItem itemWnd = new GGUIWndDinnerInteractItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemwnd(GGUIWndDinnerInteractItem _itemMono, int _itemIdx)
        {
            if(_itemIdx <0 || _itemIdx >= _m_itemDataList.Count)
                return;
            
            _itemMono.setInfo(_m_itemDataList[_itemIdx]);
        }
        
        public void showItemList(List<Dinner_JoinerLogList> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList = _itemDataList;
            _m_itemDataList.Sort(_sortItem);
            setItemCount(_m_itemDataList.Count);
            ALUGUICommon.setGameObjEnable(wnd.emptyListHide,_m_itemDataList.Count != 0);
            ALUGUICommon.setGameObjEnable(wnd.emptyListShow,_m_itemDataList.Count == 0);
        }
        

        private int _sortItem(Dinner_JoinerLogList x, Dinner_JoinerLogList y)
        {
            return y.getBeJoinedCount().CompareTo(x.getBeJoinedCount());
        }
    }
}
