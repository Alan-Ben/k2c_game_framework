package NPGameRes.Refs.Guild;

import Common.GuildEnum.EGuildPositionType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.Pair.GuildPositionNumPair;

import java.util.List;

@RefTable(tableName = "guild_level")
public class RefGuildLevel extends RefBase
{
    private static RefGuildLevelMgr _g_mgr = new RefGuildLevelMgr();

    public static RefGuildLevelMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGuildLevelMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGuildLevelMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGuildLevel newRef = (RefGuildLevel) _newRef;
        level = newRef.level;
        need_exp = newRef.need_exp;
        member_limit = newRef.member_limit;
        position_limit_list = newRef.position_limit_list;
        mail_reward_list = newRef.mail_reward_list;
        gain_box_need_active_point = newRef.gain_box_need_active_point;
        gain_guild_box_id = newRef.gain_guild_box_id;
        construct_gain_guild_exp_limit = newRef.construct_gain_guild_exp_limit;
        construct_gain_guild_wealth_limit = newRef.construct_gain_guild_wealth_limit;
    }

    /**
     * 获取指定职位的人数上限
     * @param _positionType
     * @return
     */
    public int getPositionLimit(EGuildPositionType _positionType)
    {
        for (GuildPositionNumPair pair : position_limit_list)
        {
            if (pair.type() == _positionType)
            {
                return (int) pair.num();
            }
        }
        return 0;
    }

    public static class RefGuildLevelMgr extends RefTableContainer<RefGuildLevel>
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
        return level;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int level; //唯一id
    public int need_exp; //升级所需经验
    public int member_limit; //联盟人员总数量上限
    public List<GuildPositionNumPair> position_limit_list; //职位数量限制列表
    public List<NPCommonCostItem> mail_reward_list; //升级邮件奖励列表
    
    public int gain_box_need_active_point;//获得宝箱需要的联盟活跃点
    public long gain_guild_box_id;//联盟等级对应的联盟宝箱id
    public long construct_gain_guild_exp_limit;//建造联盟经验获得上限
    public long construct_gain_guild_wealth_limit;//建造联盟财富获得上限
}