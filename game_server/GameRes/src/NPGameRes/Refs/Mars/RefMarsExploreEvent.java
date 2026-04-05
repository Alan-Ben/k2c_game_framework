package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_explore_event")
public class RefMarsExploreEvent extends RefBase
{
    private static RefMarsExploreEventMgr _g_mgr = new RefMarsExploreEventMgr();

    public static RefMarsExploreEventMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExploreEventMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExploreEventMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExploreEvent newRef = (RefMarsExploreEvent) _newRef;
        event_id = newRef.event_id;
    }

    @Override
    public long Id()
    {
        return event_id;
    }

    public static class RefMarsExploreEventMgr extends RefTableContainer<RefMarsExploreEvent>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long event_id;//事件id
}