package ActivitiesV01.Refs.HotLoaders;

import ActivitiesV01.Refs.TileMatch.RefTileMatchJackpotGroup;
import NPCommon.Util.CommonFunc;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr.HotLoaders.ParseUtil;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefTileMatchJackpotGroup 的热更配表加载器
 *
 * 表结构：
 * - id: 唯一ID (int)
 * - group_id: 奖池组ID (long)
 * - reward_item_list: 奖池列表 (List<NPCommonCostItem>)
 * - item_wei_list: 权重列表 (List<Integer>)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefTileMatchJackpotGroupHotLoader implements _IHotRefTableLoader<RefTileMatchJackpotGroup>
{
    @Override
    public String getTableName()
    {
        return "tilematch_jackpot_group";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefTileMatchJackpotGroup> getRefClass()
    {
        return RefTileMatchJackpotGroup.class;
    }

    @Override
    public boolean updateSingleField(RefTileMatchJackpotGroup ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("id".equals(fieldName))
        {
            ref.id = tableInfo.getIntByFieldName(rowIndex, "id", 0);
            return true;
        }
        else if ("group_id".equals(fieldName))
        {
            ref.group_id = tableInfo.getLongByFieldName(rowIndex, "group_id", 0L);
            return true;
        }
        else if ("reward_item_list".equals(fieldName))
        {
            String rewardItemListStr = tableInfo.getStringByFieldName(rowIndex, "reward_item_list", "");
            ref.reward_item_list = ParseUtil.parseCommonCostItemList(rewardItemListStr);
            return true;
        }
        else if ("item_wei_list".equals(fieldName))
        {
            String itemWeiListStr = tableInfo.getStringByFieldName(rowIndex, "item_wei_list", "");
            ref.item_wei_list = CommonFunc.listIntFromString(itemWeiListStr);
            return true;
        }

        return false;
    }
}
