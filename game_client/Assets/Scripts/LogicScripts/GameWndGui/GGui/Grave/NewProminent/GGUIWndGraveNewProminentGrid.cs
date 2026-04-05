using System;
using System.Collections.Generic;
using ALPackage;
using Common.GraveObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 新晋者列表界面容器
    /// </summary>
    public class GGUIWndGraveNewProminentGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGraveNewProminentGridItem,GGUIMonoGraveNewProminentGrid,GGUIWndGraveNewProminentGridItem>
    {
        private List<GraveObj_NewInfo> _m_itemDataList = new List<GraveObj_NewInfo>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGraveNewProminentGrid(GGUIMonoGraveNewProminentGrid gridMono) : base(gridMono)
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

        protected override void _onRefreshItemWnd(GGUIWndGraveNewProminentGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndGraveNewProminentGridItem _createItemWnd(GGUIMonoGraveNewProminentGridItem _itemMono)
        {
            GGUIWndGraveNewProminentGridItem itemWnd = new GGUIWndGraveNewProminentGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<GraveObj_NewInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            setItemCount(_m_itemDataList.Count);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
