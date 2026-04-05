using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 物品概率详情界面容器
    /// </summary>
    public class GGUIWndItemPercentDetailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoItemPercentDetailGridItem,GGUIMonoItemPercentDetailGrid,GGUIWndItemPercentDetailGridItem>
    {
        private List<NPCommonCostItem> _m_itemDataList = new List<NPCommonCostItem>();
        private List<int> _m_itemWeightList = new List<int>();
        private int _m_iTotalWeight;
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndItemPercentDetailGrid(GGUIMonoItemPercentDetailGrid gridMono) : base(gridMono)
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
            _m_itemDataList.Clear();
            _m_itemWeightList.Clear();
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndItemPercentDetailGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _m_itemDataList== null ||  _itemIdx >= _m_itemDataList.Count)
                return;

            int weight = 0;
            if(_m_itemWeightList != null && _itemIdx < _m_itemWeightList.Count)
                weight = _m_itemWeightList[_itemIdx];
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx], weight, _m_iTotalWeight);
        }

        protected override GGUIWndItemPercentDetailGridItem _createItemWnd(GGUIMonoItemPercentDetailGridItem _itemMono)
        {
            GGUIWndItemPercentDetailGridItem itemWnd = new GGUIWndItemPercentDetailGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<NPCommonCostItem> _itemDataList,  List<int> _itemWeightList, int _totalWeight)
        {
            if (_itemDataList == null || _itemWeightList == null)
                return;

            _m_iTotalWeight = _totalWeight;

            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            
            _m_itemWeightList.Clear();
            _m_itemWeightList.AddRange(_itemWeightList);
            setItemCount(_m_itemDataList.Count);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
