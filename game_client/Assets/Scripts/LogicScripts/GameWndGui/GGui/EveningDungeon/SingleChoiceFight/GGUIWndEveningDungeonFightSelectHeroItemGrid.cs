using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 晚间活动选择大臣出战item列表
    /// </summary>
    public class GGUIWndEveningDungeonFightSelectHeroItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoEveningDungeonFightSelectHeroItem, GGUIMonoEveningDungeonFightSelectHeroItemGrid, GGUIWndEveningDungeonFightSelectHeroItem>
    {
        //总的卡牌列表
        [NotNull] private List<_IEveningDungeonHeroFightInfo> _m_lHeroFightInfoList = new List<_IEveningDungeonHeroFightInfo>();
        //当前选中的item
        private _IEveningDungeonHeroFightInfo _m_wCurSelectHeroInfo;
        //点击Item回调
        private Action<_IEveningDungeonHeroFightInfo> _m_dClickDelegate;

        /// <summary>
        /// 当前选中的item
        /// </summary>
        public _IEveningDungeonHeroFightInfo curSelectHeroInfo { get { return _m_wCurSelectHeroInfo; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<_IEveningDungeonHeroFightInfo> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        public GGUIWndEveningDungeonFightSelectHeroItemGrid(GGUIMonoEveningDungeonFightSelectHeroItemGrid containerMonoEveningDungeonFightSelect) : base(containerMonoEveningDungeonFightSelect)
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lHeroFightInfoList != null)
                _m_lHeroFightInfoList.Clear();

            _m_dClickDelegate = null;
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override GGUIWndEveningDungeonFightSelectHeroItem _createItemWnd(GGUIMonoEveningDungeonFightSelectHeroItem itemMonoEveningDungeonFightSelect)
        {
            // 创建对象
            GGUIWndEveningDungeonFightSelectHeroItem gridItem = new GGUIWndEveningDungeonFightSelectHeroItem(itemMonoEveningDungeonFightSelect);
            // 注册物品点击事件
            gridItem.clickDelegate += _onClickCardItem;
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndEveningDungeonFightSelectHeroItem _itemMono, int _itemIdx)
        {
            if (_m_lHeroFightInfoList.Count <= _itemIdx)
                return;

            _IEveningDungeonHeroFightInfo eveningDungeonHeroFightInfo = _m_lHeroFightInfoList[_itemIdx];
            _itemMono.setInfo(eveningDungeonHeroFightInfo);
            _itemMono.setSelect(eveningDungeonHeroFightInfo?.heroId == _m_wCurSelectHeroInfo?.heroId);
        }

        /// <summary>
        /// 初始化伙伴列表
        /// </summary>
        /// <param name="_cardList"></param>
        public void showHeroList(List<_IEveningDungeonHeroFightInfo> _cardList)
        {
            if (null == _cardList)
                return;

            //获取的列表已经是排序过的
            _m_lHeroFightInfoList.Clear();
            _m_lHeroFightInfoList.AddRange(_cardList);

            //重置选中item
            _m_wCurSelectHeroInfo = null;

            //刷新卡牌
            _refreshCardList();
            //滚到上边
            scrollMoveToTop();
        }

        /// <summary>
        /// 根据下标选中item
        /// </summary>
        public void setSelectByIndex(int _index, bool _needCallBack = true)
        {
            _m_wCurSelectHeroInfo = _m_lHeroFightInfoList.SafeGet(_index);

            forceRefreshAllItem();

            if (null != _m_dClickDelegate && _needCallBack)
                _m_dClickDelegate(_m_wCurSelectHeroInfo);
        }

        public void setSelectByHeroId(long _heroId, bool _needCallBack = true)
        {
            _m_wCurSelectHeroInfo = null;
            for(int i = 0; i < _m_lHeroFightInfoList.Count; i++)
            {
                _IEveningDungeonHeroFightInfo heroFightInfo = _m_lHeroFightInfoList[i];
                if(heroFightInfo != null && heroFightInfo.heroId == _heroId)
                {
                    _m_wCurSelectHeroInfo = heroFightInfo;
                    break;
                }
            }
         
            forceRefreshAllItem();

            if (null != _m_dClickDelegate && _needCallBack)
                _m_dClickDelegate(_m_wCurSelectHeroInfo);
        }

        /// <summary>
        /// 取消选中大臣
        /// </summary>
        public void setUnSelectHero()
        {
            _m_wCurSelectHeroInfo = null;
            
            forceRefreshAllItem();
        }
        
        // 刷新卡牌列表
        private void _refreshCardList()
        {
            if (wnd == null)
                return;

            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lHeroFightInfoList.Count <= 0 ? 1 : 0);

            //刷新grid
            setItemCount(_m_lHeroFightInfoList.Count);
        }

        //移动到顶部
        public void scrollMoveToTop()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToTop();
            });
        }

        //点击卡牌
        private void _onClickCardItem(GGUIWndEveningDungeonFightSelectHeroItem itemWndEveningDungeonFightSelect)
        {
            if (null == itemWndEveningDungeonFightSelect || itemWndEveningDungeonFightSelect.eveningDungeonHeroFightInfo == null)
                return;

            if (_m_wCurSelectHeroInfo != null && _m_wCurSelectHeroInfo.heroId == itemWndEveningDungeonFightSelect.eveningDungeonHeroFightInfo.heroId)
                _m_wCurSelectHeroInfo = null;
            else
                _m_wCurSelectHeroInfo = itemWndEveningDungeonFightSelect.eveningDungeonHeroFightInfo;
            forceRefreshAllItem();

            if (null != _m_dClickDelegate)
                _m_dClickDelegate(_m_wCurSelectHeroInfo);
        }
    }
}