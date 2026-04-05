package NPUSServer.Guild.GuildDungeon;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.GuildDungeonEnum.EGuildDungeon_LogType;
import Common.GuildDungeonObj.GuildDungeon_Log;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import USDB.Bo.GuildDungeonLogBO;

import java.util.ArrayList;
import java.util.Comparator;

public class GuildDungeonLogMgr 
{
	//副本实例数据
	private GuildDungeonInstanceInfo _m_diDungeonInstance;
	//日志数据列表
	private ArrayList<GuildDungeonLogInfo> _m_alLogInfoList;
	
	public GuildDungeonLogMgr(GuildDungeonInstanceInfo _dungeon)
	{
		_m_diDungeonInstance = _dungeon;
		
		_m_alLogInfoList = new ArrayList<>();
	}
	
	public GuildDungeonInstanceInfo getInstance() {return _m_diDungeonInstance;}
	
	private void _lock() {_m_diDungeonInstance._lock();}
	private void _unlock() {_m_diDungeonInstance._unlock();}
	
	public void _initBo(GuildDungeonLogBO _bo)
	{
		_m_alLogInfoList.add(new GuildDungeonLogInfo(_m_diDungeonInstance, _bo));
	}
	
	protected void _onInited()
	{
		_sort();
	}
	
	private void _sort()
	{
		CommonFunc.sortAscList(_m_alLogInfoList, new Comparator<GuildDungeonLogInfo>() 
		{
			@Override
			public int compare(GuildDungeonLogInfo o1, GuildDungeonLogInfo o2) 
			{
				return Long.compare(o2.getId(), o1.getId());
			}
		});
		
		while(_m_alLogInfoList.size() > 0 && _m_alLogInfoList.size() > RefGeneral.Ref().guild_dungeon_log_limit)
		{
			int lastIdx = _m_alLogInfoList.size() - 1;
			GuildDungeonLogInfo removeInfo = _m_alLogInfoList.remove(lastIdx);
			
			if(null != removeInfo)
			{
				removeInfo._del();
			}
		}
	}
	
	/**
	 * 增加日志
	 * @param _logType
	 * @param _info
	 */
	public void addLog(EGuildDungeon_LogType _logType, _IALProtocolStructure _info)
	{
		_lock();
		
		try
		{
			BM bmObj = getInstance().getGuild().getGuildMgr().getServer().getBM();
			
			GuildDungeonLogBO bo = new GuildDungeonLogBO();
			bo.setGuildId(bmObj, getInstance().getGuildId());
			bo.setInstaceId(bmObj, getInstance().getId());
			bo.setLogType(bmObj, _logType.ordinal());
			bo.setCreatedAt(bmObj, CommonFunc.getNowTimeSec());
			bo.setInfo(bmObj, CommonFunc.ByteBfferToBytes(_info.makePackage()));
			bo.insert(bmObj);
			
			GuildDungeonLogInfo info = new GuildDungeonLogInfo(_m_diDungeonInstance, bo);
			_m_alLogInfoList.add(info);
			
			_sort();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<GuildDungeon_Log> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alLogInfoList.size(); i++)
			{
				GuildDungeonLogInfo info = _m_alLogInfoList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toProto());
			}
		}
		finally
		{
			_unlock();
		}
	}
}
