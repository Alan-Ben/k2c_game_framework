package NPGameRes.Refs.Guild;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "guild_construct_reward")
public class RefGuildConstructReward extends RefBase
{
    private static RefGuildConstructRewardMgr _g_mgr = new RefGuildConstructRewardMgr();

    public static RefGuildConstructRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildConstructRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildConstructRewardMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildConstructReward newRef = (RefGuildConstructReward) _newRef;
        num = newRef.num;
        reward_item_list = newRef.reward_item_list;
    }

    public static class RefGuildConstructRewardMgr extends RefTableContainer<RefGuildConstructReward>
    {
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return num;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int num; //唯一id
    public List<NPCommonCostItem> reward_item_list;//奖励列表
}
