package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GuildDungeonObj.GuildDungeon_Monster;
import Common.GuildDungeonObj.GuildDungeon_SetInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeon;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonLvl;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonMonster;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GuildDungeonSetBO;

import java.util.ArrayList;
import java.util.HashSet;

/**
 * 公会副本配置数据
 * @author mj
 *
 */
public class GuildDungeonSetInfo 
{
	//公会数据
	private GuildInfo _m_giGuildInfo;
	
	//副本配置数据
	private RefGuildDungeon _m_refDungeon;
	//副本等级配置数据
	private RefGuildDungeonLvl _m_refDungeonLvl;
	
	//副本自动开启
	private boolean _m_bIsAutoStart;

	//bo数据
	private GuildDungeonSetBO _m_bo;
	
	//锁对象
    private MutexAtom _m_mutex;
    
	public GuildDungeonSetInfo(GuildInfo _guildInfo, RefGuildDungeon _ref)
	{
		_m_giGuildInfo = _guildInfo;
		_m_refDungeon = _ref;
		
		_m_mutex = new MutexAtom();
		
		_initLvl();
	}
	
	protected void _lock() {_m_mutex.lock();}
	protected void _unlock() {_m_mutex.unlock();}
	
	public GuildInfo getGuild() {return _m_giGuildInfo;}
	public NPUserServer getUSServer() {return _m_giGuildInfo.getGuildMgr().getServer();}
	public long getGuildId() {return _m_giGuildInfo.getGuildId();}
	
	public RefGuildDungeon getRef() {return _m_refDungeon;}
	public long getDungeonId() {return _m_refDungeon.id;}
	
	public RefGuildDungeonLvl getLvlRef() {return _m_refDungeonLvl;}
	
	public GuildDungeonSetBO getBo() {return _m_bo;}
	
	/**
	 * 初始化加载等级数据，默认从1级开始
	 */
	private void _initLvl()
	{
		_m_refDungeonLvl = _m_refDungeon.getLevelMapMgr().getLevelData(1);
		if(null == _m_refDungeonLvl)
		{
			USLog.error(getUSServer(), "guild:{} dungeon:{} init lvl:{} not find ref.", getGuildId(), getDungeonId(), 1);
		}
	}
	
	/**
	 * 加载bo数据
	 * @param _bo
	 */
	public void _load(GuildDungeonSetBO _bo)
	{
		_m_bo = _bo;
		//是否自动启动副本
		_m_bIsAutoStart = _m_bo.getIsAutoStart();
		
		//公会副本等级
		_m_refDungeonLvl = _m_refDungeon.getLevelMapMgr().getLevelData(_bo.getDungeonLvl());
		if(null == _m_refDungeonLvl)
		{
			USLog.error(getUSServer(), "guild:{} dungeon:{} lvl:{} not load lvl ref.", "guild:{} GuildDungeonBO load bo fail, not find dungeon lvl:{}.", 
					getGuildId(), getDungeonId(), _bo.getDungeonLvl());
		}
	}
	
	/**
	 * 加载完成后处理
	 */
	public void _onInited()
	{
	}

