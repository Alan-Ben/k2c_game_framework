using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndMiddayDungeonHeroSelectItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMiddayDungeonHeroSelectItem,GGUIMonoMiddayDungeonHeroSelectItemGrid,GGUIWndMiddayDungeonHeroSelectItem>
    {
        private readonly Action<_IHeroCardShow> _m_onHeroSelect;

        private EMiddayDungeonSelectHeroType _m_selectHeroType;
        private _IHeroCardShow _m_curSelectHero;
        private List<_IHeroCardShow> _m_itemDataList;
        
        public GGUIWndMiddayDungeonHeroSelectItemGrid(GGUIMonoMiddayDungeonHeroSelectItemGrid gridMono, Action<_IHeroCardShow> _onHeroSelect) : base(gridMono)
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
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndMiddayDungeonHeroSelectItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _m_itemDataList == null ||  _itemIdx >= _m_itemDataList.Count)
                return;
            _IHeroCardShow heroInfo = _m_itemDataList[_itemIdx];
            int index = 0;
            switch (_m_selectHeroType)
            {
                case EMiddayDungeonSelectHeroType.Self:
                    if (_m_curSelectHero != null && heroInfo != null)
                    {
                        if (_m_curSelectHero.id == heroInfo.id)
                            index = 1;
                    }
                    break;
                case EMiddayDungeonSelectHeroType.Guild:
                    if (_m_curSelectHero != null && heroInfo != null)
                    {
                        if (heroInfo is MiddayDungeonGuildHeroInfo guildHeroInfo && _m_curSelectHero is MiddayDungeonGuildHeroInfo selectGuildHero)
                            if (selectGuildHero.cid == guildHeroInfo.cid)
                                index = 1;
                    }
                    break;
            }
            _itemMono?.refreshWnd(heroInfo, index, _m_selectHeroType);

        }

        protected override GGUIWndMiddayDungeonHeroSelectItem _createItemWnd(GGUIMonoMiddayDungeonHeroSelectItem _itemMono)
        {
            GGUIWndMiddayDungeonHeroSelectItem itemWnd = new GGUIWndMiddayDungeonHeroSelectItem(_itemMono, _onItemClick);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void refreshWnd(List<_IHeroCardShow> _showHeroList, _IHeroCardShow _curSelectHeroList, EMiddayDungeonSelectHeroType _selectHeroType)
        {
            _m_selectHeroType = _selectHeroType;
            _m_curSelectHero = _curSelectHeroList;
            _m_itemDataList = _showHeroList;

            int itemCount = _m_itemDataList == null ? 0 : _m_itemDataList.Count;
            setItemCount(itemCount);

            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, itemCount <= 0);
        }
        
        
        private void _onItemClick(_IHeroCardShow _heroInfo)
        {
            _m_onHeroSelect?.Invoke(_heroInfo);
        }
    }
}
