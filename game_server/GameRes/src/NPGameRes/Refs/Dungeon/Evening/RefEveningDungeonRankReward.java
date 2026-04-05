package NPGameRes.Refs.Dungeon.Evening;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "evening_dungeon_rank_reward")
public class RefEveningDungeonRankReward extends RefBase
{
    private static RefEveningDungeonRankRewardMgr _g_mgr = new RefEveningDungeonRankRewardMgr();

    public static RefEveningDungeonRankRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefEveningDungeonRankRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefEveningDungeonRankRewardMgr) _mgr;
    }

    public static class RefEveningDungeonRankRewardMgr extends RefTableContainer<RefEveningDungeonRankReward>
    {
        /**
         * 找到对应的有奖励最大排名
         */
        public int getRankRewardMaxRank()
        {
            int maxRank = 0;
            for (RefEveningDungeonRankReward ref : getList())
            {
                if (ref.rank_end == -1)
                    return -1;

                if (ref.rank_end > maxRank)
                    maxRank = ref.rank_end;
            }

            return maxRank;
        }

        /**
         * 根据排名获取对应的奖励
         * @param _rank
         * @return
         */
        public RefEveningDungeonRankReward getRefByRank(int _rank)
        {
            for (RefEveningDungeonRankReward ref : getList())
            {
                if (ref.rank_begin <= _rank && ref.rank_end >= _rank)
                {
                    return ref;
                }
            }
            return null;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefEveningDungeonRankReward newRef = (RefEveningDungeonRankReward) _newRef;
        id = newRef.id;
        rank_begin = newRef.rank_begin;
        rank_end = newRef.rank_end;
        reward_item_list = newRef.reward_item_list;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//唯一id
    public int rank_begin;//开始排名
    public int rank_end;//结束排名
    public List<NPCommonCostItem> reward_item_list;//结算排名奖励

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
