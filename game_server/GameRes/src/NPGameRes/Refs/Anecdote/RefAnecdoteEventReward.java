package NPGameRes.Refs.Anecdote;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "anecdote_event_reward")
public class RefAnecdoteEventReward extends _ARefAnecdoteEvent
{
    private static RefAnecdoteEventRewardMgr _g_mgr = new RefAnecdoteEventRewardMgr();
    public static RefAnecdoteEventRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAnecdoteEventRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAnecdoteEventRewardMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAnecdoteEventReward newRef = (RefAnecdoteEventReward) _newRef;
        id = newRef.id;
        reward_item = newRef.reward_item;
    }

    public static class RefAnecdoteEventRewardMgr extends RefTableContainer<RefAnecdoteEventReward>
    {
    }

    @Override
    public EAnecdoteEventType getEventType()
    {
        return EAnecdoteEventType.REWARD;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//唯一id
    public NPCommonCostItem reward_item;//奖励
}