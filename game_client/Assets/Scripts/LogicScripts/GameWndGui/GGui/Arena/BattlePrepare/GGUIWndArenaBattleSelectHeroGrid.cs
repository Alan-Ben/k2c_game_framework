using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 竞技场选择伙伴列表
    /// </summary>
    public class GGUIWndArenaBattleSelectHeroGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoArenaBattleSelectHeroGridItem, GGUIMonoArenaBattleSelectHeroGrid, GGUIWndArenaBattleSelectHeroGridItem>
    {
        //总的卡牌列表
        [NotNull] private List<HeroInfo> _m_lCardList = new List<HeroInfo>();
        //是否是指定谈判
        private bool _m_bIsSelectAttack;
        //当前选中的item
        private HeroInfo _m_wCurSelectHeroInfo;
        //点击Item回调
        private Action<HeroInfo> _m_dClickDelegate;

        /// <summary>
        /// 当前选中的item
        /// </summary>
        public HeroInfo curSelectHeroInfo { get { return _m_wCurSelectHeroInfo; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<HeroInfo> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        public GGUIWndArenaBattleSelectHeroGrid(GGUIMonoArenaBattleSelectHeroGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lCardList != null)
                _m_lCardList.Clear();

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
        protected override GGUIWndArenaBattleSelectHeroGridItem _createItemWnd(GGUIMonoArenaBattleSelectHeroGridItem _itemMono)
        {
            // 创建对象
            GGUIWndArenaBattleSelectHeroGridItem gridItem = new GGUIWndArenaBattleSelectHeroGridItem(_itemMono);
            // 注册物品点击事件
            gridItem.clickDelegate += _onClickCardItem;
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndArenaBattleSelectHeroGridItem _itemMono, int _itemIdx)
        {
            if (_m_lCardList.Count <= _itemIdx)
                return;

            HeroInfo heroShowInfo = _m_lCardList[_itemIdx];
            _itemMono.setInfo(heroShowInfo, _m_bIsSelectAttack);
            _itemMono.setSelect(heroShowInfo?.id == _m_wCurSelectHeroInfo?.id);
        }

        /// <summary>
        /// 初始化伙伴列表
        /// </summary>
        /// <param name="_cardList"></param>
        public void showHeroList(List<HeroInfo> _cardList, bool _isSelectAttack)
        {
            if (null == _cardList)
                return;

            //获取的列表已经是排序过的
            _m_lCardList.Clear();
            _m_lCardList.AddRange(_cardList);
            _m_bIsSelectAttack = _isSelectAttack;

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
        public void setSelectByIndex(int _index)
        {
            if (_m_lCardList.Count <= _index || _index < 0)
                return;

            _m_wCurSelectHeroInfo = _m_lCardList[_index];

            forceRefreshAllItem();

            if (null != _m_dClickDelegate)
                _m_dClickDelegate(_m_wCurSelectHeroInfo);
        }

        // 刷新卡牌列表
        private void _refreshCardList()
        {
            if (wnd == null)
                return;

            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lCardList.Count <= 0 ? 1 : 0);

            //刷新grid
            setItemCount(_m_lCardList.Count);
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
        private void _onClickCardItem(GGUIWndArenaBattleSelectHeroGridItem _itemWnd)
        {
            if (null == _itemWnd || _itemWnd.heroInfo == null)
                return;

            if (_m_wCurSelectHeroInfo != null && _m_wCurSelectHeroInfo.id == _itemWnd.heroInfo.id)
                _m_wCurSelectHeroInfo = null;
            else
                _m_wCurSelectHeroInfo = _itemWnd.heroInfo;
            forceRefreshAllItem();

            if (null != _m_dClickDelegate)
                _m_dClickDelegate(_m_wCurSelectHeroInfo);
        }
    }
}
