package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPGameRes.Refs.RechargeRebate.RefRechargeRebateStep;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefRechargeRebateStep 的热更配表加载器
 *
 * 表结构：
 * - id: 档位ID (long)
 * - group_id: 所属组ID (long)
 * - step: 档位顺序 (long)
 * - target_count: 目标计数 (long)
 * - reward_list: 奖励列表 (List<NPCommonCostItem>)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefRechargeRebateStepHotLoader implements _IHotRefTableLoader<RefRechargeRebateStep>
{
    @Override
    public String getTableName()
    {
        return "recharge_rebate_step";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefRechargeRebateStep> getRefClass()
    {
        return RefRechargeRebateStep.class;
    }

    @Override
    public boolean updateSingleField(RefRechargeRebateStep ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("id".equals(fieldName))
        {
            ref.id = tableInfo.getLongByFieldName(rowIndex, "id", 0L);
            return true;
        }
        else if ("step".equals(fieldName))
        {
            ref.step = tableInfo.getLongByFieldName(rowIndex, "step", 0L);
            return true;
        }
        else if ("target_count".equals(fieldName))
        {
            ref.target_count = tableInfo.getLongByFieldName(rowIndex, "target_count", 0L);
            return true;
        }
        else if ("reward_list".equals(fieldName))
        {
            String rewardListStr = tableInfo.getStringByFieldName(rowIndex, "reward_list", "");
            ref.reward_list = ParseUtil.parseCommonCostItemList(rewardListStr);
            return true;
        }

        return false;
    }
}
