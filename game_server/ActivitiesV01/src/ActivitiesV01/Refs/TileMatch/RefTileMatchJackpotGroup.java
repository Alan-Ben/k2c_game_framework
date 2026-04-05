package ActivitiesV01.Refs.TileMatch;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "tilematch_jackpot_group")
public class RefTileMatchJackpotGroup extends RefBase
{
    private static RefTileMatchJackpotGroupMgr _g_mgr = new RefTileMatchJackpotGroupMgr();

    public static RefTileMatchJackpotGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTileMatchJackpotGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchJackpotGroupMgr) _mgr;
    }

    public static class RefTileMatchJackpotGroupMgr extends RefTableContainer<RefTileMatchJackpotGroup>
    {
        private Map<Long, WeightValueList<NPCommonCostItem>> _m_weightValueListMap = new HashMap<>();

        @Override
        public void _onTableLoaded()
        {
            Map<Long, WeightValueList<NPCommonCostItem>> map = new HashMap<>();
            for (RefTileMatchJackpotGroup refJackpot : getList())
            {
                for (int i = 0; i < refJackpot.reward_item_list.size(); i++)
                {
                    NPCommonCostItem item = refJackpot.reward_item_list.get(i);
                    //避免越界
                    if (refJackpot.reward_item_list.size() < i + 1)
                        continue;

                    int weight = refJackpot.item_wei_list.get(i);
                    if (weight <= 0)
                        continue;

                    WeightValueList<NPCommonCostItem> weightValueList = map.computeIfAbsent(refJackpot.group_id, k -> new WeightValueList<>());
                    weightValueList.add(item, weight);
                }
            }
            _m_weightValueListMap = map;
        }

        /**
         * 抽取道具
         */
        public NPCommonCostItem drawItem(long groupId)
        {
            WeightValueList<NPCommonCostItem> weightValueList = _m_weightValueListMap.get(groupId);
            //没有找到对应的奖池
            if (weightValueList == null)
                return null;

            return weightValueList.random();
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchJackpotGroup newRef = (RefTileMatchJackpotGroup) _newRef;
        id = newRef.id;
        group_id = newRef.group_id;
        reward_item_list = newRef.reward_item_list;
        item_wei_list = newRef.item_wei_list;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int id;
    public long group_id;//奖池组id
    public List<NPCommonCostItem> reward_item_list;//奖池列表
    public List<Integer> item_wei_list;//权重列表
}