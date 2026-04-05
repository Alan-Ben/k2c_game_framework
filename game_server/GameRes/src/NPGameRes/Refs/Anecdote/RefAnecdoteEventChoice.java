package NPGameRes.Refs.Anecdote;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "anecdote_event_choice")
public class RefAnecdoteEventChoice extends _ARefAnecdoteEvent
{
    private static RefAnecdoteEventChoiceMgr _g_mgr = new RefAnecdoteEventChoiceMgr();
    public static RefAnecdoteEventChoiceMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAnecdoteEventChoiceMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAnecdoteEventChoiceMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAnecdoteEventChoice newRef = (RefAnecdoteEventChoice) _newRef;
        id = newRef.id;
        option_id_list = newRef.option_id_list;
    }

    public static class RefAnecdoteEventChoiceMgr extends RefTableContainer<RefAnecdoteEventChoice>
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
        return EAnecdoteEventType.CHOICE;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //唯一id
    public List<Long> option_id_list; //选项列表
}
