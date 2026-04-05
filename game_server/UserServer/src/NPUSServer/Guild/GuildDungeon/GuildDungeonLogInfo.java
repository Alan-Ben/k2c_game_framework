package NPUSServer.Guild.GuildDungeon;

import Common.GuildDungeonEnum.EGuildDungeon_LogType;
import Common.GuildDungeonObj.GuildDungeon_Log;
import USDB.Bo.GuildDungeonLogBO;

public class GuildDungeonLogInfo 
{
	//副本实例数据
	private GuildDungeonInstanceInfo _m_diDungeonInstance;
	//bo数据
	private GuildDungeonLogBO _m_bo;
	
	public GuildDungeonLogInfo(GuildDungeonInstanceInfo _dungeon, GuildDungeonLogBO _bo)
	{
		_m_diDungeonInstance = _dungeon;
		
		_m_bo = _bo;
	}

	public GuildDungeonInstanceInfo getInstance() {return _m_diDungeonInstance;}
	
	public GuildDungeonLogBO getBo() {return _m_bo;}
	public long getId() {return getBo().getId();}
	public EGuildDungeon_LogType getLogType() {return EGuildDungeon_LogType.EGuildDungeon_LogType_FromInt(getBo().getLogType());}
	
	public GuildDungeon_Log toProto()
	{
		GuildDungeon_Log proto = new GuildDungeon_Log();
		proto.setLogType(getLogType());
		proto.setCreatedAt(getBo().getCreatedAt());
		proto.setInfo(getBo().getInfo());
		
		return proto;
	}
	
	protected void _del() 
	{
		_m_bo.del(getInstance().getGuild().getGuildMgr().getServer().getBM());
	}
}
