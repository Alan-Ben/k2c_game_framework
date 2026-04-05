using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会item容器
    /// </summary>
    public class GGUIWndDinnerItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoDinnerItem,GGUIMonoDinnerItemGrid,GGUIWndDinnerItem>
    {
        private List<DinnerIndex> _m_itemDataList = new List<DinnerIndex>();

        public GGUIWndDinnerItemGrid(GGUIMonoDinnerItemGrid gridMono) : base(gridMono)
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
            
            if(wnd == null)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndDinnerItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemMono.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndDinnerItem _createItemWnd(GGUIMonoDinnerItem _itemMono)
        {
            GGUIWndDinnerItem itemWnd = new GGUIWndDinnerItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<DinnerIndex> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            setItemCount(_m_itemDataList.Count);
        }
    }
}
