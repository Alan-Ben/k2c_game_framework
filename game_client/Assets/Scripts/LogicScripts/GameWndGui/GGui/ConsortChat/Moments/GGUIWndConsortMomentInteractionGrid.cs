using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 中文窗口注释容器
    /// </summary>
    public class GGUIWndConsortMomentInteractionGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoConsortMomentInteractionGridItem,GGUIMonoConsortMomentInteractionGrid,GGUIWndConsortMomentInteractionGridItem>
    {
        private List<ConsortMomentConsortAICommentData> _m_itemDataList = new List<ConsortMomentConsortAICommentData>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndConsortMomentInteractionGrid(GGUIMonoConsortMomentInteractionGrid gridMono) : base(gridMono)
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

        protected override void _onRefreshItemWnd(GGUIWndConsortMomentInteractionGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndConsortMomentInteractionGridItem _createItemWnd(GGUIMonoConsortMomentInteractionGridItem _itemMono)
        {
            GGUIWndConsortMomentInteractionGridItem itemWnd = new GGUIWndConsortMomentInteractionGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<ConsortMomentConsortAICommentData> _itemDataList)
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
