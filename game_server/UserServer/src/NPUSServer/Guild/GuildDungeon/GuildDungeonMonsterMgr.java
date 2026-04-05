package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GuildDungeonObj.GuildDungeon_DungeonMonster;
import Common.GuildDungeonObj.GuildDungeon_Monster;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GuildDungeonMonsterBO;

import java.util.ArrayList;
import java.util.HashSet;

public class GuildDungeonMonsterMgr 
{
	//副本实例数据
	private GuildDungeonInstanceInfo _m_diDungeonInfo;
	//boss怪物数据
	private GuildDungeonMonsterInfo _m_bmiBossMonsterInfo;
	//怪物数据列表
	private ArrayList<GuildDungeonMonsterInfo> _m_alMonsterInfoList;

	public GuildDungeonMonsterMgr(GuildDungeonInstanceInfo _info)
	{
		_m_diDungeonInfo = _info;
		
		_m_alMonsterInfoList = new ArrayList<>();
	}
	
	public GuildDungeonInstanceInfo getDungeon() {return _m_diDungeonInfo;}
	public GuildInfo getGuild() {return _m_diDungeonInfo.getGuild();}
	public NPUserServer getUSServer() {return _m_diDungeonInfo.getGuild().getGuildMgr().getServer();}

	public GuildDungeonMonsterInfo getBoss() {return _m_bmiBossMonsterInfo;}
	
	private void _lock() {_m_diDungeonInfo._lock();}
	private void _unlock() {_m_diDungeonInfo._unlock();}
	
	public void _initBo(GuildDungeonMonsterBO _bo)
	{
		GuildDungeonMonsterInfo monster = new GuildDungeonMonsterInfo(_m_diDungeonInfo, _bo);
		
		_initAddMonster(monster);
	}

	/**
	 * 初始化加载怪物数据
	 * @param _monster
	 */
	protected void _initAddMonster(GuildDungeonMonsterInfo _monster) 
	{
		_m_alMonsterInfoList.add(_monster);
		
		//设置boss数据
		if(_monster.isBoss())
		{
			if(null != _m_bmiBossMonsterInfo)
			{
				USLog.error(getUSServer(), "guild:{} dungeon:{} already has boss:{}, new boss:{}.", 
						getGuild().getGuildId(), _m_diDungeonInfo.getDungeonId(), _m_bmiBossMonsterInfo.getMonsterId());
			}
			
			_m_bmiBossMonsterInfo = _monster;
		}
	}
	
	/**
	 * 加载完成后处理
	 */
	protected void _onInited()
	{
	}
	
