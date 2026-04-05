package NPGameRes.Refs.Guild;

import Common.GuildEnum.EGuildConstructType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.List;

@RefTable(tableName = "guild_construct")
public class RefGuildConstruct extends RefBase
{
    private static RefGuildConstructMgr _g_mgr = new RefGuildConstructMgr();

    public static RefGuildConstructMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildConstructMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildConstructMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildConstruct newRef = (RefGuildConstruct) _newRef;
        id = newRef.id;
        type = newRef.type;
        fix_cd_id = newRef.fix_cd_id;
        free_condition = newRef.free_condition;
        cost = newRef.cost;
        add_guild_exp = newRef.add_guild_exp;
        add_guild_wealth = newRef.add_guild_wealth;
        add_devote = newRef.add_devote;
        add_personal_guild_coin = newRef.add_personal_guild_coin;
        add_reward_point = newRef.add_reward_point;
    }

    public static class RefGuildConstructMgr extends RefTableContainer<RefGuildConstruct>
    {
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

    public long id; //唯一id
    public EGuildConstructType type;//类型
    public long fix_cd_id; //fixCd消耗
    public NPPlayerConditionGroupObj free_condition; //免建设花费条件
    public List<NPCommonCostItem> cost;//建设花费
    public int add_guild_exp;//增加联盟经验
    public int add_guild_wealth;//增加联盟财富
    public int add_devote;//增加个人贡献
    public int add_personal_guild_coin;//个人联盟币
    public int add_reward_point;//增加奖励进度
}
