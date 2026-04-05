package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GuildDungeonEnum.EGuildDungeon_LogType;
import Common.GuildDungeonObj.GuildDungeon_InstanceInfo;
import Common.GuildDungeonObj.GuildDungeon_LogAttack;
import Common.GuildDungeonObj.GuildDungeon_LogKill;
import NPCommon.ErrMain.GuildErr;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeon;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GuildDungeonInstanceBO;
import USLOGDB.Bo.LogGuildDungeonMonsterKillBO;

/**
 * 公会副本实例数据
 * @author mj
 *
 */
public class GuildDungeonInstanceInfo 
{
	//公会数据
	private GuildInfo _m_giGuildInfo;

	//bo数据
	private GuildDungeonInstanceBO _m_bo;
	
	//副本配置数据
	private RefGuildDungeon _m_refDungeon;
	//副本等级配置数据
	private RefGuildDungeonLvl _m_refDungeonLvl;
	
	//怪物数据管理对象
	private GuildDungeonMonsterMgr _m_mgrDungeonMonsterMgr;
	//攻击日志管理对象
	private GuildDungeonLogMgr _m_mgrDungeonLogMgr;
	
	//锁对象
    private MutexAtom _m_mutex;
	
    /**
     * 数据加载副本实例对象
     * @param _guildInfo
     * @param _bo
     */
	public GuildDungeonInstanceInfo(GuildInfo _guildInfo, GuildDungeonInstanceBO _bo)
	{
		_m_giGuildInfo = _guildInfo;
		_m_bo = _bo;
		
		_m_mgrDungeonMonsterMgr = new GuildDungeonMonsterMgr(this);
		_m_mgrDungeonLogMgr = new GuildDungeonLogMgr(this);
		
		_m_mutex = new MutexAtom();
		
		_initFromBo();
	}
	public GuildDungeonInstanceInfo(GuildInfo _guildInfo, GuildDungeonInstanceBO _bo, RefGuildDungeon _ref, RefGuildDungeonLvl _lvlRef)
	{
		_m_giGuildInfo = _guildInfo;
		_m_bo = _bo;
		_m_refDungeon = _ref;
		_m_refDungeonLvl = _lvlRef;
		
		_m_mgrDungeonMonsterMgr = new GuildDungeonMonsterMgr(this);
		_m_mgrDungeonLogMgr = new GuildDungeonLogMgr(this);
		
		_m_mutex = new MutexAtom();
	}
	
	protected void _lock() {_m_mutex.lock();}
	protected void _unlock() {_m_mutex.unlock();}
	
	public GuildInfo getGuild() {return _m_giGuildInfo;}
	public NPUserServer getUSServer() {return _m_giGuildInfo.getGuildMgr().getServer();}
	public long getGuildId() {return _m_giGuildInfo.getGuildId();}
	
	public RefGuildDungeon getRef() {return _m_refDungeon;}
	public RefGuildDungeonLvl getLvlRef() {return _m_refDungeonLvl;}
	public int getLvl() {return null == _m_refDungeonLvl ? 0 : _m_refDungeonLvl.lvl;}
	
	public GuildDungeonInstanceBO getBo() {return _m_bo;}
	public long getId() {return _m_bo.getId();}
	public long getDungeonId() {return _m_bo.getDungeonId();}
	public long getStartMs() {return _m_bo.getStartMs();}
	
	//实例副本已经完成
	public boolean isDone() {return _m_mgrDungeonMonsterMgr.isBossKilled();}
	
	//公会副本实例已结算标志
	public boolean isSettled() {return _m_bo.getIsSettled();}
	public void setSettled() {_m_bo.saveIsSettled(getUSServer().getBM(), true);}
	
	public GuildDungeonMonsterMgr getMonsterMgr() {return _m_mgrDungeonMonsterMgr;}
	public GuildDungeonLogMgr getLogMgr() {return _m_mgrDungeonLogMgr;}
	
	/**
	 * bo加载数据
	 */
	private void _initFromBo()
	{
		//加载副本配置
		_m_refDungeon = RefGuildDungeon.getMgr().get(_m_bo.getDungeonId());
		if(null == _m_refDungeon)
		{
			USLog.error(getUSServer(), "guild:{} dungeon:{} not find ref.", _m_bo.getGuildId(), _m_bo.getDungeonId());
			return;
		}
		
		//加载副本等级配置
		_m_refDungeonLvl = _m_refDungeon.getLevelMapMgr().getLevelData(_m_bo.getDungeonLvl());
		if(null == _m_refDungeonLvl)
		{
			USLog.error(getUSServer(), "guild:{} dungeon:{} lvl:{} not find ref.", _m_bo.getGuildId(), _m_bo.getDungeonId(), _m_bo.getDungeonLvl());
			return;
		}
	}
	
	/**
	 * 加载完成后处理
	 */
	public void _onInited()
	{
		_m_mgrDungeonMonsterMgr._onInited();
		_m_mgrDungeonLogMgr._onInited();
	}
	
