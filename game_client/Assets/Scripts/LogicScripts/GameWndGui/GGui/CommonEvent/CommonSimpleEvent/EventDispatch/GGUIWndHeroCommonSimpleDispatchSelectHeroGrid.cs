using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndHeroCommonSimpleDispatchSelectHeroGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoHeroCommonSimpleDispatchSelectHeroItem, GGUIMonoHeroCommonSimpleDispatchSelectHeroGrid, GGUIWndHeroCommonSimpleDispatchSelectHeroItem>
    {
        private CommonSimpleDispatchEventAgent _m_dispatchEventAgent;

        private List<HeroSatisfyConditionCount> _m_lShowInfoList;//显示数据列表
        private List<long> _m_lSelectedIdList;//选中的大臣id列表

        public event Action<GGUIWndHeroCommonSimpleDispatchSelectHeroItem> onItemClick;
        
        public GGUIWndHeroCommonSimpleDispatchSelectHeroGrid(GGUIMonoHeroCommonSimpleDispatchSelectHeroGrid _wnd) : base(_wnd)
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
            onItemClick = default;
        }

        protected override void _onWndInitDone()
        {
        }

        public void setShowInfoList(CommonSimpleDispatchEventAgent _dispatchEventAgent, List<HeroSatisfyConditionCount> _infoList, List<_IHeroCardShow> _selectInfoList = null)
        {
            _m_dispatchEventAgent = _dispatchEventAgent;
            _m_lShowInfoList = _infoList;
            if (_m_lSelectedIdList == null)
                _m_lSelectedIdList = new List<long>();
            _m_lSelectedIdList.Clear();
            foreach (_IHeroCardShow item in _selectInfoList)
            {
                if(item == null || item.heroRefObj == null)
                    continue;

                _IHeroCardShow heroShowInfo = _findHeroShowInfo(item.heroRefObj.id, out int _inShowIndex);//在显示的大臣数据列表中查找是否存在选中大臣
                if(heroShowInfo != null)
                    _m_lSelectedIdList.Add(item.heroRefObj.id);
            }
            
            if (_m_lShowInfoList == null || _m_lShowInfoList.Count <= 0)
            {
                setItemCount(0);
                if(wnd != null)
                    ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, true);
                
                return;
            }
         
            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, false);
            
            setItemCount(_m_lShowInfoList.Count);
        }

        /// <summary>
        /// 查找大臣显示数据
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_info"></param>
        /// <returns></returns>
        private _IHeroCardShow _findHeroShowInfo(long _heroId, out int _index)
        {
            _index = -1;
            if (_m_lShowInfoList == null)
                return null;

            _IHeroCardShow item = null;
            for (int i = 0; i < _m_lShowInfoList.Count; i++)
            {
                item = _m_lShowInfoList[i].heroShowInfo;
                if(item != null && item.heroRefObj != null && item.heroRefObj.id == _heroId)
                {
                    _index = i;
                    return item;
                }
            }
            
            return null;
        }

        /// <summary>
        /// 设置大臣选中
        /// </summary>
        /// <param name="_info">大臣信息</param>
        /// <param name="_selected">是否选中</param>
        public void setHeroSelect(_IHeroCardShow _info, bool _selected)
        {
            if (_info == null || _info.heroRefObj == null)
                return;

            long heroId = _info.heroRefObj.id;
            // 在展示数据中没有找到, 直接返回
            if (_findHeroShowInfo(heroId, out int inShowListIndex) == null)
                return;

            if (_m_lSelectedIdList == null)
                _m_lSelectedIdList = new List<long>();
            
            bool hasSelected = _m_lSelectedIdList.Contains(heroId);//判断大臣是否已经选中过
            if ((hasSelected && _selected) || (!hasSelected && !_selected))//若 (之前已经选择过 且 当前要选择它) 或 (之前没有选择过 且 当前要取消选中) 直接返回
                return;

            if (_selected)//若要变成选中状态
            {
                _m_lSelectedIdList.Add(heroId);
            }
            else//若要取消选中
            {
                _m_lSelectedIdList.Remove(heroId);
            }
            
            forceRefreshItem(inShowListIndex);//刷新item显示
        }

        /// <summary>
        /// 变化大臣的选中状态
        /// </summary>
        public void chgHeroSelectedState(_IHeroCardShow _info)
        {
            if (_info == null || _info.heroRefObj == null)
                return;

            long heroId = _info.heroRefObj.id;
            
            // 在展示数据中没有找到, 直接返回
            if (_findHeroShowInfo(heroId, out int inShowListIndex) == null)
                return;

            if (_m_lSelectedIdList == null)
                _m_lSelectedIdList = new List<long>();
            bool hasSelected = _m_lSelectedIdList.Contains(heroId);
            
            if (hasSelected)//若之前选中过
            {
                _m_lSelectedIdList.Remove(heroId);
            }
            else//若之前没有选中过
            {
                _m_lSelectedIdList.Add(heroId);
            }
            
            forceRefreshItem(inShowListIndex);//刷新item显示
        }
        
        /// <summary>
        /// 是否是相同大臣信息
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private bool _isSameHeroCardInfo(_IHeroCardShow _a, _IHeroCardShow _b)
        {
            if (ReferenceEquals(_a, _b))
                return true;

            if (_a == null || _b == null)
                return false;

            if (_a.heroRefObj != null && _b.heroRefObj != null && _a.heroRefObj.id == _b.heroRefObj.id)
                return true;

            return false;
        }
        
        protected override GGUIWndHeroCommonSimpleDispatchSelectHeroItem _createItemWnd(GGUIMonoHeroCommonSimpleDispatchSelectHeroItem _itemMono)
        {
            GGUIWndHeroCommonSimpleDispatchSelectHeroItem itemWnd = new GGUIWndHeroCommonSimpleDispatchSelectHeroItem(_itemMono);
            itemWnd.ClickAction += _onClickItem;

            return itemWnd;
        }

        protected override void _onRefreshItemWnd(GGUIWndHeroCommonSimpleDispatchSelectHeroItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null || _m_lShowInfoList == null || _itemIdx < 0 || _itemIdx >= _m_lShowInfoList.Count)
                return;

            
            HeroSatisfyConditionCount heroSatisfyConditionCount = _m_lShowInfoList[_itemIdx];
            _IHeroCardShow showInfo = _m_lShowInfoList[_itemIdx].heroShowInfo;
            if (showInfo == null || showInfo.heroRefObj == null)
                return;
            
            _itemMono.showWnd();
            _itemMono.setInfo(_m_dispatchEventAgent, heroSatisfyConditionCount);
            
            if(_m_lSelectedIdList?.Contains(showInfo.heroRefObj.id) ?? false)//在选中大臣数据列表中查找是否存在该大臣
                _itemMono.setState(ECommonSelectState.SELECTED);
            else
                _itemMono.setState(ECommonSelectState.UNSELECTED);
        }

        /// <summary>
        /// 点击某一item时
        /// </summary>
        /// <param name="_item"></param>
        private void _onClickItem(GGUIWndHeroCommonSimpleDispatchSelectHeroItem _item)
        {
            onItemClick?.Invoke(_item);
        }
    }
}