package NPGameRes.Refs.Arena;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.List;

@RefTable(tableName = "arena_final_reward")
public class RefArenaFinalReward extends RefBase
{
    private static RefArenaFinalRewardMgr _g_mgr = new RefArenaFinalRewardMgr();

    public static RefArenaFinalRewardMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefArenaFinalRewardMgr extends RefTableContainer<RefArenaFinalReward>
    {
        public RefArenaFinalReward getRefByDefeatNum(int _defeatNum)
        {
            for (RefArenaFinalReward ref : getList())
            {
                // 检查击败数量范围
                if ((ref.defeat_num_range.first() <= _defeatNum || ref.defeat_num_range.first() == -1)
                        && (ref.defeat_num_range.second() >= _defeatNum || ref.defeat_num_range.second() == -1))
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
        RefArenaFinalReward newRef = (RefArenaFinalReward) _newRef;
        id = newRef.id;
        defeat_num_range = newRef.defeat_num_range;
        reward_item_list = newRef.reward_item_list;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefArenaFinalRewardMgr) _mgr;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public WCGPairInt defeat_num_range = new WCGPairInt(-1, -1);//击败数量范围
    public List<NPCommonCostItem> reward_item_list;//奖励道具列表
}
