package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPDateTimeObj;
import NPGameRes.Refs.RankGift.RefRankGiftPack;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefRankGiftPack 的热更配表加载器
 *
 * 支持热更字段：
 * - sale: 折扣(万分比) (int)
 * - buy_limit: 购买次数限制 (int)
 * - cost: 消耗道具 (NPCommonCostItem)
 * - ori_cost: 原价消耗道具 (NPCommonCostItem)
 * - reward_item_list: 奖励道具列表 (List<NPCommonCostItem>)
 * - end_time: 截止时间 (NPDateTimeObj)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefRankGiftPackHotLoader implements _IHotRefTableLoader<RefRankGiftPack>
{
    @Override
    public String getTableName()
    {
        return "rank_gift_pack";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefRankGiftPack> getRefClass()
    {
        return RefRankGiftPack.class;
    }

    @Override
    public boolean updateSingleField(RefRankGiftPack ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("id".equals(fieldName))
        {
            ref.id = tableInfo.getLongByFieldName(rowIndex, "id", 0L);
            return true;
        }
        else if ("sale".equals(fieldName))
        {
            ref.sale = tableInfo.getIntByFieldName(rowIndex, "sale", 0);
            return true;
        }
        else if ("buy_limit".equals(fieldName))
        {
            ref.buy_limit = tableInfo.getIntByFieldName(rowIndex, "buy_limit", 0);
            return true;
        }
        else if ("cost".equals(fieldName))
        {
            String costStr = tableInfo.getStringByFieldName(rowIndex, "cost", "");
            NPCommonCostItem cost = new NPCommonCostItem();
            cost.parseFromString(costStr);
            ref.cost = cost;
            return true;
        }
        else if ("ori_cost".equals(fieldName))
        {
            String oriCostStr = tableInfo.getStringByFieldName(rowIndex, "ori_cost", "");
            NPCommonCostItem oriCost = new NPCommonCostItem();
            oriCost.parseFromString(oriCostStr);
            ref.ori_cost = oriCost;
            return true;
        }
        else if ("reward_item_list".equals(fieldName))
        {
            String rewardItemListStr = tableInfo.getStringByFieldName(rowIndex, "reward_item_list", "");
            ref.reward_item_list = ParseUtil.parseCommonCostItemList(rewardItemListStr);
            return true;
        }
        else if ("end_time".equals(fieldName))
        {
            String endTimeStr = tableInfo.getStringByFieldName(rowIndex, "end_time", "");
            NPDateTimeObj endTime = new NPDateTimeObj();
            endTime.parseFromString(endTimeStr);
            ref.end_time = endTime;
            return true;
        }

        return false;
    }
}
