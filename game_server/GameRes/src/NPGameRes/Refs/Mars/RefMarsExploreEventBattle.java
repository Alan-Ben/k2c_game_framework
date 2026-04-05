package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;

@RefTable(tableName = "mars_explore_event_battle")
public class RefMarsExploreEventBattle extends RefBase
{
    private static RefMarsExploreEventBattleMgr _g_mgr = new RefMarsExploreEventBattleMgr();

    public static RefMarsExploreEventBattleMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExploreEventBattleMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExploreEventBattleMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExploreEventBattle newRef = (RefMarsExploreEventBattle) _newRef;
        event_id = newRef.event_id;
        quality = newRef.quality;
        refresh_wei = newRef.refresh_wei;
    }

    @Override
    public long Id()
    {
        return event_id;
    }

    public static class RefMarsExploreEventBattleMgr extends RefTableContainer<RefMarsExploreEventBattle>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long event_id;//事件id
    public EQuality quality = EQuality.NONE;//品质
    public int refresh_wei;//刷新权重
}