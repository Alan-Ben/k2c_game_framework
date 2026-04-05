package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.Guild.GuildInfo;

/**
 * @description: 联盟协作相关
 */
@ACommander(comment = "联盟协作", name = "guildCooperate")
public class CmdGuildCooperate extends UsCmdBase
{
    @ACommand(comment = "恢复所有大臣使用记录")
    public String recoverHeroUse()
    {
        getOwner().getGuildCooperateComponent().recoverAllHeroUseRecords(getContext());
        return "ok";
    }

    @ACommand(comment = "清除据点奖励领取记录")
    public String clearAllDrawRecords()
    {
        getOwner().getGuildCooperateComponent().clearAllDrawRecords(getContext());
        return "ok";
    }

    @ACommand(comment = "修改属性据点已造成攻击 [区域id][奖励据点索引][属性据点索引][已造成攻击]")
    public String chgPropPointHadDamage(long areaId, int index, int propertyPointIndex, long _damage)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        return guildInfo.getCooperateInfo().chgPropPointHadDamage(areaId, index, propertyPointIndex, _damage, getContext()).toString();
    }

    @ACommand(comment = "击败指定奖励据点 [区域id][奖励据点索引]")
    public String defeatRewardPoint(long areaId, int index)
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        return guildInfo.getCooperateInfo().forceDefeatRewardPoint(areaId, index, getContext()).toString();
    }

    @ACommand(comment = "重新刷新地图")
    public String resetMap()
    {
        GuildInfo guildInfo = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
        if (guildInfo == null)
            return "not in Local guild";

        guildInfo.getCooperateInfo().checkResetData(true);
        return "ok";
    }
}
