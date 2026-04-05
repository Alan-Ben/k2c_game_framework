using ALPackage;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 可合成道具列表
    /// </summary>
    public class GGUIWndBagConvertItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoBagGridItem, GGUIMonoBagConvertItemGrid, GGUIWndBagGridItem>
    {
        // 物品列表
        private List<BagItemShowInfo> _m_lItemList;

        public GGUIWndBagConvertItemGrid(GGUIMonoBagConvertItemGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            //窗口记录红点已读跟着窗口隐藏一起设置为已读
            _setReadWndRedTip();
        }
        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        // 创建对象
        protected override GGUIWndBagGridItem _createItemWnd(GGUIMonoBagGridItem _itemMono)
        {
            // 创建对象
            GGUIWndBagGridItem gridItem = new GGUIWndBagGridItem(_itemMono);
            gridItem.setSelectDelegate(_onSelectIndex);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndBagGridItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_lItemList.Count)
                return;

            // 刷新物品UI
            _itemMono.refreshItem(_m_lItemList[_itemIdx], false);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if(_m_lItemList == null)
                _m_lItemList = new List<BagItemShowInfo>();
            _m_lItemList.Clear();

            //获取数据
            List<BagItemRefObj> bagItemRefList = new List<BagItemRefObj>();
            List<ItemConvertRefObj> itemConvertRefList = new List<ItemConvertRefObj>();
            itemConvertRefList.AddRange(GRefdataCoreMgr.instance.itemConvertCore.refList);
            //根据排序id从小到大排序
            itemConvertRefList.Sort((_a,_b)=>_a.sort_id.CompareTo(_b.sort_id));
            //获取背包道具类型
            for (int i = 0; i < itemConvertRefList.Count; i++)
            {
                if (itemConvertRefList[i] != null && itemConvertRefList[i].target_item != null && itemConvertRefList[i].target_item.itemType == ENPItemType.BAG_ITEM)
                {
                    BagItemRefObj bagItemRef = GRefdataCoreMgr.instance.bagItemCore.getRef(itemConvertRefList[i].target_item.itemId);
                    bagItemRefList.Add(bagItemRef);
                }
            }
            for (int i = 0; i < bagItemRefList.Count; i++)
            {
                if (bagItemRefList[i] == null)
                    return;

                BagItem bagItem = NPPlayer.instance.bagComp.getItem(bagItemRefList[i].id);
                CommonItemData itemData = new CommonItemData(ENPItemType.BAG_ITEM, bagItemRefList[i].id, bagItem != null ? bagItem.count : 0);
                _m_lItemList.Add(new BagItemShowInfo(bagItem, itemData, bagItemRefList[i]));
            }

            // 设置已查看新物品
            _setReadNewBagItem();

            // 空物品提示
            if (null != wnd.noneItemsTips)
                ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lItemList.Count <= 0 ? 1 : 0);

            // 刷新grid
            setItemCount(_m_lItemList.Count);
        }

        /// <summary>
        /// 不移除物品 数量置为0
        /// </summary>
        /// <param name="_removeItem"></param>
        public void removeItem(BagItem _removeItem)
        {
            // 修改物品状态
            int idx = _getItemIdx(_removeItem);

            if (idx >= 0 && idx < _m_lItemList.Count)
            {
                BagItemShowInfo info = _m_lItemList[idx];
                if (null != info)
                    info.setCount(0);
                // 刷新物品
                forceRefreshItem(idx);
            }
        }

        /// <summary>
        /// 刷新物品
        /// </summary>
        /// <param name="_item"></param>
        public void updateItem(BagItem _item)
        {
            // 刷新物品
            int idx = _getItemIdx(_item);

            if (idx >= 0 && idx < _m_lItemList.Count)
            {
                BagItemShowInfo info = _m_lItemList[idx];
                if (null != info)
                    info.setBagItem(_item);
                //设置查看过新物品
                _setReadNewBagItem();
                forceRefreshItem(idx);
            }
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="_item"></param>
        public void addItem(BagItem _item)
        {
            // 刷新物品
            int idx = _getItemIdx(_item);

            if (idx >= 0 && idx < _m_lItemList.Count)
            {
                BagItemShowInfo info = _m_lItemList[idx];
                if (null != info)
                    info.setBagItem(_item);
                //设置查看过新物品
                _setReadNewBagItem();
                forceRefreshItem(idx);
            }
        }

        /// <summary>
        /// 刷新合成道具
        /// </summary>
        /// <param name="_itemId"></param>
        public void refreshConvertItem(long _itemId)
        {
            int idx = _getItemIdx(_itemId);

            if (idx >= 0 && idx < _m_lItemList.Count)
            {
                forceRefreshItem(idx);
            }
        }

        /// <summary>
        /// 获取物品下标
        /// </summary>
        private int _getItemIdx(BagItem _item)
        {
            int idx = -1;

            if (null == _item)
                return idx;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                BagItemShowInfo info = _m_lItemList[i];
                if (null == info)
                    continue;
                if (info.bagItemRef.id == _item.itemId)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }
        private int _getItemIdx(long _itemId)
        {
            int idx = -1;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                BagItemShowInfo info = _m_lItemList[i];
                if (null == info)
                    continue;
                if (info.bagItemRef.id == _itemId)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        #region 红点相关
        
        //设置查看过新物品
        private void _setReadNewBagItem()
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if(_m_lItemList[i] != null && (_m_lItemList[i].isNew || _m_lItemList[i].isNewAddItem))
                    NPPlayer.instance.bagComp.setItemIsViewed(_m_lItemList[i].bagItem.itemId);
            }
        }

        //设置窗口记录红点已读
        private void _setReadWndRedTip()
        {
            //设置窗口红点已读
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _setReadWndRedTip(_m_lItemList[i]);
            }
        }

        //设置窗口记录红点已读
        private void _setReadWndRedTip(BagItemShowInfo _info)
        {
            if (_info == null || _info.bagItemRef == null)
                return;

            AccountSettingMgr.instance.bagItemWndRedTipSaver.setReadRedTipByType(_info.bagItemRef.id, EBagItemRedTipType.BE_COMBINE);
        }

        #endregion

        //点击选择item
        private void _onSelectIndex(int _index)
        {
            if (_m_lItemList == null || _m_lItemList.Count <= _index)
                return;

            //设置查看过新物品 需要在点击事件之前，因为点击事件会修改isNew
            if (_index >= 0)
            {
                BagItemShowInfo item = _m_lItemList[_index];
                if (item == null || item.itemData == null)
                    return;

                if (null != item && (item.isNew || item.isNewAddItem))
                {
                    NPPlayer.instance.bagComp.setItemIsViewed(item.bagItem.itemId);
                }

                //设置窗口记录红点已读
                _setReadWndRedTip(item);
                forceRefreshItem(_index);

                //打开合成窗口
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemBeCombined.instance, () =>
                {
                    GGUIWndBagItemBeCombined.instance.showWnd();
                    GGUIWndBagItemBeCombined.instance.init(item.itemData.getItemType(), item.itemData.subId);
                }, UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
            }
        }
    }
}
