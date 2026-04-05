package NPGameRes.Refs.Guild;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.WCGPairInt;

import java.util.List;

/**
 * 联盟协作区域配置表
 * <p>
 * 主要功能：
 * 1. 定义联盟协作区域的配置数据
 * 2. 提供区域属性和奖励配置的解析方法
 * 3. 管理区域配置的加载和访问
 * <p>
 * 配置项包括：
 * - 区域ID
 * - 奖励据点数量
 * - 属性据点血量配置
 * - 工会财富奖励
 * - 固定和随机奖励配置
 * - 据点数量范围
 * - 随机据点ID列表
 */
@RefTable(tableName = "guild_cooperate_area")
public class RefGuildCooperateArea extends RefBase
{
    // 管理器静态实例
    private static RefGuildCooperateAreaMgr _g_mgr = new RefGuildCooperateAreaMgr();

    /**
     * 获取配置管理器
     * @return 联盟协作区域配置管理器
     */
    public static RefGuildCooperateAreaMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildCooperateAreaMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildCooperateAreaMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildCooperateArea newRef = (RefGuildCooperateArea) _newRef;
        area_id = newRef.area_id;
        reward_point_num = newRef.reward_point_num;
        property_point_hp = newRef.property_point_hp;
        reward_point_guild_wealth_reward = newRef.reward_point_guild_wealth_reward;
        reward_point_reward_list = newRef.reward_point_reward_list;
        reward_point_random_reward_list = newRef.reward_point_random_reward_list;
        property_point_num_range = newRef.property_point_num_range;
        pos_id_list = newRef.pos_id_list;
        property_point_hp_per_guild_coin_reward = newRef.property_point_hp_per_guild_coin_reward;
        property_point_hp_per_guild_devote_reward = newRef.property_point_hp_per_guild_devote_reward;
    }

    /**
     * 管理联盟协作区域配置的容器类
     */
    public static class RefGuildCooperateAreaMgr extends RefTableContainer<RefGuildCooperateArea>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 在这里可以进行额外的初始化或验证操作
        }
    }

    /**
     * 获取对象的唯一标识
     * @return 区域ID作为唯一标识
     */
    @Override
    public long Id()
    {
        return area_id;
    }

    // 配置表字段定义
    public long area_id;// 区域ID
    public int reward_point_num;// 奖励据点数量
    public List<Long> property_point_hp;// 属性据点血量
    public int reward_point_guild_wealth_reward;// 奖励据点工会财富奖励值
    public List<NPCommonCostItem> reward_point_reward_list;// 奖励据点固定奖励列表
    public List<NPCommonCostItem> reward_point_random_reward_list;// 奖励据点随机奖励列表
    public WCGPairInt property_point_num_range;// 属性据点数量范围
    public List<Long> pos_id_list;// 需要随机的据点ID列表
    public long property_point_hp_per_guild_coin_reward;// 每对属性据点建设n%血量的公会币奖励
    public int property_point_hp_per_guild_devote_reward;// 每对属性据点建设n%血量的公会贡献奖励
}