package NPGameRes.Refs.Building;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;


@RefTable(tableName = "business_building_product")
public class RefBusinessBuildingProduct extends RefBase
{
    private static RefBusinessBuildingProductMgr _g_mgr = new RefBusinessBuildingProductMgr();
    public static RefBusinessBuildingProductMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefBusinessBuildingProductMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefBusinessBuildingProductMgr) _mgr;
    }

    public static class RefBusinessBuildingProductMgr extends RefTableContainer<RefBusinessBuildingProduct>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }
    
    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBusinessBuildingProduct newRef = (RefBusinessBuildingProduct) _newRef;
        id = newRef.id;
        building_id = newRef.building_id;
        employee_required = newRef.employee_required;
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
    public int employee_required;//所需等级
    public UnionBonus add_bonus;//属性加成

}
