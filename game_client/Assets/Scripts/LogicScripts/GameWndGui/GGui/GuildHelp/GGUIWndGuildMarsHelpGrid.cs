using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟火星互助详情容器
    /// </summary>
    public class GGUIWndGuildMarsHelpGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildMarsHelpGridItem,GGUIMonoGuildMarsHelpGrid,GGUIWndGuildMarsHelpGridItem>
    {
        private List<_IGuildMarsHelpShow> _m_itemDataList = new List<_IGuildMarsHelpShow>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildMarsHelpGrid(GGUIMonoGuildMarsHelpGrid gridMono) : base(gridMono)
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

        protected override void _onRefreshItemWnd(GGUIWndGuildMarsHelpGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndGuildMarsHelpGridItem _createItemWnd(GGUIMonoGuildMarsHelpGridItem _itemMono)
        {
            GGUIWndGuildMarsHelpGridItem itemWnd = new GGUIWndGuildMarsHelpGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<_IGuildMarsHelpShow> _itemDataList)
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
