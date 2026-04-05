package NPGameRes.Refs.Arena;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "arena_station_level")
public class RefArenaStationLevel extends RefBase
{
    private static RefTableContainer<RefArenaStationLevel> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefArenaStationLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefArenaStationLevel newRef = (RefArenaStationLevel) _newRef;
        level = newRef.level;
        upgrade_cost = newRef.upgrade_cost;
        harvest_ratio = newRef.harvest_ratio;
        storage_limit_sec = newRef.storage_limit_sec;
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
        _g_mgr = (RefTableContainer<RefArenaStationLevel>) _mgr;
    }

    @Override
    public long Id()
    {
        return level;
    }

    public int level;
    public NPCommonCostItem upgrade_cost;//升级到下一等级道具消耗
    public int harvest_ratio;//收成比例（万分比）
    public long storage_limit_sec;//储存上限时间/秒
}
