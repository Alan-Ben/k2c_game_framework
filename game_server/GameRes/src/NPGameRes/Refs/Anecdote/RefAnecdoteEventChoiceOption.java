package NPGameRes.Refs.Anecdote;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "anecdote_event_choice_option")
public class RefAnecdoteEventChoiceOption extends RefBase
{
    private static RefAnecdoteEventChoiceOptionMgr _g_mgr = new RefAnecdoteEventChoiceOptionMgr();
    public static RefAnecdoteEventChoiceOptionMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAnecdoteEventChoiceOptionMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAnecdoteEventChoiceOptionMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAnecdoteEventChoiceOption newRef = (RefAnecdoteEventChoiceOption) _newRef;
        id = newRef.id;
        reward_item_list = newRef.reward_item_list;
    }

    public static class RefAnecdoteEventChoiceOptionMgr extends RefTableContainer<RefAnecdoteEventChoiceOption>
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

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //唯一id
    public List<NPCommonCostItem> reward_item_list; //奖励物品列表
}
