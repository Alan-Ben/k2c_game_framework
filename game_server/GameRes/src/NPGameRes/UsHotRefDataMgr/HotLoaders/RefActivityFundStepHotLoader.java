package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPGameRes.Refs.ActivityFund.RefActivityFundStep;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefActivityFundStep 的热更配表加载器
 *
 * 支持热更字段：
 * - need_count: 领取所需计数 (long)
 * - free_reward_item_list: 免费档奖励 (ArrayList<NPCommonCostItem>)
 * - pay_reward_item_list: 付费档奖励 (ArrayList<NPCommonCostItem>)
 * - step: 阶段 (int)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefActivityFundStepHotLoader implements _IHotRefTableLoader<RefActivityFundStep>
{
    @Override
    public String getTableName()
    {
        return "activity_fund_step";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefActivityFundStep> getRefClass()
    {
        return RefActivityFundStep.class;
    }

    @Override
    public boolean updateSingleField(RefActivityFundStep ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
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
        else if ("need_count".equals(fieldName))
        {
            ref.need_count = tableInfo.getLongByFieldName(rowIndex, "need_count", 0L);
            return true;
        }
        else if ("free_reward_item_list".equals(fieldName))
        {
            String freeRewardItemListStr = tableInfo.getStringByFieldName(rowIndex, "free_reward_item_list", "");
            ref.free_reward_item_list = ParseUtil.parseCommonCostItemList(freeRewardItemListStr);
            return true;
        }
        else if ("pay_reward_item_list".equals(fieldName))
        {
            String payRewardItemListStr = tableInfo.getStringByFieldName(rowIndex, "pay_reward_item_list", "");
            ref.pay_reward_item_list = ParseUtil.parseCommonCostItemList(payRewardItemListStr);
            return true;
        }

        return false;
    }
}