	/**
	 * 获取boss怪物的血量
	 * @return
	 */
	public long getBossHp()
	{
		_lock();
		
		try
		{
			return null == _m_bmiBossMonsterInfo ? 0 : _m_bmiBossMonsterInfo.getHp();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查boss是否被击杀
	 * @return
	 */
	public boolean isBossKilled()
	{
		_lock();
		
		try
		{
			return null != _m_bmiBossMonsterInfo && _m_bmiBossMonsterInfo.getHp() <= 0;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 查找对应怪物数据
	 * @param _monsterId
	 * @return
	 */
	public GuildDungeonMonsterInfo lookup(long _monsterId)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo info = _m_alMonsterInfoList.get(i);
				if(null == info)
					continue;
			
				if(info.getMonsterId() == _monsterId)
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
	 * 构造怪物数据列表
	 * @param _list
	 */
	public void makeProto(ArrayList<GuildDungeon_Monster> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo info = _m_alMonsterInfoList.get(i);
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
	
	/**
	 * 构造已标记的怪物ID列表
	 * @param _list
	 */
	public void makeTagMonsterIdList(ArrayList<Long> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo info = _m_alMonsterInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.isTag())
				{
					_list.add(info.getMonsterId());
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取指定玩家已领取怪物奖励数据
	 * @param _cid
	 * @param _list
	 */
	public void makeGainedRewardList(long _cid, ArrayList<GuildDungeon_DungeonMonster> _list)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo info = _m_alMonsterInfoList.get(i);
				if(null == info)
					continue;
				
				if(info._isGainedReward(_cid))
				{
					_list.add(info.toDungeonMonsterProto());
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 结算玩家怪物奖励
	 * @param _memberCid
	 * @param _collector
	 */
	protected void _settlePlayerMonsterReward(long _memberCid, NPItemCollector _collector) 
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo monster = _m_alMonsterInfoList.get(i);
				if(null == monster)
					continue;
				
				NPCommonCostItem gainItem = monster._gainReward(_memberCid);
				if(null != gainItem)
				{
					_collector.addItem(gainItem);
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置领取怪物奖励
	 * @param _cid
	 * @param _context
	 */
	public void setGainAllMonsterReward(long _cid, NPItemCollector _collector)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo monster = _m_alMonsterInfoList.get(i);
				if(null == monster)
					continue;
			
				NPCommonCostItem gainItem = monster._gainReward(_cid);
				if(null != gainItem)
				{
					_collector.addItem(gainItem);
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置标签
	 * @param _tagMonsterIdList
	 * @return
	 */
	public Result cmdSetTag(HashSet<Long> _tagMonsterIdList)
	{
		_lock();
		
		try
		{
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo info = _m_alMonsterInfoList.get(i);
				if(null == info)
					continue;
				
				if(info.isTag() && !_tagMonsterIdList.contains(info.getMonsterId()))
				{
					info._setTag(false);
				}
				else if(!info.isTag() && _tagMonsterIdList.contains(info.getMonsterId()))
				{
					info._setTag(true);
				}
			}

			//广播所有玩家
			ALSynTaskManager.getInstance().regTask(()->
			{
				_m_diDungeonInfo.getGuild().getMemberMgr().broadcastMsg(US2GCWriter_037_GuildDungeonOp.make_055_OnDungeonTagMonsterChg(_m_diDungeonInfo));
			});
			
			return Result.SUCC;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置已领奖
	 * @param _cid
	 * @param _monsterId
	 * @return
	 */
	public Result cmdSetGainedReward(long _cid, long _monsterId, ArrayList<NPCommon.NPCommon_ItemInfo> _recReward)
	{
		//领取奖励
		NPCommonCostItem rewardItem;

		_lock();
		try
		{
			GuildDungeonMonsterInfo info = lookup(_monsterId);
			if(null == info)
				return GuildErr.GUILD_DUNGEON_MONSTER_NOT_FOUND;
			
			if(!info.isReward())
				return GuildErr.GUILD_DUNGEON_MONSTER_NOT_REWARD;
			
			if(!info.isKilled())
				return GuildErr.GUILD_DUNGEON_MONSTER_NOT_KILLED;
			
			if(info._isGainedReward(_cid))
				return GuildErr.GUILD_DUNGEON_MONSTER_GAINED_REWARD;

			//领取奖励
			rewardItem = info._gainReward(_cid);
		}
		finally
		{
			_unlock();
		}

		if(null != _recReward)
		{
			_recReward.add(rewardItem.toProto());
		}

		return Result.SUCC;
	}
	
	@Override
	public String toString()
	{
		_lock();
		
		try
		{
			StringBuilder sb = new StringBuilder();
			
			sb.append("monster:").append(_m_alMonsterInfoList.size());
			for(int i = 0; i < _m_alMonsterInfoList.size(); i++)
			{
				GuildDungeonMonsterInfo info = _m_alMonsterInfoList.get(i);
				if(null == info)
					continue;
				
				sb.append("\n").append("monster id:").append(info.getMonsterId())
					.append(", type:").append(info.getMonsterType())
					.append(", hp:").append(info.getHp())
					.append(", isReward:").append(info.isReward())
					.append(", isTag:").append(info.isTag());
			}
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
