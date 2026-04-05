package NPGameRes.Refs.Anecdote;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "anecdote_event")
public class RefAnecdoteEvent extends RefBase
{
    private static RefAnecdoteEventMgr _g_mgr = new RefAnecdoteEventMgr();

    public static RefAnecdoteEventMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefAnecdoteEventMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefAnecdoteEventMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAnecdoteEvent newRef = (RefAnecdoteEvent) _newRef;
        id = newRef.id;
        show_condition = newRef.show_condition;
        next_event_item = newRef.next_event_item;
    }

    public static class RefAnecdoteEventMgr extends RefTableContainer<RefAnecdoteEvent>
    {
        @Override
        protected void _onTableLoaded()
        {

        }
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
    public NPPlayerConditionGroupObj show_condition; //显示条件
    public WCGPairLong next_event_item; //下一个事件 事件id:位置

    @RefField(isIgnore = true)
    public _ARefAnecdoteEvent detailRef;

    public void setDetailRef(_ARefAnecdoteEvent _ref)
    {
        detailRef = _ref;
    }
}
