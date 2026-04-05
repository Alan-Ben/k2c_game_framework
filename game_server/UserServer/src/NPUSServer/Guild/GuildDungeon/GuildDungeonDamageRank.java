package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.GuildDungeonObj.GuildDungeon_DamageRankItem;
import GS2GC.p037_GuildDungeonOp.GS2GC_037_007_RetDamageRank;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import USDB.Bo.GuildDungeonDamageRankBO;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * 公会副本攻击记录
 * @author mj
 *
 */
public class GuildDungeonDamageRank 
{
	//公会数据
	private GuildInfo _m_giGuildInfo;
	//数据记录列表
	private ArrayList<GuildDungeonDamageRankBO> _m_alRankBoList;
	//锁对象
	private MutexAtom _m_mutex;
	
	public GuildDungeonDamageRank(GuildInfo _guildInfo)
	{
		_m_giGuildInfo = _guildInfo;
		
		_m_alRankBoList = new ArrayList<>();
		
		_m_mutex = new MutexAtom();
	}

	public GuildInfo getGuild() {return _m_giGuildInfo;}
	public NPUserServer getUSServer() {return _m_giGuildInfo.getGuildMgr().getServer();}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public void _initBo(GuildDungeonDamageRankBO _bo)
	{
		_m_alRankBoList.add(_bo);
	}
	
	public void _onInited()
	{
		_sort();
	}
	
	/**
	 * 排序处理：分数高排前面
	 */
	private void _sort()
	{
		CommonFunc.sortAscList(_m_alRankBoList, new Comparator<GuildDungeonDamageRankBO>() 
		{
			@Override
			public int compare(GuildDungeonDamageRankBO o1, GuildDungeonDamageRankBO o2) 
			{
				if(o1.getValue() == o2.getValue())
				{
					return Long.compare(o1.getUpdatedMs(), o2.getUpdatedMs());
				}
				
				return Long.compare(o2.getValue(), o1.getValue());
			}
		});
	}
	
	/**
	 * 查找指定攻击记录
	 * @param _cid
	 * @return
	 */
	public GuildDungeonDamageRankBO lookup(long _cid)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alRankBoList.size(); i++)
			{
				GuildDungeonDamageRankBO bo = _m_alRankBoList.get(i);
				if(null == bo)
					continue;
				
				if(bo.getCid() == _cid)
					return bo;
			}
			
			return null;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加数据记录
	 * @param _cid
	 * @param _value
	 */
	public void addRank(long _cid, long _value) 
	{
		_lock();
		
		try
		{
			BM bmObj = getUSServer().getBM();
			
			GuildDungeonDamageRankBO bo = lookup(_cid);
			if(null == bo)
			{
				bo = new GuildDungeonDamageRankBO();
				bo.setGuildId(bmObj, getGuild().getGuildId());
				bo.setCid(bmObj, _cid);
				bo.setValue(bmObj, _value);
				bo.setUpdatedMs(bmObj, CommonFunc.getNowTimeMS());
				bo.insert(bmObj);

				_m_alRankBoList.add(bo);
			}
			else
			{
				long newValue = bo.getValue() + _value;
				
				bo.setValue(bmObj, newValue);
				bo.setUpdatedMs(bmObj, CommonFunc.getNowTimeMS());
				bo.saveAll(bmObj);
			}
			
			//重新排序
			_sort();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造数据列表协议
	 * @param _page
	 * @param _pageNum
	 * @param _proto
	 */
	public void makeProto(long _cid, int _page, int _pageNum, GS2GC_037_007_RetDamageRank _proto)
	{
		_lock();
		
		try
		{
			//排行数据
			int startIdx = (_page - 1) * _pageNum;
			for(int i = startIdx; i < _m_alRankBoList.size(); i++)
			{
				GuildDungeonDamageRankBO bo = _m_alRankBoList.get(i);
				if(null == bo)
					continue;
				
				GuildDungeon_DamageRankItem obj = new GuildDungeon_DamageRankItem();
				obj.setCid(bo.getCid());
				obj.setValue(bo.getValue());
				
				_proto.getItemList().add(obj);
				
				if(_proto.getItemList().size() >= _pageNum)
					break;
			}
			
			//获取玩家自身排行数据
			for(int i = 0; i < _m_alRankBoList.size(); i++)
			{
				GuildDungeonDamageRankBO bo = _m_alRankBoList.get(i);
				if(null == bo)
					continue;
				
				if(bo.getCid() == _cid)
				{
					_proto.setMyRank(i + 1);
					_proto.setMyValue(bo.getValue());
					break;
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 销毁实例数据
	 */
	protected void _discard() 
	{
		_lock();
		
		try
		{
			getUSServer().getBM().getBM(GuildDungeonDamageRankBO.class).delAll("guildId", getGuild().getGuildId());
			
			_m_alRankBoList.clear();
		}
		finally
		{
			_unlock();
		}
	}
}
