package NPGameRes.Refs.Building;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;


@RefTable(tableName = "farming_building")
public class RefFarmingBuilding extends RefBase
{
    private static RefFarmingBuildingMgr _g_mgr = new RefFarmingBuildingMgr();
    public static RefFarmingBuildingMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefFarmingBuildingMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefFarmingBuildingMgr) _mgr;
    }

    public static class RefFarmingBuildingMgr extends RefTableContainer<RefFarmingBuilding>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefFarmingBuilding newRef = (RefFarmingBuilding) _newRef;
        building_id = newRef.building_id;
        upgrade_cost_item = newRef.upgrade_cost_item;
        building_level_max = newRef.building_level_max;
    }

    @Override
    public long Id()
    {
        return building_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long building_id;
    public NPCommonItem upgrade_cost_item;//升级消耗道具
    public int building_level_max;//建筑等级上限

    @RefField(isIgnore = true)
    private _TLevelAreaMgr<RefFarmingBuildingLevel> _m_levelMapMgr = new _TLevelAreaMgr<>();
    public _TLevelAreaMgr<RefFarmingBuildingLevel> getLevelMapMgr()
    {
        return _m_levelMapMgr;
    }

    public void setLevelMapMgr(_TLevelAreaMgr<RefFarmingBuildingLevel> _mgr)
    {
        _m_levelMapMgr = _mgr;
    }
}
