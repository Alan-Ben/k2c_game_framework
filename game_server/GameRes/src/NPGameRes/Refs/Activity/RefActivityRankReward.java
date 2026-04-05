package NPGameRes.Refs.Activity;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairLong;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RefTable(tableName = "activity_rank_reward")
public class RefActivityRankReward extends RefBase
{
    private static RefActivityRankRewardMgr _g_mgr = new RefActivityRankRewardMgr();

    public static RefActivityRankRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefActivityRankRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefActivityRankRewardMgr) _mgr;
    }

    public static class RefActivityRankRewardMgr extends RefTableContainer<RefActivityRankReward>
    {
        private Map<Long, List<RefActivityRankReward>> _m_map = new HashMap<>();

        @Override
        protected void _onTableLoaded()
        {
            Map<Long, List<RefActivityRankReward>> map = new HashMap<>();
            for (RefActivityRankReward ref : getList())
            {
                map.computeIfAbsent(ref.rank_id, k -> new ArrayList<>()).add(ref);
            }

            _m_map = map;
        }

        /**
         * 通过排行榜id和排名，找到对应奖励配表
         * @param _rankId
         * @param _rank
         */
        public List<RefActivityRankReward> getRankRewardRefByRank(long _rankId, int _rank)
        {
            List<RefActivityRankReward> ActivityRankRewardList = _m_map.get(_rankId);
            if (ActivityRankRewardList == null)
                return null;

            List<RefActivityRankReward> resultList = new ArrayList<>();

            //遍历查找符合排名的奖励
            for (RefActivityRankReward refReward : ActivityRankRewardList)
            {
                if (_rank >= refReward.rank_begin && (_rank <= refReward.rank_end || refReward.rank_end == -1))
                {
                    resultList.add(refReward);
                }
            }

            return resultList;
        }

        /**
         * 通过排行榜id找到对应的有奖励最大排名
         * @param _rankId
         */
        public int getRankRewardMaxRank(long _rankId)
        {
            List<RefActivityRankReward> rankRewardList = _m_map.get(_rankId);
            if (rankRewardList == null)
                return 0;

            int maxRank = 0;
            for (RefActivityRankReward ref : rankRewardList)
            {
                if (ref.rank_end == -1)
                    return -1;

                if (ref.rank_end > maxRank)
                {
                    maxRank = ref.rank_end;
                }
            }

            return maxRank;
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefActivityRankReward newRef = (RefActivityRankReward) _newRef;
        id = newRef.id;
        rank_id = newRef.rank_id;
        rank_begin = newRef.rank_begin;
        rank_end = newRef.rank_end;
        reward_item_list = newRef.reward_item_list;
        member_reward_item_list = newRef.member_reward_item_list;
        title_reward = newRef.title_reward;
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
    public long rank_id;//排行奖励id
    public int rank_begin;//开始排名
    public int rank_end;//结束排名
    public ArrayList<NPCommonCostItem> reward_item_list = new ArrayList<>();//结算排名奖励
    public ArrayList<NPCommonCostItem> member_reward_item_list = new ArrayList<>();//联盟成员结算排名奖励
    
    public WCGPairLong title_reward;//称号奖励：称号ID:有效时长（秒）
    
    public long getTitleId() {return null == title_reward ? 0 : title_reward.first();}
    public long getTitleExpiredSecs() {return null == title_reward ? 0 : title_reward.second();}
    

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
