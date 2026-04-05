package NPUSServer.GMCommand.Cmds;

import Common.GuildDungeonObj.GuildDungeon_Monster;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.Guild.GuildDungeon.GuildDungeonSetInfo;
import NPUSServer.Guild.GuildInfo;

import java.util.ArrayList;

/**
 * @description: 任务相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "联盟副本", name = "guildDungeon")
public class CmdGuildDungeon extends UsCmdBase
{
    @ACommand(comment = "展示联盟副本配置数据")
    public String showSet()
    {
		GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
		if (guild == null)
			return "not in Local guild";
    	
    	return guild.getDungeonMgr().getGlobalSetInfo().toString() + guild.getDungeonMgr().getSetMgr().toString();
    }

    @ACommand(comment = "展示联盟副本配置生成的怪物数据[副本ID]")
    public String showSetMonster(long _dungeonId)
    {
		GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
		if (guild == null)
			return "not in Local guild";
    	
    	GuildDungeonSetInfo setInfo = guild.getDungeonMgr().getSetMgr().lookup(_dungeonId);
    	if(null == setInfo)
    		return "fail, guild not find set.";
    	
    	ArrayList<GuildDungeon_Monster> monsterList = setInfo.buildMonsterList();
    	if(null == monsterList)
    		return "fail, guild dungeno not build monster.";
    	
    	StringBuilder sb = new StringBuilder();
    	for(int i = 0; i < monsterList.size(); i++)
    	{
    		GuildDungeon_Monster monster = monsterList.get(i);
    		if(null == monster)
    			continue;
    		
    		sb.append("\nmonster:").append(monster.getMonsterId()).append(", hp:").append(monster.getHp()).append(", isReward:").append(monster.getIsReward());
    	}
    	sb.append("\n");
    	
    	return sb.toString();
    }
    
    @ACommand(comment = "展示联盟副本实例数据")
    public String showInstance()
    {
		GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
		if (guild == null)
			return "not in Local guild";
    	
    	return guild.getDungeonMgr().getInstanceMgr().toString();
    }

    @ACommand(comment = "设置下次开启时间[偏移秒数]")
    public String setNextAutoStartMs(int _offSecs)
    {
		GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
		if (guild == null)
			return "not in Local guild";

    	long time = CommonFunc.getNowTimeMS() + _offSecs * 1000;
    	guild.getDungeonMgr().getGlobalSetInfo().setNextAutoStartMs(time);
    	
    	return "ok";
    }

    @ACommand(comment = "设置下次结算时间[偏移秒数]")
    public String setNextAutoSettleTime(int _offSecs)
    {
		GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
		if (guild == null)
			return "not in Local guild";

    	long time = CommonFunc.getNowTimeMS() + _offSecs * 1000;
    	guild.getDungeonMgr().getGlobalSetInfo().setNextAutoSettleMs(time);
    	
    	return "ok";
    }

    @ACommand(comment = "结算所有副本")
    public String settleAll()
    {
		GuildInfo guild = getUserServer().getGuildMgr().lookupGuild(getOwner().getGuildComponent().getGuildId());
		if (guild == null)
			return "not in Local guild";
    	
    	guild.getDungeonMgr().settleAll(getContext());
    	
    	return "ok";
    }
}
