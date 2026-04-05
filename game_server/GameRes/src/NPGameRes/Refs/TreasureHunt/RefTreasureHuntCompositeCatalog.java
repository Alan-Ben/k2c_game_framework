package NPGameRes.Refs.TreasureHunt;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;

import java.util.List;

/**
 * id	矿石列表	普通技能id	高级技能id	组合品质
 * id	ore_list	normal_skill_id	advanced_skill_id	quality
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_composite_catalog")
public class RefTreasureHuntCompositeCatalog extends RefBase
{
    private static RefTreasureHuntCompositeCatalogMgr _g_mgr = new RefTreasureHuntCompositeCatalogMgr();

    public static RefTreasureHuntCompositeCatalogMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntCompositeCatalogMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntCompositeCatalogMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntCompositeCatalog newRef = (RefTreasureHuntCompositeCatalog) _newRef;
        id = newRef.id;
        ore_list = newRef.ore_list;
        normal_skill_id = newRef.normal_skill_id;
        advanced_skill_id = newRef.advanced_skill_id;
        quality = newRef.quality;
    }

    public static class RefTreasureHuntCompositeCatalogMgr extends RefTableContainer<RefTreasureHuntCompositeCatalog>
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

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//id
    public List<Long> ore_list;//矿石列表
    public long normal_skill_id;//普通技能id
    public long advanced_skill_id;//高级技能id
    public EQuality quality;//组合品质
}
