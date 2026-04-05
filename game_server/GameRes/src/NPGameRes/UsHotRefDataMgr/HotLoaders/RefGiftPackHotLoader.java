package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPCommon.CommonObj.NPRefreshTimeObj;
import NPGameRes.Refs.GiftPack.RefGiftPack;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefGiftPack 的热更配表加载器
 *
 * 表结构：
 * - id: 礼包ID (long)
 * - buy_limit_count: 限购次数 (int)
 * - buy_limit_refresh_time: 限购次数刷新时间 (NPRefreshTimeObj)
 * - item_list: 奖励物品列表 (List<NPCommonCostItem>)
 * - cost_list: 消耗列表 (List<NPCommonCostItem>)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefGiftPackHotLoader implements _IHotRefTableLoader<RefGiftPack>
{
    @Override
    public String getTableName()
    {
        return "gift_pack";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefGiftPack> getRefClass()
    {
        return RefGiftPack.class;
    }

    @Override
    public boolean updateSingleField(RefGiftPack ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("id".equals(fieldName))
        {
            ref.id = tableInfo.getLongByFieldName(rowIndex, "id", 0L);
            return true;
        }
        else if ("buy_limit_count".equals(fieldName))
        {
            ref.buy_limit_count = tableInfo.getIntByFieldName(rowIndex, "buy_limit_count", 0);
            return true;
        }
        else if ("buy_limit_refresh_time".equals(fieldName))
        {
            String buyLimitRefreshTimeStr = tableInfo.getStringByFieldName(rowIndex, "buy_limit_refresh_time", "");
            NPRefreshTimeObj refreshTimeObj = new NPRefreshTimeObj();
            refreshTimeObj.parseFromString(buyLimitRefreshTimeStr);
            ref.buy_limit_refresh_time = refreshTimeObj;
            return true;
        }
        else if ("item_list".equals(fieldName))
        {
            String itemListStr = tableInfo.getStringByFieldName(rowIndex, "item_list", "");
            ref.item_list = ParseUtil.parseCommonCostItemList(itemListStr);
            return true;
        }
        else if ("cost_list".equals(fieldName))
        {
            String costListStr = tableInfo.getStringByFieldName(rowIndex, "cost_list", "");
            ref.cost_list = ParseUtil.parseCommonCostItemList(costListStr);
            return true;
        }

        return false;
    }
}