	/**
	 * 更新BO数据
	 */
	private void _saveBo()
	{
		_lock();
		
		try
		{
			BM bmObj = getUSServer().getBM();
			
			if(null == _m_bo)
			{
				_m_bo = new GuildDungeonSetBO();
				_m_bo.setGuildId(bmObj, getGuildId());
				_m_bo.setDungeonId(bmObj, getDungeonId());
				_m_bo.setDungeonLvl(bmObj, getLvl());
				_m_bo.setIsAutoStart(bmObj, _m_bIsAutoStart);
				_m_bo.insert(bmObj);
			}
			else
			{
				_m_bo.setDungeonLvl(bmObj, getLvl());
				_m_bo.setIsAutoStart(bmObj, _m_bIsAutoStart);
				_m_bo.saveAll(bmObj);
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 公会副本是否解锁
	 * @return
	 */
	public boolean isUnlock()
	{
		return null != _m_refDungeonLvl && _m_giGuildInfo.getLevel() >= _m_refDungeon.unlock_need_guild_lvl;
	}
	
	/**
	 * 是否自动启动
	 * @return
	 */
	public boolean isAutoStart()
	{
		return null != _m_refDungeonLvl && _m_bIsAutoStart;
	}
	
	/**
	 * 设置自动开启
	 */
	public void setAutoStart()
	{
		_m_bIsAutoStart = true;
		
		_saveBo();
	}
	
	/**
	 * 关闭自动开启
	 */
	public void unsetAutoStart()
	{
		_m_bIsAutoStart = false;
		
		_saveBo();
	}
	
	/**
	 * 获取当前等级数据
	 * @return
	 */
	public int getLvl() 
	{
		_lock();
		
		try
		{
			return null == _m_refDungeonLvl ? 0 : _m_refDungeonLvl.lvl;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 更新副本等级
	 * @param _lvlRef
	 * @param _nextLvlRef
	 * @param _context
	 * @return
	 */
	public boolean cmdUpgradeLvl(RefGuildDungeonLvl _lvlRef, RefGuildDungeonLvl _nextLvlRef, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//公会已经销毁
			if(_m_giGuildInfo.isDissolve())
				return false;
			
			//未解锁不能进行设置等级操作
			if(!isUnlock())
				return false;
			
			//避免其他玩家升级了副本，引发副本数据变更
			if(_lvlRef != _m_refDungeonLvl)
				return false;
			
			//更新等级数据
			_m_refDungeonLvl = _nextLvlRef;
			_saveBo();

			//推送数据
			ALSynTaskManager.getInstance().regTask(()->
			{
				_m_giGuildInfo.getMemberMgr().broadcastMsg(US2GCWriter_037_GuildDungeonOp.make_050_OnDungeonSetChg(this));
			});
			
			return true;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造怪物数据列表
	 * @return
	 */
	public ArrayList<GuildDungeon_Monster> buildMonsterList() 
	{
		RefGuildDungeon ref = _m_refDungeon;
		RefGuildDungeonLvl lvlRef = _m_refDungeonLvl;
		
		if(null == ref || null == lvlRef || ref.getDungeonMonsterRefList().isEmpty())
		{
			USLog.error(getUSServer(), "guild:{} dungeon:{} lvl:{} build monster fail.", getGuildId(), getDungeonId(), getLvl());
			return null;
		}
		
		//选定有奖励的怪物
		HashSet<Long> rewardMonsterIdSet = ref.buildRewardMonsterList();
		
		ArrayList<GuildDungeon_Monster> monsterList = new ArrayList<>();
		for(int i = 0; i < ref.getDungeonMonsterRefList().size(); i++)
		{
			RefGuildDungeonMonster monsterRef = ref.getDungeonMonsterRefList().get(i);
			if(null == monsterRef)
				continue;

			long value = lvlRef.monster_hp.getValue(monsterRef.monster_type);
			if(value <= 0)
			{
				USLog.error(getUSServer(), "guild:{} dungeon:{} lvl:{} monster:{} hp error.", getGuildId(), getDungeonId(), getLvl(), monsterRef.id);
				return null;
			}
			
			GuildDungeon_Monster monster = new GuildDungeon_Monster();
			monster.setMonsterId(monsterRef.id);
			monster.setHp(value);
			monster.setIsReward(rewardMonsterIdSet.contains(monsterRef.id));
			
			monsterList.add(monster);
		}
		
		return monsterList;
	}
	
	/**
	 * 构造副本数据
	 * @return
	 */
	public GuildDungeon_SetInfo toProto()
	{
		_lock();
		
		try
		{
			GuildDungeon_SetInfo proto = new GuildDungeon_SetInfo();
			proto.setDungeonId(getDungeonId());
			proto.setLvl(getLvl());
			
			return proto;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 操作升级
	 * @param _userData
	 * @param _oriLvl
	 * @param _context
	 * @return
	 */
	public Result checkUpgradeLvl(int _oriLvl, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			if(!isUnlock())
			{
				return GuildErr.GUILD_DUNGEON_NOT_UNLOCK;
			}
			
			//等级已发生变化，可能是其他玩家进行了升级
			if(_oriLvl != getLvl())
			{
				return GuildErr.GUILD_DUNGEON_LVL_UPDATED;
			}
			
			//检查下一个等级
			int nextLvl = _oriLvl + 1;
			RefGuildDungeonLvl nextLvlRef = _m_refDungeon.getLevelMapMgr().getLevelData(nextLvl);
			if(null == nextLvlRef)
			{
				return CommErr.REF_NOT_FOUND;
			}
			
			return Result.SUCC;
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
			
			sb.append("dungeon:").append(getDungeonId());
			if(!isUnlock())
			{
				sb.append(": not unlock.");
			}
			else
			{
				sb.append(", lvl:").append(getLvl())
					.append(", isAutoStart:").append(isAutoStart());
			}
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
