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
    public class GGUIWndHeroCommonSelectItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoHeroCommonSelectItem,GGUIMonoHeroCommonSelectItemGrid,GGUIWndHeroCommonSelectItem>
    {
        private readonly Action<_IHeroCardShow> _m_onHeroSelect;

        private List<_IHeroCardShow> _m_curSelectHeroList;
        private List<_IHeroCardShow> _m_itemDataList;
        
        public GGUIWndHeroCommonSelectItemGrid(GGUIMonoHeroCommonSelectItemGrid gridMono, Action<_IHeroCardShow> _onHeroSelect) : base(gridMono)
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

        protected override void _onRefreshItemWnd(GGUIWndHeroCommonSelectItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _m_itemDataList == null ||  _itemIdx >= _m_itemDataList.Count)
                return;
            _IHeroCardShow heroInfo = _m_itemDataList[_itemIdx];
            int index = 0;
            if (_m_curSelectHeroList != null && heroInfo != null)
            {
                for (var i = 0; i < _m_curSelectHeroList.Count; i++)
                {
                    _IHeroCardShow selectedHero = _m_curSelectHeroList[i];
                    if (selectedHero == null || selectedHero.id != heroInfo.id) continue;
                    index = i + 1;
                    break;
                }
            }
            _itemMono?.refreshWnd(heroInfo, index);

        }

        protected override GGUIWndHeroCommonSelectItem _createItemWnd(GGUIMonoHeroCommonSelectItem _itemMono)
        {
            GGUIWndHeroCommonSelectItem itemWnd = new GGUIWndHeroCommonSelectItem(_itemMono, _onItemClick);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void refreshWnd(List<_IHeroCardShow> _showHeroList, List<_IHeroCardShow> _curSelectHeroList)
        {
            if(_showHeroList == null)
                return;
            _m_curSelectHeroList = _curSelectHeroList;
            _m_itemDataList = _showHeroList;
            setItemCount(_m_itemDataList.Count);
        }
        
        /// <summary>
        /// 添加一个选中大臣
        /// </summary>
        /// <param name="_heroInfo"></param>
        public void addSelectHero(_IHeroCardShow _heroInfo)
        {
            if (_heroInfo == null || _m_itemDataList == null)
                return;
            
            int selectHeroIdx = _m_itemDataList.IndexOf(_heroInfo);
            if(selectHeroIdx < 0 || selectHeroIdx >= _m_itemDataList.Count)
                return;
            
            if(_m_curSelectHeroList == null)
                _m_curSelectHeroList = new List<_IHeroCardShow>();
            _m_curSelectHeroList.Add(_heroInfo);
            
            forceRefreshItem(selectHeroIdx);
        }
        
        /// <summary>
        /// 移除一个选中大臣
        /// </summary>
        /// <param name="_heroInfo"></param>
        public void removeSelectHero(_IHeroCardShow _heroInfo)
        {
            _m_curSelectHeroList?.Remove(_heroInfo);
            if (_heroInfo == null || _m_itemDataList == null)
                return;
            
            int selectHeroIdx = _m_itemDataList.IndexOf(_heroInfo);
            if(selectHeroIdx < 0 || selectHeroIdx >= _m_itemDataList.Count)
                return;
            
            forceRefreshItem(selectHeroIdx);
        }
        
        private void _onItemClick(_IHeroCardShow _heroInfo)
        {
            _m_onHeroSelect?.Invoke(_heroInfo);
        }
    }
}
