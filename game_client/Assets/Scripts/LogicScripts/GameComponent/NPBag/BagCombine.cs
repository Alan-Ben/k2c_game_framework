using System.Collections.Generic;
using NPEnum;
using Unity.Mathematics;

namespace GOE
{
    /// <summary>
    /// 合成对象节点
    /// </summary>
    public class BagCombine
    {

        //已经消耗的列表，包含节点顺序，包含自身
        private BagCombine _m_costParent;
        //当前这个物品类型
        private NPEnum.ENPItemType _m_itemType;
        private long _m_ItemId;
        //当前这个物品需要多少个
        private long _m_needCount;
        //玩家自己当前这个物品有多少个
        private long _m_hasCostCount;
        //需要消耗的资源
        private List<NPCommonCostItem> _m_resCostItemList;
        public BagCombine(ENPItemType _itemType, long _itemId, long _needCount, List<NPCommonCostItem> _resCostItemList = null, BagCombine _costParent = null)
        {
            _m_itemType = _itemType;
            _m_ItemId = _itemId;
            _m_needCount = _needCount;
            _m_hasCostCount = GCommon.getItemCount(_itemType, _m_ItemId);
            _m_resCostItemList = _resCostItemList;
            _m_costParent = _costParent;
        }
        public BagCombine(NPCommonCostItem _targetItem, List<NPCommonCostItem> _resCostItemList = null, BagCombine _costParent = null)
        {
            _m_itemType = _targetItem.getItemType();
            _m_ItemId = _targetItem.subId;
            _m_needCount = _targetItem.count;
            _m_hasCostCount = GCommon.getItemCount(_m_itemType, _m_ItemId);
            _m_resCostItemList = _resCostItemList;
            _m_costParent = _costParent;
        }

        public ENPItemType ItemType
        {
            get { return _m_itemType; }
        }

        public long ItemId
        {
            get { return _m_ItemId; }
        }

        public long NeedCount
        {
            get { return _m_needCount; }
        }

        public long HasCostCount
        {
            get { return _m_hasCostCount; }
        }
        public List<NPCommonCostItem> resCostItemList
        {
            get
            {
                return _m_resCostItemList;
            }
        }
        public BagCombine CostParent
        {
            get { return _m_costParent; }
        }

        //是否可以满足合成条件了
        public bool canCombine()
        {
            return _m_hasCostCount >= _m_needCount && GCommon.isItemEnough(_m_resCostItemList, false);
        }

        //获取子对象合成需求列表
        public List<BagCombine> getSubCombineList()
        {
            List<BagCombine> bagCombines = new List<BagCombine>();
            foreach (ItemConvertRefObj npsoItemConvertRefObj in GRefdataCoreMgr.instance.itemConvertCore.refList)
            {
                if (null == npsoItemConvertRefObj)
                    continue;

                if (npsoItemConvertRefObj.target_item.itemType == _m_itemType && npsoItemConvertRefObj.target_item.itemId == _m_ItemId)
                {
                    long realCount = _m_needCount - _m_hasCostCount;
                    if (realCount < 0)
                        realCount = 0;
                    List<NPCommonCostItem> list = new List<NPCommonCostItem>();
                    NPCommonCostItem temp = null;
                    for (int i = 0; i < npsoItemConvertRefObj.cost_item_list.Count; i++)
                    {
                        temp = npsoItemConvertRefObj.cost_item_list[i];
                        if (null == temp)
                            continue;

                        NPCommonCostItem item = new NPCommonCostItem(temp.item, realCount * temp.count);
                        list.Add(item);
                    }
                    bagCombines.Add(new BagCombine(
                        ENPItemType.BAG_ITEM,
                        npsoItemConvertRefObj._refId,
                        npsoItemConvertRefObj.ori_item_num * realCount,
                        list, this));
                }
            }

            return bagCombines;
        }

         //获取最终结果材料消耗列表
        public BagOnceCombineItem getCombineFinalCostList()
        {
            //总的原材料消耗
            List<NPCommonCostItem> commonCostItems = new List<NPCommonCostItem>();
            //总的资源消耗
            List<NPCommonCostItem>resCostItems = new List<NPCommonCostItem>();
            BagCombine _combineNode = this;
            while (_combineNode.CostParent != null)
            {
                long count = math.min(_combineNode.NeedCount, _combineNode.HasCostCount);
                if (count > 0)
                {
                    commonCostItems.Insert(0, new NPCommonCostItem(_combineNode.ItemType, _combineNode.ItemId, count));
                    if (null != _combineNode.resCostItemList)
                        resCostItems.InsertRange(0,_combineNode.resCostItemList);
                }
                _combineNode = _combineNode.CostParent;
            }

            BagOnceCombineItem combineItem = new BagOnceCombineItem();
            long targetCount = _combineNode.NeedCount - _combineNode.HasCostCount;
            if (targetCount < 0)
                targetCount = 0;

            NPCommonCostItem targetItem = new NPCommonCostItem(_combineNode.ItemType, _combineNode.ItemId, targetCount);
            combineItem.setTargetItem(targetItem);
            combineItem.setOriCostItemList(commonCostItems);
            combineItem.setResCostItemList(resCostItems);
            return combineItem;
        }
    }
}