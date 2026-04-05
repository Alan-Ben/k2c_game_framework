using ALPackage;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 藏品列表
    /// </summary>
    public class GGUIWndEquipMainListGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoEquipMainListGridItem, GGUIMonoEquipMainListGrid, GGUIWndEquipMainListGridItem>
    {
        //总的卡牌列表
        [NotNull] private List<_IEquipCardShow> _m_lCardList = new List<_IEquipCardShow>();
        //页签类型
        private EEquipMainTabType _m_eTabType;
        //点击Item回调
        private Action<EEquipMainTabType, _IEquipCardShow> _m_dClickDelegate;
        //标题栏控制器列表
        private List<GGUIWndEquipMainListGridBarController> _m_lBarControllerList;


        public Action<EEquipMainTabType, _IEquipCardShow> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        public GGUIWndEquipMainListGrid(GGUIMonoEquipMainListGrid _containerMono) : base(_containerMono)
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
            if (_m_lCardList != null)
                _m_lCardList.Clear();

            _m_dClickDelegate = null;

            if (_m_lBarControllerList != null)
            {
                for (int i = 0; i < _m_lBarControllerList.Count; i++)
                {
                    if (_m_lBarControllerList[i] != null)
                    {
                        removeBar(_m_lBarControllerList[i]);
                        _m_lBarControllerList[i].discard();
                    }
                }
                _m_lBarControllerList.Clear();
                _m_lBarControllerList = null;
            }
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override GGUIWndEquipMainListGridItem _createItemWnd(GGUIMonoEquipMainListGridItem _itemMono)
        {
            // 创建对象
            GGUIWndEquipMainListGridItem gridItem = new GGUIWndEquipMainListGridItem(_itemMono);
            // 注册物品点击事件
            gridItem.clickDelegate += _onClickCardItem;
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndEquipMainListGridItem _itemMono, int _itemIdx)
        {
            if (_m_lCardList.Count <= _itemIdx)
                return;

            _IEquipCardShow heroShowInfo = _m_lCardList[_itemIdx];
            _itemMono.setInfo(_m_eTabType, heroShowInfo);
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="_cardList"></param>
        public void setInfo(EEquipMainTabType _tabType, List<_IEquipCardShow> _cardList)
        {
            if (null == _cardList)
                return;

            _m_eTabType = _tabType;
            _m_lCardList.Clear();
            _m_lCardList.AddRange(_cardList);
            //刷新卡牌
            _refreshCardList();
            //刷新标题栏
            _showListBar();
            //滚到上边
            scrollMoveToTop();
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

        private void _showListBar()
        {
            if (_m_lCardList.Count <= 0 || _m_eTabType != EEquipMainTabType.ILLUSTRATED_HANDBOOK)
                return;

            //先清空bar
            if (_m_lBarControllerList != null)
            {
                for (int i = 0; i < _m_lBarControllerList.Count; i++)
                {
                    if (_m_lBarControllerList[i] != null)
                    {
                        removeBar(_m_lBarControllerList[i]);
                        _m_lBarControllerList[i].discard();
                    }
                }
                _m_lBarControllerList.Clear();
            }

            //bar需要插入的位置
            List<int> barIndexList = new List<int>();
            //bar需要的品质列表
            List<EQuality> qualityList = new List<EQuality>();

            EQuality lastQuality = EQuality.NONE;
            for (int i = 0; i < _m_lCardList.Count; i++)
            {
                if (_m_lCardList[i] == null)
                    continue;

                EQuality curQuality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_lCardList[i].equipRef.id);
                if (lastQuality != curQuality)
                {
                    barIndexList.Add(i);
                    qualityList.Add(curQuality);
                    lastQuality = curQuality;
                }
            }

            if (_m_lBarControllerList == null)
                _m_lBarControllerList = new List<GGUIWndEquipMainListGridBarController>();

            for (int i = 0; i < qualityList.Count; i++)
            {
                EQuality quality = qualityList[i];
                int barIndex = barIndexList[i];
                GGUIWndEquipMainListGridBarController barController = new GGUIWndEquipMainListGridBarController(wnd.gridAreaUIObj);
                addBar(barController);
                barController.regLoadDoneDelegate(() =>
                {
                    barController.setInsertIndex(barIndex);
                    barController.setInfo(quality);
                    forceRefreshBar();
                });
                _m_lBarControllerList.Add(barController);
            }
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
        private void _onClickCardItem(GGUIWndEquipMainListGridItem _itemWnd)
        {
            if (null == _itemWnd)
                return;

            if (null != _m_dClickDelegate)
                _m_dClickDelegate(_itemWnd.tabType, _itemWnd.showInfo);
        }

        //滚动到指定位置
        public void scrollMoveToTarget(float _pos)
        {
            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToVerticalRate(_pos);
            });
        }

        //获取当前位置
        public float getScrollCurPos()
        {
            if (wnd == null || wnd.scrollRect == null)
                return 0;
            return 1 - wnd.scrollRect.verticalNormalizedPosition;
        }
    }
}
