package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.GuildDungeonObj.GuildDungeon_SetInfo;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeon;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import USDB.Bo.GuildDungeonSetBO;

import java.util.ArrayList;
import java.util.HashSet;

/**
 * 公会副本管理对象
 * @author mj
 *
 */
public class GuildDungeonSetMgr 
{
	//公会数据
	private GuildInfo _m_giGuildInfo;
	//副本设置数据列表
	private ArrayList<GuildDungeonSetInfo> _m_alDungeonSetList;
	//锁对象
    private MutexObject _m_mutex;
	
	public GuildDungeonSetMgr(GuildInfo _guildInfo)
	{
		_m_giGuildInfo = _guildInfo;
		
		_m_alDungeonSetList = new ArrayList<>();
		
		_m_mutex = new MutexObject();
		
		_initAllRef();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public GuildInfo getGuild() {return _m_giGuildInfo;}
	public NPUserServer getUSServer() {return _m_giGuildInfo.getGuildMgr().getServer();}

	/**
	 * 所有副本加载到内存中
	 */
	private void _initAllRef()
	{
		for(RefGuildDungeon ref : RefGuildDungeon.getMgr().getList())
		{
			if(null == ref)
				continue;
			
			GuildDungeonSetInfo info = new GuildDungeonSetInfo(_m_giGuildInfo, ref);
			_m_alDungeonSetList.add(info);
		}
	}
	
	/**
	 * 加载bo数据
	 * @param _bo
	 */
	public void _loadBo(GuildDungeonSetBO _bo)
	{
		GuildDungeonSetInfo info = lookup(_bo.getDungeonId());
		if(null == info)
		{
			return;
		}
		
		info._load(_bo);
	}
	
	/**
	 * 加载完成后处理
	 */
	public void _onInited()
	{
		for(int i = 0; i < _m_alDungeonSetList.size(); i++)
		{
			GuildDungeonSetInfo info = _m_alDungeonSetList.get(i);
			if(null == info)
				continue;
			
			info._onInited();
		}
	}
	
	/**
	 * 查找指定公会副本设置数据
	 * @param _dungeonId
	 * @return
	 */
	public GuildDungeonSetInfo lookup(long _dungeonId)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonSetList.size(); i++)
			{
				GuildDungeonSetInfo info = _m_alDungeonSetList.get(i);
				if(null == info)
					continue;
				
				if(info.getDungeonId() == _dungeonId)
					return info;
			}
			
			return null;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造已解锁的公会配置数据
	 * @param _list
	 */
	public void makeUnlcokProto(ArrayList<GuildDungeon_SetInfo> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonSetList.size(); i++)
			{
				GuildDungeonSetInfo info = _m_alDungeonSetList.get(i);
				if(null == info)
					continue;
				
				if(info.isUnlock())
				{
					_list.add(info.toProto());
				}
			}
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 构造自动开启副本的ID列表
	 * @param _list
	 */
	public void makeAutoStartDungeonIdList(ArrayList<Long> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonSetList.size(); i++)
			{
				GuildDungeonSetInfo info = _m_alDungeonSetList.get(i);
				if(null == info)
					continue;
				
				if(info.isAutoStart())
				{
					_list.add(info.getDungeonId());
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 更新自动开启副本配置
	 * @param _dungeonIdSet
	 */
	public void updateAutoStartList(HashSet<Long> _dungeonIdSet)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alDungeonSetList.size(); i++)
			{
				GuildDungeonSetInfo setInfo = _m_alDungeonSetList.get(i);
				if(null == setInfo)
					continue;
				
				if(setInfo.isAutoStart() && !_dungeonIdSet.contains(setInfo.getDungeonId()))
				{
					setInfo.unsetAutoStart();
				}
				else if(!setInfo.isAutoStart() && _dungeonIdSet.contains(setInfo.getDungeonId()))
				{
					setInfo.setAutoStart();
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 销毁数据
	 */
	protected void _discard() 
	{
		_lock();
		
		try
		{
			getUSServer().getBM().getBM(GuildDungeonSetBO.class).delAll("guildId", getGuild().getGuildId());
			
			_m_alDungeonSetList.clear();
		}
		finally
		{
			_unlock();
		}
	}
	
	@Override
	public String toString()
	{
		_lock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("\nset size:").append(_m_alDungeonSetList.size());
			
			for(int i = 0; i < _m_alDungeonSetList.size(); i++)
			{
				GuildDungeonSetInfo info = _m_alDungeonSetList.get(i);
				if(null == info)
					continue;
				
				sb.append("\n==========================================\n")
					.append(info.toString());
			}
			
			sb.append("\n");
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
