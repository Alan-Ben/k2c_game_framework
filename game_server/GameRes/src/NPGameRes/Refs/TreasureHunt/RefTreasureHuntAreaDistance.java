package NPGameRes.Refs.TreasureHunt;

import NPCommon.Game.QualityValueData;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 飞行区域深度	获得宝箱奖励(累加)	获取奖励数量增加(覆盖)(基础有1个)	矿石品质万分比修正(覆盖)	奇物品质万分比修正(覆盖)
 * distance	common_item	capture_reward_num_add	ore_gain_weight_adjust_per	treasure_gain_weight_adjust_per
 */
@RefTable(tableName = "treasure_hunt_area_distance")
public class RefTreasureHuntAreaDistance extends RefBase
{
    private static RefTreasureHuntTreasureMgr _g_mgr = new RefTreasureHuntTreasureMgr();

    public static RefTreasureHuntTreasureMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntTreasureMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntTreasureMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntAreaDistance newRef = (RefTreasureHuntAreaDistance) _newRef;
        distance = newRef.distance;
        capture_reward_num_add = newRef.capture_reward_num_add;
        ore_gain_weight_adjust_per = newRef.ore_gain_weight_adjust_per;
    }

    public static class RefTreasureHuntTreasureMgr extends RefTableContainer<RefTreasureHuntAreaDistance>
    {
        @Override
        protected void _onTableLoaded()
        {
        }

        /**
         * 通过距离获取对应的飞行区域深度配置
         * @param _distance
         * @return
         */
        public RefTreasureHuntAreaDistance getRefByDistance(long _distance)
        {
            RefTreasureHuntAreaDistance ref = null;
            for (RefTreasureHuntAreaDistance temp : getList())
            {
                if (_distance >= temp.distance)
                {
                    ref = temp;
                }else
                {
                    break;
                }
            }
            return ref;
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return distance;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long distance;//飞行区域深度
    public int capture_reward_num_add;//获取奖励数量增加(覆盖)(基础有1个)
    public QualityValueData ore_gain_weight_adjust_per = new QualityValueData();//矿石品质万分比修正(覆盖)
}
