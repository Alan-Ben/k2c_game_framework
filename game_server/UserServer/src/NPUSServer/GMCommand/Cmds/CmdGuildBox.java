package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.Guild.GuildInfo;

/**
 * @author mark
 * @date 2022年4月14日
 */
@ACommander(comment = "联盟宝箱系统", name = "guildBox")
public class CmdGuildBox extends UsCmdBase
{
    @ACommand(comment = "增加活跃积分[增加的数值]")
    public String incrActivePoin(int _value)
    {
		GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
		if (guild == null)
			return "not in Local guild";
    	
    	guild.incrActivePoint(_value, getContext());
    	
    	return "ok";
    }
}
