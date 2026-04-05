using System;
using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 藏品列表
    /// </summary>
    public class GGUIWndEquipRecycleSelectGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoEquipRecycleSelectGridItem, GGUIMonoEquipRecycleSelectGrid, GGUIWndEquipRecycleSelectGridItem>
    {
        //可分解藏品信息列表
        [NotNull] private List<EquipInfo> _m_lInfoList = new List<EquipInfo>();
        //已选中的藏品数据id列表
        [NotNull] private List<EquipInfo> _m_lAlreadySelectList = new List<EquipInfo>();
        //点击Item回调
        private Action<GGUIWndEquipRecycleSelectGridItem> _m_dClickDelegate;

        /// <summary>
        /// 已选中的藏品列表
        /// </summary>
        [NotNull]  public List<EquipInfo> alreadySelectList { get { return _m_lAlreadySelectList; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndEquipRecycleSelectGridItem> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        public GGUIWndEquipRecycleSelectGrid(GGUIMonoEquipRecycleSelectGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
            _m_lInfoList.Clear();
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
        protected override GGUIWndEquipRecycleSelectGridItem _createItemWnd(GGUIMonoEquipRecycleSelectGridItem _itemMono)
        {
            // 创建对象
            GGUIWndEquipRecycleSelectGridItem gridItem = new GGUIWndEquipRecycleSelectGridItem(_itemMono);
            // 注册物品点击事件
            gridItem.clickDelegate += _onClickCardItem;
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndEquipRecycleSelectGridItem _itemMono, int _itemIdx)
        {
            if (_m_lInfoList.Count <= _itemIdx)
                return;

            EquipInfo equipInfo = _m_lInfoList[_itemIdx];
            bool isSelect = equipInfo != null && _m_lAlreadySelectList.Contains(equipInfo);
            _itemMono.setInfo(_m_lInfoList[_itemIdx]);
            _itemMono.setSelect(isSelect);
        }

        /// <summary>
        /// 初始化伙伴列表
        /// </summary>
        /// <param name="_cardList"></param>
        public void setInfo(List<EquipInfo> _cardList)
        {
            if (null == _cardList)
                return;

            _m_lAlreadySelectList.Clear();

            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_cardList);
            //刷新列表
            _refreshCardList();
            //滚到上边
            scrollMoveToTop();
        }

        /// <summary>
        /// 按照类型选中所有目标藏品
        /// </summary>
        /// <param name="_type"></param>
        public void setSelectAllByType(EEquipRecycleBatchSelectTabType _type)
        {
            _m_lAlreadySelectList.Clear();
            EQuality targetQuality = EQuality.NONE;
            switch (_type)
            {
                case EEquipRecycleBatchSelectTabType.NONE:
                case EEquipRecycleBatchSelectTabType.GREEN_AND_BELOW://绿色及以下
                    targetQuality = EQuality.GREEN;
                    break;
                case EEquipRecycleBatchSelectTabType.BLUE_AND_BELOW://蓝色及以下
                    targetQuality = EQuality.BLUE;
                    break;
                case EEquipRecycleBatchSelectTabType.PURPLE_AND_BELOW://紫色及以下
                    targetQuality = EQuality.PURPLE;
                    break;
                case EEquipRecycleBatchSelectTabType.ORANGE_AND_BELOW://橙色及以下
                    targetQuality = EQuality.ORANGE;
                    break;
            }

            for (int i = 0; i < _m_lInfoList.Count; i++)
            {
                EquipInfo equipInfo = _m_lInfoList[i];
                if (equipInfo == null)
                    continue;

                EQuality curQuality = GCommon.getItemQuality(ENPItemType.EQUIP, equipInfo.equipId);
                if ((long) curQuality <= (long) targetQuality)
                    _m_lAlreadySelectList.Add(equipInfo);
            }

            forceRefreshAllItem();
        }

        /// <summary>
        /// 设置点击item
        /// </summary>
        /// <param name="_targetIndex"></param>
        public void setClickItemByIndex(int _targetIndex)
        {
            refreshAllItem((_item, _index) =>
            {
                if(_targetIndex == _index)
                    _item?.setClickItem();
            });
        }

        // 刷新卡牌列表
        private void _refreshCardList()
        {
            if (wnd == null)
                return;

            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lInfoList.Count <= 0 ? 1 : 0);

            //刷新grid
            setItemCount(_m_lInfoList.Count);
        }

        //移动到顶部
        public void scrollMoveToTop()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToTop();
            });
        }

        //点击藏品
        private void _onClickCardItem(GGUIWndEquipRecycleSelectGridItem _itemWnd)
        {
            if (null == _itemWnd || _itemWnd.showInfo == null)
                return;

            //根据是否选中从列表添加移除
            if(_itemWnd.isSelect && !_m_lAlreadySelectList.Contains(_itemWnd.showInfo))
                _m_lAlreadySelectList.Add(_itemWnd.showInfo);
            else if (!_itemWnd.isSelect && _m_lAlreadySelectList.Contains(_itemWnd.showInfo))
                _m_lAlreadySelectList.Remove(_itemWnd.showInfo);

            if (null != _m_dClickDelegate)
                _m_dClickDelegate(_itemWnd);
        }
    }
}
