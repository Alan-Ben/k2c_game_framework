package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPGameRes.Refs.Reward.RefReward;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefReward 的热更配表加载器
 *
 * 表结构：
 * - reward_id: 奖励ID (long)
 * - certainly_drop_item_count_list: 必掉物品列表 (List<NPCommonCostItem>)
 * - is_merge: 奖励是否合并 (boolean)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefRewardHotLoader implements _IHotRefTableLoader<RefReward>
{
    @Override
    public String getTableName()
    {
        return "reward";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "reward_id";
    }

    @Override
    public Class<RefReward> getRefClass()
    {
        return RefReward.class;
    }

    @Override
    public boolean updateSingleField(RefReward ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("reward_id".equals(fieldName))
        {
            ref.reward_id = tableInfo.getLongByFieldName(rowIndex, "reward_id", 0L);
            return true;
        }
        else if ("is_merge".equals(fieldName))
        {
            ref.is_merge = tableInfo.getBooleanByFieldName(rowIndex, "is_merge", false);
            return true;
        }
        else if ("certainly_drop_item_count_list".equals(fieldName))
        {
            String certainlyDropItemCountListStr = tableInfo.getStringByFieldName(rowIndex, "certainly_drop_item_count_list", "");
            ref.certainly_drop_item_count_list = ParseUtil.parseCommonCostItemList(certainlyDropItemCountListStr);
            return true;
        }

        return false;
    }
}
