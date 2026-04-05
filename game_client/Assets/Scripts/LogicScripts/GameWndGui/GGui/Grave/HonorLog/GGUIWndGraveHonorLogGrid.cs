using System;
using System.Collections.Generic;
using ALPackage;
using Common.GraveObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 荣誉列表界面容器
    /// </summary>
    public class GGUIWndGraveHonorLogGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGraveHonorLogGridItem,GGUIMonoGraveHonorLogGrid,GGUIWndGraveHonorLogGridItem>
    {
        private List<GraveObj_Record> _m_itemDataList = new List<GraveObj_Record>();
        private int _m_totalCount = 0; //总条数
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGraveHonorLogGrid(GGUIMonoGraveHonorLogGrid gridMono) : base(gridMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            // <AutoGen:_onShowWnd>
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            // <AutoGen:_onHideWnd>
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            // </AutoGen:_onWndInitDone>
        }

        protected override void _onRefreshItemWnd(GGUIWndGraveHonorLogGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx], _itemIdx, _m_totalCount);

        }

        protected override GGUIWndGraveHonorLogGridItem _createItemWnd(GGUIMonoGraveHonorLogGridItem _itemMono)
        {
            GGUIWndGraveHonorLogGridItem itemWnd = new GGUIWndGraveHonorLogGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(int _totalCount, List<GraveObj_Record> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_totalCount = _totalCount;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            setItemCount(_m_itemDataList.Count);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
