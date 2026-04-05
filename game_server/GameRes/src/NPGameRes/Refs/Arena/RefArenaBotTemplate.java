package NPGameRes.Refs.Arena;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;

@RefTable(tableName = "arena_bot_template")
public class RefArenaBotTemplate extends RefBase
{
    private static RefArenaBotTemplateMgr _g_mgr = new RefArenaBotTemplateMgr();

    public static RefArenaBotTemplateMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefArenaBotTemplateMgr extends RefTableContainer<RefArenaBotTemplate>
    {
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefArenaBotTemplate newRef = (RefArenaBotTemplate) _newRef;
        id = newRef.id;
        power_per = newRef.power_per;
        hero_num_per = newRef.hero_num_per;
        level_per_pair = newRef.level_per_pair;
        power_per_pair = newRef.power_per_pair;
    }

    @Override
    public RefContainerBase<? extends RefBase> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefArenaBotTemplateMgr) _mgr;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public long id;
    public int power_per;//总实力万分比
    public int hero_num_per;//伙伴数量万分比
    public WCGPairInt level_per_pair;//分组伙伴等级万分比(高战力:低战力)
    public WCGPairInt power_per_pair;//分组伙伴实力万分比(高战力:低战力)
}
