package NPGameRes.Refs.Building;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;


@RefTable(tableName = "business_building_develop")
public class RefBusinessBuildingDevelop extends RefBase
{
    private static RefBusinessBuildingDevelopMgr _g_mgr = new RefBusinessBuildingDevelopMgr();
    public static RefBusinessBuildingDevelopMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefBusinessBuildingDevelopMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefBusinessBuildingDevelopMgr) _mgr;
    }

    public static class RefBusinessBuildingDevelopMgr extends RefTableContainer<RefBusinessBuildingDevelop>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }
    
    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBusinessBuildingDevelop newRef = (RefBusinessBuildingDevelop) _newRef;
        id = newRef.id;
        building_id = newRef.building_id;
        level_required = newRef.level_required;
        add_bonus = newRef.add_bonus;
    }

    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long building_id;//建筑唯一 id
    public int level_required;//所需等级
    public UnionBonus add_bonus;//属性加成

}
