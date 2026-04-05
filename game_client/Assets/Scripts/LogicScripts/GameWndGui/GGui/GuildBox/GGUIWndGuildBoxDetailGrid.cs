using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱详情容器
    /// </summary>
    public class GGUIWndGuildBoxDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildBoxDetailGridItem,GGUIMonoGuildBoxDetailGrid,GGUIWndGuildBoxDetailGridItem>
    {
        private List<NPCommonCostItem> _m_itemDataList = new List<NPCommonCostItem>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildBoxDetailGrid(GGUIMonoGuildBoxDetailGrid gridMono) : base(gridMono)
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
            if(null == wnd)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndGuildBoxDetailGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndGuildBoxDetailGridItem _createItemWnd(GGUIMonoGuildBoxDetailGridItem _itemMono)
        {
            GGUIWndGuildBoxDetailGridItem itemWnd = new GGUIWndGuildBoxDetailGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<NPCommonCostItem> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            _m_itemDataList.Sort(_sortList);
            setItemCount(_m_itemDataList.Count);
        }

        // 排序方法： 品质从高到低，同品质按id从大到小
        private int _sortList(NPCommonCostItem _a, NPCommonCostItem _b)
        {
            if (_a == null || _b == null)
                return 0;

            if (_a.getQuality() != _b.getQuality())
                return -(_a.getQuality().CompareTo(_b.getQuality()));

            return -_a.subId.CompareTo(_b.subId);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
