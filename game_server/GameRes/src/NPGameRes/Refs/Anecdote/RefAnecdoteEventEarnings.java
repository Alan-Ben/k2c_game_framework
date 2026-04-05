package NPGameRes.Refs.Anecdote;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "anecdote_event_earnings")
public class RefAnecdoteEventEarnings extends _ARefAnecdoteEvent
{
    private static RefAnecdoteEventEarningsMgr _g_mgr = new RefAnecdoteEventEarningsMgr();
    public static RefAnecdoteEventEarningsMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAnecdoteEventEarningsMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAnecdoteEventEarningsMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAnecdoteEventEarnings newRef = (RefAnecdoteEventEarnings) _newRef;
        id = newRef.id;
        earnings = newRef.earnings;
        reward_item = newRef.reward_item;
        result_reward_item = newRef.result_reward_item;
    }

    public static class RefAnecdoteEventEarningsMgr extends RefTableContainer<RefAnecdoteEventEarnings>
    {
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    @Override
    public EAnecdoteEventType getEventType()
    {
        return EAnecdoteEventType.EARNINGS;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //唯一id
    public long earnings; //需要赚速
    public NPCommonCostItem reward_item; //奖励
    public NPCommonCostItem result_reward_item; //完成目标的奖励
}
