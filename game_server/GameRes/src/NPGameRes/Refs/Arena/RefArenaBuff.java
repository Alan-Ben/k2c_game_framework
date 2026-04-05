package NPGameRes.Refs.Arena;

import Common.ArenaEnum.EArenaBuffType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "arena_buff")
public class RefArenaBuff extends RefBase
{
    private static RefArenaBuffMgr _g_mgr = new RefArenaBuffMgr();

    public static RefArenaBuffMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefArenaBuffMgr extends RefTableContainer<RefArenaBuff>
    {
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefArenaBuff newRef = (RefArenaBuff) _newRef;
        id = newRef.id;
        type = newRef.type;
        value = newRef.value;
        cost = newRef.cost;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefArenaBuffMgr) _mgr;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public EArenaBuffType type;//类型
    public int value;//数值
    public NPCommonCostItem cost;//消耗
}
