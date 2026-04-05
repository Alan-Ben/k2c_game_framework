using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndDinnerDetailLogItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoDinnerDetailLogItem,GGUIMonoDinnerDetailLogItemGrid,GGUIWndDinnerDetailLogItem>
    {
       
        private List<DinnerDetailLogInfo> _m_itemDataList = new List<DinnerDetailLogInfo>();

        public GGUIWndDinnerDetailLogItemGrid(GGUIMonoDinnerDetailLogItemGrid gridMono) : base(gridMono)
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

        protected override void _onRefreshItemWnd(GGUIWndDinnerDetailLogItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemMono.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndDinnerDetailLogItem _createItemWnd(GGUIMonoDinnerDetailLogItem _itemMono)
        {
            GGUIWndDinnerDetailLogItem itemWnd = new GGUIWndDinnerDetailLogItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(GDinnerInfo _dinnerInfo)
        {
            if (_dinnerInfo == null)
                return;

            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_dinnerInfo.getDetailLogList());
            setItemCount(_m_itemDataList.Count);
        }
    }
}
