package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_explore_pos")
public class RefMarsExplorePos extends RefBase
{
    private static RefMarsExplorePosMgr _g_mgr = new RefMarsExplorePosMgr();

    public static RefMarsExplorePosMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExplorePosMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExplorePosMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExplorePos newRef = (RefMarsExplorePos) _newRef;
        id = newRef.id;
        march_time = newRef.march_time;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsExplorePosMgr extends RefTableContainer<RefMarsExplorePos>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//id
    public long march_time;//行军时间(s)
}