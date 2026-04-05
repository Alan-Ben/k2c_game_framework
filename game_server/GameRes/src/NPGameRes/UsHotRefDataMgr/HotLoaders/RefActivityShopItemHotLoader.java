package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPCommon.CommonObj.NPCommonCostItem;
import NPGameRes.Refs.Activity.RefActivityShopItem;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefActivityShopItem 的热更配表加载器
 *
 * 表结构：
 * - id: 活动ID (long)
 * - activity_shop_id: 活动ID (long)
 * - item: 物品 (NPCommonCostItem)
 * - cost_item: 积分 (List<NPCommonCostItem>)
 * - discount: 兑换折扣 (int)
 * - buy_num: 兑换次数 (int)
 * - times_price_type_id: 单价递增消耗 (int)
 * - is_recommend: 是否推荐商品 (boolean)
 */
public class RefActivityShopItemHotLoader implements _IHotRefTableLoader<RefActivityShopItem>
{
    @Override
    public String getTableName()
    {
        return "activity_shop_item";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefActivityShopItem> getRefClass()
    {
        return RefActivityShopItem.class;
    }

    @Override
    public boolean updateSingleField(RefActivityShopItem ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("id".equals(fieldName))
        {
            ref.id = tableInfo.getLongByFieldName(rowIndex, "id", 0L);
            return true;
        }
        else if ("discount".equals(fieldName))
        {
            ref.discount = tableInfo.getIntByFieldName(rowIndex, "discount", 0);
            return true;
        }
        else if ("buy_num".equals(fieldName))
        {
            ref.buy_num = tableInfo.getIntByFieldName(rowIndex, "buy_num", 0);
            return true;
        }
        else if ("times_price_type_id".equals(fieldName))
        {
            ref.times_price_type_id = tableInfo.getIntByFieldName(rowIndex, "times_price_type_id", 0);
            return true;
        }
        else if ("is_recommend".equals(fieldName))
        {
            ref.is_recommend = tableInfo.getBooleanByFieldName(rowIndex, "is_recommend", false);
            return true;
        }
        else if ("item".equals(fieldName))
        {
            String itemStr = tableInfo.getStringByFieldName(rowIndex, "item", "");
            NPCommonCostItem costItem = new NPCommonCostItem();
            costItem.parseFromString(itemStr);
            ref.item = costItem;
            return true;
        }
        else if ("cost_item".equals(fieldName))
        {
            String costItemStr = tableInfo.getStringByFieldName(rowIndex, "cost_item", "");
            ref.cost_item = ParseUtil.parseCommonCostItemList(costItemStr);
            return true;
        }

        return false;
    }
}