package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Reward.RefRewardSub;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefRewardSub 的热更配表加载器
 *
 * 表结构：
 * - reward_sub_id: 奖励id (long)
 * - reward_id: 奖励id (long)
 * - pro_item_count_list: 概率掉落-物品数量列表 (List<NPCommonCostItem>)
 * - pro_list: 有效概率列表 (List<Long>)
 * - only_pro_suc_wei: 是否只有成功的时候才走wei掉落 (boolean)
 * - wei_count: 权重掉落数 (int)
 * - wei_item_count_list: 奖励配置 (List<NPCommonCostItem>)
 * - wei_list: 奖励配置 (List<Long>)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefRewardSubHotLoader implements _IHotRefTableLoader<RefRewardSub>
{
    @Override
    public String getTableName()
    {
        return "reward_sub";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "reward_sub_id";
    }

    @Override
    public Class<RefRewardSub> getRefClass()
    {
        return RefRewardSub.class;
    }

    @Override
    public boolean updateSingleField(RefRewardSub ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("reward_sub_id".equals(fieldName))
        {
            ref.reward_sub_id = tableInfo.getLongByFieldName(rowIndex, "reward_sub_id", 0L);
            return true;
        }
        else if ("only_pro_suc_wei".equals(fieldName))
        {
            ref.only_pro_suc_wei = tableInfo.getBooleanByFieldName(rowIndex, "only_pro_suc_wei", false);
            return true;
        }
        else if ("wei_count".equals(fieldName))
        {
            ref.wei_count = tableInfo.getIntByFieldName(rowIndex, "wei_count", 0);
            return true;
        }
        else if ("pro_item_count_list".equals(fieldName))
        {
            String proItemCountListStr = tableInfo.getStringByFieldName(rowIndex, "pro_item_count_list", "");
            ref.pro_item_count_list = ParseUtil.parseCommonCostItemList(proItemCountListStr);
            return true;
        }
        else if ("pro_list".equals(fieldName))
        {
            String proListStr = tableInfo.getStringByFieldName(rowIndex, "pro_list", "");
            ref.pro_list = CommonFunc.listLongFromString(proListStr);
            return true;
        }
        else if ("wei_item_count_list".equals(fieldName))
        {
            String weiItemCountListStr = tableInfo.getStringByFieldName(rowIndex, "wei_item_count_list", "");
            ref.wei_item_count_list = ParseUtil.parseCommonCostItemList(weiItemCountListStr);
            return true;
        }
        else if ("wei_list".equals(fieldName))
        {
            String weiListStr = tableInfo.getStringByFieldName(rowIndex, "wei_list", "");
            ref.wei_list = CommonFunc.listLongFromString(weiListStr);
            return true;
        }

        return false;
    }
}