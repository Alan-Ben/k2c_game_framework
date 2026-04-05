package NPGameRes.Refs.Arena;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "arena_round_reward")
public class RefArenaRoundReward extends RefBase
{
    private static RefTableContainer<RefArenaRoundReward> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefArenaRoundReward> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefArenaRoundReward newRef = (RefArenaRoundReward) _newRef;
        round = newRef.round;
        reward_id = newRef.reward_id;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    @SuppressWarnings("unchecked")
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefArenaRoundReward>) _mgr;
    }

    @Override
    public long Id()
    {
        return round;
    }

    public long round;
    public long reward_id;//奖励id
}
