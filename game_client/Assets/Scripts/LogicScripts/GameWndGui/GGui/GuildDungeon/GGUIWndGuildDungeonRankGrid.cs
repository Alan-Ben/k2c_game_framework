using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟副本排行容器
    /// </summary>
    public class GGUIWndGuildDungeonRankGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildDungeonRankGridItem,GGUIMonoGuildDungeonRankGrid,GGUIWndGuildDungeonRankGridItem>
    {
        [NotNull] private List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> _m_itemDataList = new List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonRankGrid(GGUIMonoGuildDungeonRankGrid gridMono) : base(gridMono)
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

        protected override void _onRefreshItemWnd(GGUIWndGuildDungeonRankGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx], _itemIdx + 1);

        }

        protected override GGUIWndGuildDungeonRankGridItem _createItemWnd(GGUIMonoGuildDungeonRankGridItem _itemMono)
        {
            GGUIWndGuildDungeonRankGridItem itemWnd = new GGUIWndGuildDungeonRankGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<Common.GuildDungeonObj.GuildDungeon_DamageRankItem> _itemDataList)
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
