package NPGameRes.UsHotRefDataMgr.HotLoaders;

import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.Refs.Activity.RefActivityRankReward;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefTableInfo;
import NPGameRes.UsHotRefDataMgr._IHotRefTableLoader;

/**
 * RefActivityRankReward 的热更配表加载器
 *
 * 表结构：
 * - id: 个人排行榜奖励ID (long)
 * - rank_id: 个人排行榜奖励ID (long)
 * - rank_begin: 开始名次 (int)
 * - rank_end: 结束名次 (int)
 * - title_reward: 称号奖励 (WCGPairLong)
 * - reward_item_list: 奖励内容 (ArrayList<NPCommonCostItem>)
 * - member_reward_item_list: 联盟成员奖励内容 (ArrayList<NPCommonCostItem>)
 *
 * 硬编码实现：避免反射，性能最优
 */
public class RefActivityRankRewardHotLoader implements _IHotRefTableLoader<RefActivityRankReward>
{
    @Override
    public String getTableName()
    {
        return "activity_rank_reward";
    }

    @Override
    public String getPrimaryKeyFieldName()
    {
        return "id";
    }

    @Override
    public Class<RefActivityRankReward> getRefClass()
    {
        return RefActivityRankReward.class;
    }

    @Override
    public boolean updateSingleField(RefActivityRankReward ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex)
    {
        // 使用 if-else 链判断字段名，第一次匹配后立即返回
        if ("id".equals(fieldName))
        {
            ref.id = tableInfo.getLongByFieldName(rowIndex, "id", 0L);
            return true;
        }
        else if ("rank_begin".equals(fieldName))
        {
            ref.rank_begin = tableInfo.getIntByFieldName(rowIndex, "rank_begin", 0);
            return true;
        }
        else if ("rank_end".equals(fieldName))
        {
            ref.rank_end = tableInfo.getIntByFieldName(rowIndex, "rank_end", 0);
            return true;
        }
        else if ("title_reward".equals(fieldName))
        {
            String titleRewardStr = tableInfo.getStringByFieldName(rowIndex, "title_reward", "");
            WCGPairLong pair = new WCGPairLong();
            pair.parseFromString(titleRewardStr);
            ref.title_reward = pair;
            return true;
        }
        else if ("reward_item_list".equals(fieldName))
        {
            String rewardItemListStr = tableInfo.getStringByFieldName(rowIndex, "reward_item_list", "");
            ref.reward_item_list = ParseUtil.parseCommonCostItemList(rewardItemListStr);
            return true;
        }
        else if ("member_reward_item_list".equals(fieldName))
        {
            String memberRewardItemListStr = tableInfo.getStringByFieldName(rowIndex, "member_reward_item_list", "");
            ref.member_reward_item_list = ParseUtil.parseCommonCostItemList(memberRewardItemListStr);
            return true;
        }

        return false;
    }
}