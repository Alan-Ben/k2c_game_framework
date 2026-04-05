using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会举办历史item容器
    /// </summary>
    public class GGUIWndDinnerStartItemGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoDinnerStartItem,GGUIMonoDinnerStartItemGrid,GGUIWndDinnerStartItem>
    {
        
        private List<DinnerStartLogIdx> _m_itemDataList;
        
        public GGUIWndDinnerStartItemGrid(GGUIMonoDinnerStartItemGrid _wnd) : base(_wnd)
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

        protected override GGUIWndDinnerStartItem _createItemWnd(GGUIMonoDinnerStartItem _itemMono)
        {
            GGUIWndDinnerStartItem itemWnd = new GGUIWndDinnerStartItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemwnd(GGUIWndDinnerStartItem _itemMono, int _itemIdx)
        {
            if(_itemIdx <0 || _itemIdx >= _m_itemDataList.Count)
                return;
            
            _itemMono.setInfo(_m_itemDataList[_itemIdx]);
        }
        
        public void showItemList(List<DinnerStartLogIdx> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList = _itemDataList;
            _m_itemDataList.Sort(_sortItem);
            setItemCount(_m_itemDataList.Count);
            ALUGUICommon.setGameObjEnable(wnd.emptyListHide,_m_itemDataList.Count != 0);
            ALUGUICommon.setGameObjEnable(wnd.emptyListShow,_m_itemDataList.Count == 0);
        }
        
        private int _sortItem(DinnerStartLogIdx x, DinnerStartLogIdx y)
        {
            if (x.startTs > y.startTs)
                return -1;
            if (x.startTs < y.startTs)
                return 1;
            return 0;
        }
    }
}
