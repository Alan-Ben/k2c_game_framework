using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟副本升级容器
    /// </summary>
    public class GGUIWndGuildDungeonUpgradeGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildDungeonUpgradeGridItem,GGUIMonoGuildDungeonUpgradeGrid,GGUIWndGuildDungeonUpgradeGridItem>
    {
        private List<GuildDungeonInfo> _m_itemDataList = new List<GuildDungeonInfo>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonUpgradeGrid(GGUIMonoGuildDungeonUpgradeGrid gridMono) : base(gridMono)
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

        protected override void _onRefreshItemWnd(GGUIWndGuildDungeonUpgradeGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndGuildDungeonUpgradeGridItem _createItemWnd(GGUIMonoGuildDungeonUpgradeGridItem _itemMono)
        {
            GGUIWndGuildDungeonUpgradeGridItem itemWnd = new GGUIWndGuildDungeonUpgradeGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<GuildDungeonInfo> _itemDataList)
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
