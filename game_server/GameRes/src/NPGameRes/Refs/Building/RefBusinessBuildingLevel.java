package NPGameRes.Refs.Building;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;


@RefTable(tableName = "business_building_level")
public class RefBusinessBuildingLevel extends RefBase implements _ILevelBasicObj
{
    private static RefBusinessBuildingLevelMgr _g_mgr = new RefBusinessBuildingLevelMgr();
    public static RefBusinessBuildingLevelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefBusinessBuildingLevelMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefBusinessBuildingLevelMgr) _mgr;
    }

    public static class RefBusinessBuildingLevelMgr extends RefTableContainer<RefBusinessBuildingLevel>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }
    
    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBusinessBuildingLevel newRef = (RefBusinessBuildingLevel) _newRef;
        id = newRef.id;
        building_id = newRef.building_id;
        level = newRef.level;
        upgrade_cost_item = newRef.upgrade_cost_item;
        earning_rate = newRef.earning_rate;
    }

    @Override
    public long Id()
    {
        return id;
    }

    @Override
    public int getLevel()
    {
        return level;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long building_id;//建筑唯一 id
    public int level;
    public NPCommonCostItem upgrade_cost_item;//升到这级所需的消耗
    public int earning_rate;//赚取资源的倍率
}
