using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟PVE自动开启容器
    /// </summary>
    public class GGUIWndGuildDungeonAutoOpenGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildDungeonAutoOpenGridItem,GGUIMonoGuildDungeonAutoOpenGrid,GGUIWndGuildDungeonAutoOpenGridItem>
    {
        private List<GuildDungeonInfo> _m_itemDataList = new List<GuildDungeonInfo>();
        [NotNull]private List<long> _m_curSelectDungeonIdList = new List<long>(); // 当前选中的副本ID列表
        private Action<GuildDungeonInfo> _m_onItemClick;
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonAutoOpenGrid(GGUIMonoGuildDungeonAutoOpenGrid gridMono, Action<GuildDungeonInfo> _onItemClick) : base(gridMono)
        {
            _m_onItemClick = _onItemClick;
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

        protected override void _onRefreshItemWnd(GGUIWndGuildDungeonAutoOpenGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            GuildDungeonInfo data = _m_itemDataList[_itemIdx];
            
            _itemWnd?.setInfo(data, _m_curSelectDungeonIdList.Contains(data.dungeonId));

        }

        protected override GGUIWndGuildDungeonAutoOpenGridItem _createItemWnd(GGUIMonoGuildDungeonAutoOpenGridItem _itemMono)
        {
            GGUIWndGuildDungeonAutoOpenGridItem itemWnd = new GGUIWndGuildDungeonAutoOpenGridItem(_itemMono, _m_onItemClick);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<GuildDungeonInfo> _itemDataList, List<long> _curSelectDungeonIdList)
        {
            _m_itemDataList.Clear();
            if (_itemDataList != null) 
                _m_itemDataList.AddRange(_itemDataList);

            _m_curSelectDungeonIdList.Clear();
            if (_curSelectDungeonIdList != null) 
                _m_curSelectDungeonIdList.AddRange(_curSelectDungeonIdList);
            setItemCount(_m_itemDataList.Count);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
