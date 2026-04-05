package NPGameRes.Refs.Building;

import NPCommon.Game.CS.CSWeightRandomList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;


@RefTable(tableName = "farming_building_level")
public class RefFarmingBuildingLevel extends RefBase implements _ILevelBasicObj
{
    private static RefFarmingBuildingLevelMgr _g_mgr = new RefFarmingBuildingLevelMgr();
    public static RefFarmingBuildingLevelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefFarmingBuildingLevelMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefFarmingBuildingLevelMgr) _mgr;
    }

    public static class RefFarmingBuildingLevelMgr extends RefTableContainer<RefFarmingBuildingLevel>
    {
        @Override
        public void _onTableLoaded()
        {
        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefFarmingBuildingLevel newRef = (RefFarmingBuildingLevel) _newRef;
        id = newRef.id;
        building_id = newRef.building_id;
        level = newRef.level;
        upgrade_cost_num = newRef.upgrade_cost_num;
        upgrade_cost_num_per_level = newRef.upgrade_cost_num_per_level;
        earning_rate = newRef.earning_rate;
        earning_rate_per_level = newRef.earning_rate_per_level;
        tap_to_collect_num = newRef.tap_to_collect_num;
        tap_to_collect_num_per_level = newRef.tap_to_collect_num_per_level;
        auto_tap_num_per_sec = newRef.auto_tap_num_per_sec;
        auto_tap_num_per_sec_per_level = newRef.auto_tap_num_per_sec_per_level;
        multiple_weight_list = newRef.multiple_weight_list;
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
    public int upgrade_cost_num;//升到这级所需的消耗
    public int upgrade_cost_num_per_level;//每级消耗增长值
    public int earning_rate;//建筑收益加成（万分比）
    public int earning_rate_per_level;//每级建筑收益加成增长值（万分比）
    public int tap_to_collect_num;//点击收益的数量
    public int tap_to_collect_num_per_level;//每级点击收益的数量增长值
    public int auto_tap_num_per_sec;//每秒自动点击收益
    public int auto_tap_num_per_sec_per_level;//每秒自动点击收益每级增长值
    public CSWeightRandomList multiple_weight_list = new CSWeightRandomList();//暴击倍数权重列表

}
