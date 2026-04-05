using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 可空大臣头像列表
    /// </summary>
    public class GGUIWndHeroIconNullableItemContainer : _ANPGGUIBasicSubWndControlContainer<GGUIMonoHeroIconNullableItem, GGUIMonoHeroIconNullableItemContainer, GGUIWndHeroIconNullableItem>
    {
        public event Action<GGUIWndHeroIconNullableItem> onItemClick;

        public GGUIWndHeroIconNullableItemContainer(GGUIMonoHeroIconNullableItemContainer _containerMono) : base(_containerMono)
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

        protected override GGUIWndHeroIconNullableItem _createItemWnd(GGUIMonoHeroIconNullableItem _itemMono)
        {
            GGUIWndHeroIconNullableItem itemWnd = new GGUIWndHeroIconNullableItem(_itemMono);
            itemWnd.onItemClick += _onItemClick;

            return itemWnd;
        }

        protected override void _discardItem(GGUIWndHeroIconNullableItem _itemWnd)
        {
            if (_itemWnd != null)
                _itemWnd.onItemClick -= _onItemClick;
        }

        private void _onItemClick(GGUIWndHeroIconNullableItem _itemWnd)
        {
            onItemClick?.Invoke(_itemWnd);
        }

        /// <summary>
        /// 设置窗口显示信息
        /// </summary>
        /// <param name="_infoList"></param>
        public void  setShowList(List<_IHeroCardShow> _infoList, int _showTotalCount)
        {
            showItemList(_showTotalCount, (_index, _wnd) =>
            {
                if (_index < 0 || _index >= _showTotalCount || _wnd == null)
                    return false;

                if(_infoList != null && _index < _infoList.Count)
                    _wnd.setData(_infoList[_index]);
                else
                    _wnd.setData(null);
                
                return true;
            });
        }

        public void setShowList(List<HeroInfo> _infoList, int _showTotalCount)
        {
            showItemList(_showTotalCount, (_index, _wnd) =>
            {
                if (_index < 0 || _index >= _showTotalCount || _wnd == null)
                    return false;

                if(_infoList != null && _index < _infoList.Count)
                    _wnd.setData(_infoList[_index]);
                else
                    _wnd.setData(null);
                
                return true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_heroIdList"></param>
        public void setShowList(List<long> _heroIdList, int _showTotalCount, bool _forceSelfHasHero)
        {
            showItemList(_showTotalCount, (_index, _wnd) =>
            {
                if (_index < 0 || _index >= _showTotalCount || _wnd == null)
                    return false;

                if (_heroIdList != null && _index < _heroIdList.Count)//窗口显示下标在给定数据范围内
                {
                    long heroId = _heroIdList[_index];
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(heroId);
                    if (heroInfo != null)//若是玩家有的大臣
                    {
                        _wnd.setData(heroInfo);
                    }
                    else if(!_forceSelfHasHero)//若不强制必须显示的是玩家有的大臣
                    {
                        HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(heroId);
                        HeroCardShowInfo heroCardShowInfo = new HeroCardShowInfo(null, heroRefObj);
                        _wnd.setData(heroCardShowInfo);       
                    }
                    else
                    {
                        _wnd.setData(null);
                    }
                }
                else
                {
                    _wnd.setData(null);
                }
                
                return true;
            });
        }
    }
}