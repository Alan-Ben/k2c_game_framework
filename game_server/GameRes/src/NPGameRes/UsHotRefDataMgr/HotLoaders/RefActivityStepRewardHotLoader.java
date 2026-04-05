package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPGameRes.Refs.Activity.RefActivityStepReward;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefActivityStepReward 的热更配表加载器
 *
 * 表结构：
 * - id: 唯一ID (long)
 * - step_reward_set_id: 阶段奖励ID (long)
 * - step: 阶段奖励步骤 (int)
 * - complete_count: 完成计数目标值 (long)
 * - reward_item_list: 完成阶段奖励物品列表 (ArrayList<NPCommonCostItem>)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefActivityStepRewardHotLoader implements _IHotRefTableLoader<RefActivityStepReward>
{
    @Override
    public String getTableName()
    {
        return "activity_step_reward";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefActivityStepReward> getRefClass()
    {
        return RefActivityStepReward.class;
    }

    @Override
    public boolean updateSingleField(RefActivityStepReward ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("id".equals(fieldName))
        {
            ref.id = tableInfo.getLongByFieldName(rowIndex, "id", 0L);
            return true;
        }
        else if ("step".equals(fieldName))
        {
            ref.step = tableInfo.getIntByFieldName(rowIndex, "step", 0);
            return true;
        }
        else if ("complete_count".equals(fieldName))
        {
            ref.complete_count = tableInfo.getLongByFieldName(rowIndex, "complete_count", 0L);
            return true;
        }
        else if ("reward_item_list".equals(fieldName))
        {
            String rewardItemListStr = tableInfo.getStringByFieldName(rowIndex, "reward_item_list", "");
            ref.reward_item_list = ParseUtil.parseCommonCostItemList(rewardItemListStr);
            return true;
        }

        return false;
    }
}
