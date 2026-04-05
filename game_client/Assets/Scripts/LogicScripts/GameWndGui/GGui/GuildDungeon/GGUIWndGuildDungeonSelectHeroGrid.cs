using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟副本日志容器
    /// </summary>
    public class GGUIWndGuildDungeonSelectHeroGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoGuildDungeonSelectHeroGridItem,GGUIMonoGuildDungeonSelectHeroGrid,GGUIWndGuildDungeonSelectHeroGridItem>
    {
        private readonly Action<HeroInfo> _m_onHeroSelect;
        private List<HeroInfo> _m_itemDataList = new List<HeroInfo>();

        private _IHeroCardShow _m_curSelectHero;        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonSelectHeroGrid(GGUIMonoGuildDungeonSelectHeroGrid gridMono, Action<HeroInfo> _onHeroSelect) : base(gridMono)
        {
            _m_onHeroSelect = _onHeroSelect;
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
            _m_curSelectHero = null;
            _m_itemDataList.Clear();
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }


        protected override void _onRefreshItemWnd(GGUIWndGuildDungeonSelectHeroGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            HeroInfo itemData = _m_itemDataList[_itemIdx];
            _itemWnd?.setInfo(itemData, itemData != null && _m_curSelectHero != null && _m_curSelectHero.id == itemData.id);

        }

        protected override GGUIWndGuildDungeonSelectHeroGridItem _createItemWnd(GGUIMonoGuildDungeonSelectHeroGridItem _itemMono)
        {
            GGUIWndGuildDungeonSelectHeroGridItem itemWnd = new GGUIWndGuildDungeonSelectHeroGridItem(_itemMono, _m_onHeroSelect);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<HeroInfo> _itemDataList, HeroInfo _curSelectHero)
        {
            _m_curSelectHero = _curSelectHero;
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