	/**
	 * 构造副本数据
	 * @return
	 */
	public GuildDungeon_InstanceInfo toProto()
	{
		_lock();
		
		try
		{
			GuildDungeon_InstanceInfo proto = new GuildDungeon_InstanceInfo();
			proto.setId(getId());
			proto.setDungeonId(getDungeonId());
			proto.setLvl(getLvl());
			proto.setStartMs(getStartMs());
			_m_mgrDungeonMonsterMgr.makeProto(proto.getMonsterList());
			_m_mgrDungeonMonsterMgr.makeTagMonsterIdList(proto.getTagMonsterIdLiist());
			
			return proto;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 攻击怪物
	 * @param _cid
	 * @param _heroId
	 * @param _monsterId
	 * @param _damge
	 * @param _context
	 * @return
	 */
	public GuildDungeonAttackResult cmdAttack(long _cid, long _heroId, long _monsterId, long _damge, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			GuildDungeonAttackResult result = new GuildDungeonAttackResult();
			
			//检查怪物数据
			GuildDungeonMonsterInfo monster = _m_mgrDungeonMonsterMgr.lookup(_monsterId);
			if(null == monster)
			{
				result.result.setCode(GuildErr.GUILD_DUNGEON_MONSTER_NOT_FOUND.getCode());
				return result;
			}
			if(monster.isKilled())
			{
				result.result.setCode(GuildErr.GUILD_DUNGEON_MONSTER_KILLED.getCode());
				return result;
			}
			
			//检查怪物之前的数据是否被击杀
			boolean isPreMonsterKilled = monster.getRef().pre_monster_id_list.isEmpty();
			for(long preMonsterId : monster.getRef().pre_monster_id_list)
			{
				GuildDungeonMonsterInfo preMonster = _m_mgrDungeonMonsterMgr.lookup(preMonsterId);
				if(null == preMonster)
					continue;
				
				if(preMonster.isKilled())
				{
					isPreMonsterKilled = true;
					break;
				}
			}
			if(!isPreMonsterKilled)
			{
				result.result.setCode(GuildErr.GUILD_DUNGEON_MONSTER_PRE_MONSTER_NOT_KILLED.getCode());
				return result;
			}
			
			//减少血量
			long realDamge = monster._reduceHp(_damge);
			if(realDamge <= 0)
			{
				result.result.setCode(GuildErr.GUILD_DUNGEON_ATTACK_FAIL.getCode());
				return result;
			}
			
			result.damge = realDamge;
			
			//后续处理
			if(monster.isKilled()) //怪物被击杀
			{
				//日志数据
				GuildDungeon_LogKill log = new GuildDungeon_LogKill();
				log.setCid(_cid);
				log.setMonsterId(_monsterId);

				_m_mgrDungeonLogMgr.addLog(EGuildDungeon_LogType.KILL, log);

				//记录运营数据日志
                try
                {
                    LogGuildDungeonMonsterKillBO logBo = new LogGuildDungeonMonsterKillBO();
                    logBo.setGuildId(getUSServer().getBM(), _m_giGuildInfo.getGuildId());
                    logBo.setGuildLevel(getUSServer().getBM(), _m_giGuildInfo.getLevel());
                    logBo.setDungeonId(getUSServer().getBM(), (int)getDungeonId());
                    logBo.setDungeonLevel(getUSServer().getBM(), getLvl());
                    logBo.setMonsterId(getUSServer().getBM(), monster.getRef().Id());
                    logBo.setType(getUSServer().getBM(), monster.getRef().monster_type.ordinal());
                    CommLogDB.log(getUSServer().getBM(), logBo, _context);
                } catch (Exception e)
                {
                    USLog.error(getUSServer(), "", e.getMessage());
                }

                result.isKilled = true;

				//副本是否完成
				if(isDone())
				{
					result.isDone = true;
				}
			}
			else //怪物数据变化
			{
				//日志数据
				GuildDungeon_LogAttack log = new GuildDungeon_LogAttack();
				log.setCid(_cid);
				log.setMonsterId(_monsterId);
				log.setDamage(_damge);
				
				_m_mgrDungeonLogMgr.addLog(EGuildDungeon_LogType.ATTACK, log);
			}

			//广播数据
			ALSynTaskManager.getInstance().regTask(()->
			{
				_m_giGuildInfo.getMemberMgr().broadcastMsg(US2GCWriter_037_GuildDungeonOp.make_052_OnDungeonMonsterChg(monster));
			});
			
			result.result.setCode(0);
			
			return result;
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
			
			sb.append("\nid:").append(getId()).append(", dungeon:").append(getDungeonId());
			sb.append(", lvl:").append(getLvl()).append(", start:").append(CommonFunc.getTimeStringMs(getBo().getStartMs()));
			
			sb.append("\n").append(_m_mgrDungeonMonsterMgr.toString());
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
