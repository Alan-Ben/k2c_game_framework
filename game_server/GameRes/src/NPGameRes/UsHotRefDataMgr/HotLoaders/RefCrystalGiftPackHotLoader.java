package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPGameRes.Refs.CrystalGiftPack.RefCrystalGiftPack;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefCrystalGiftPack 的热更配表加载器
 *
 * 表结构：
 * - id: 礼包ID (long)
 * - crystal_gift_pack_group_id: 礼包组ID (long)
 * - item_list: 获得物品列表 (ArrayList<NPCommonCostItem>)
 * - discount: 折扣 (int)
 * - buy_num: 可购买次数 (int)
 * - cost_item: 固定单价 (List<NPCommonCostItem>)
 * - times_price_type_id: 单价递增消耗 (int)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefCrystalGiftPackHotLoader implements _IHotRefTableLoader<RefCrystalGiftPack>
{
    @Override
    public String getTableName()
    {
        return "crystal_gift_pack";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefCrystalGiftPack> getRefClass()
    {
        return RefCrystalGiftPack.class;
    }

    @Override
    public boolean updateSingleField(RefCrystalGiftPack ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
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
        else if ("item_list".equals(fieldName))
        {
            String itemListStr = tableInfo.getStringByFieldName(rowIndex, "item_list", "");
            ref.item_list = ParseUtil.parseCommonCostItemList(itemListStr);
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
