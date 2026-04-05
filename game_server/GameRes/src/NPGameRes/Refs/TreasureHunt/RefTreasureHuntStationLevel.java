package NPGameRes.Refs.TreasureHunt;

import NPCommon.Game.QualityValueData;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.Refs.RefGeneral;

/**
 * 太空舱等级	每次拾取处理经验	升到下级所需处理经验	获取矿石和奖励道具的权重列表	可获取的矿石品质比例	奖励
 * level	each_pickup_exp	level_up_need_exp	gain_type_weight_list	ore_quality_weight_list	reward_id
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_station_lvl")
public class RefTreasureHuntStationLevel extends RefBase
{
    private static RefTreasureHuntStationLevelMgr _g_mgr = new RefTreasureHuntStationLevelMgr();

    public static RefTreasureHuntStationLevelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntStationLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntStationLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntStationLevel newRef = (RefTreasureHuntStationLevel) _newRef;
        level = newRef.level;
        each_pickup_exp = newRef.each_pickup_exp;
        level_up_need_exp = newRef.level_up_need_exp;
        ore_gain_weight = newRef.ore_gain_weight;
        ore_quality_weight_list = newRef.ore_quality_weight_list;
        reward_gain_weight = newRef.reward_gain_weight;
        reward_id = newRef.reward_id;
        max_fly_distance = newRef.max_fly_distance;
    }

    public static class RefTreasureHuntStationLevelMgr extends RefTableContainer<RefTreasureHuntStationLevel>
    {
        @Override
        protected void _onTableLoaded()
        {
            for (RefTreasureHuntStationLevel refLevel : getList())
            {
                // 初始化高级能源权重列表
                QualityValueData advancedEnergyWeightList = refLevel.ore_quality_weight_list.copy();
                advancedEnergyWeightList.merge(RefGeneral.Ref().treasure_hunt_advanced_energy_add_ore_quality_weight);
                refLevel.advancedEnergyWeightList = advancedEnergyWeightList;
            }
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return level;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long level;//太空舱等级
    public int each_pickup_exp;//每次拾取处理经验
    public long level_up_need_exp;//升到下级所需处理经验
    public int ore_gain_weight;//矿石抽取权重
    public QualityValueData ore_quality_weight_list = new QualityValueData();//可获取的矿石品质比例
    public int reward_gain_weight;//奖励抽取权重
    public long reward_id;//奖励
    public long max_fly_distance;//最远飞行距离

    @RefField(isIgnore = true)
    public QualityValueData advancedEnergyWeightList = new QualityValueData(); // 高级能源权重列表
}
