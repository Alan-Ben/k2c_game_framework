package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.GuildDungeonEnum.EGuildDungeon_LogType;
import Common.GuildDungeonEnum.EGuildDungeon_StartType;
import Common.GuildDungeonObj.GuildDungeon_LogAutoStart;
import Common.GuildDungeonObj.GuildDungeon_LogStart;
import Common.GuildDungeonObj.GuildDungeon_Monster;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeon;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

/**
 * 公会副本管理对象
 * @author mj
 *
 */
public class GuildDungeonMgr 
{
	//公会数据
	private GuildInfo _m_giGuildInfo;
	//副本全局设置数据
	private GuildDungeonGlobalSetInfo _m_diDungeonGlobalSetInfo;
	//副本设置数据管理
	private GuildDungeonSetMgr _m_alDungeonSetMgr;
	//副本实例数据管理
	private GuildDungeonInstanceMgr _m_mgrDungeonInstanceMgr;
	//攻击记录管理对象
	private GuildDungeonDamageRank _m_mgrDungeonDamageRank;
	
	public GuildDungeonMgr(GuildInfo _guildInfo)
	{
		_m_giGuildInfo = _guildInfo;
		
		_m_diDungeonGlobalSetInfo = new GuildDungeonGlobalSetInfo(_m_giGuildInfo);
		_m_alDungeonSetMgr = new GuildDungeonSetMgr(_m_giGuildInfo);
		_m_mgrDungeonInstanceMgr = new GuildDungeonInstanceMgr(_m_giGuildInfo);
		_m_mgrDungeonDamageRank = new GuildDungeonDamageRank(_m_giGuildInfo);
	}
	
	public GuildInfo getGuild() {return _m_giGuildInfo;}
	public NPUserServer getUSServer() {return _m_giGuildInfo.getGuildMgr().getServer();}

	public GuildDungeonGlobalSetInfo getGlobalSetInfo() {return _m_diDungeonGlobalSetInfo;}
	public GuildDungeonSetMgr getSetMgr() {return _m_alDungeonSetMgr;}
	public GuildDungeonInstanceMgr getInstanceMgr() {return _m_mgrDungeonInstanceMgr;}
	public GuildDungeonDamageRank getDamageRank() {return _m_mgrDungeonDamageRank;}
	
	/**
	 * 加载完成后处理
	 */
	public void _onInited()
	{
		_m_diDungeonGlobalSetInfo._onInited();
		_m_alDungeonSetMgr._onInited();
		_m_mgrDungeonInstanceMgr._onInited();
		_m_mgrDungeonDamageRank._onInited();
	}
	
	/**
	 * 结算所有公会副本
	 * @param _context
	 */
	public void settleAll(NPPlayerContext _context)
	{
		//结算所有副本
		List<Long> memberCidList = _m_giGuildInfo.getMemberMgr().getMemberCidList(null);
		_m_mgrDungeonInstanceMgr._settle(memberCidList, _context);
		
		//销毁数据
		_m_mgrDungeonInstanceMgr._discard();
		_m_mgrDungeonDamageRank._discard();
	}
	
	/**
	 * 每秒tick操作
	 * @param _nowTimeMs
	 */
	public void tick(long _nowTimeMs)
	{
		//公会销毁
		if(_m_giGuildInfo.isDissolve())
			return;
		
		//进行自动结算
		if(_m_diDungeonGlobalSetInfo.canAutoSettle(_nowTimeMs) && _m_mgrDungeonInstanceMgr.getCount() > 0)
		{
			NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_AUTO_SETTLE);
			
			settleAll(context);
		}
		
		//开启自动开启
		if(_m_diDungeonGlobalSetInfo.canAutoStart(_nowTimeMs))
		{
			//对所有副本配置进行检查，避免重复生成大量列表
			List<RefGuildDungeon> dungeonRefList = RefGuildDungeon.getMgr().getList();
			for(int i = 0; i < dungeonRefList.size(); i++)
			{
				RefGuildDungeon ref = dungeonRefList.get(i);
				if(null == ref)
					continue;
			
				//是否自动开启
				GuildDungeonSetInfo setInfo = _m_alDungeonSetMgr.lookup(ref.id);
				if(null == setInfo || !setInfo.isAutoStart())
					continue;
				
				//已经开启的实例
				if(_m_mgrDungeonInstanceMgr.hasDungeon(ref.id))
					continue;
				
				//生成怪物数据
				ArrayList<GuildDungeon_Monster> monsterList = setInfo.buildMonsterList();
				if(null == monsterList || monsterList.isEmpty())
				{
					USLog.error(getUSServer(), "guild:{} dungeon:{} start fail, not get monster.", getGuild().getGuildId(), setInfo.getDungeonId());
					continue;
				}
				
				NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_AUTO_START);
				
				//检查联盟财富消耗
				long startCost = setInfo.getLvlRef().star_cost_guild_wealth;
				if(!_m_giGuildInfo.spendWealth(startCost, context))
					continue;
				
				//构造实例
				GuildDungeonInstanceInfo instance = _m_mgrDungeonInstanceMgr.start(setInfo.getRef(), setInfo.getLvlRef(), monsterList, context);
				if(null == instance)
				{
					USLog.error(getUSServer(), "guild:{} dungeon:{} start fail.", getGuild().getGuildId(), setInfo.getDungeonId());
					continue;
				}

				//推送数据
				_m_giGuildInfo.getMemberMgr().broadcastMsg(US2GCWriter_037_GuildDungeonOp.make_051_OnDungeonInstanceChg(instance));
			
				//公会副本日志
				GuildDungeon_LogAutoStart dungeonLog = new GuildDungeon_LogAutoStart();
				dungeonLog.setDungeonId(ref.id);
				dungeonLog.setCostValue(startCost);
				
				instance.getLogMgr().addLog(EGuildDungeon_LogType.AUTO_START, dungeonLog);
			}
		}
	}
	
	/**
	 * 销毁数据
	 */
	public void discard()
	{
		_m_alDungeonSetMgr._discard();
		_m_mgrDungeonInstanceMgr._discard();
		_m_mgrDungeonDamageRank._discard();
	}
	
	/**
	 * 开启公会副本
	 * @param _startType
	 * @param _dungeonId
	 * @param _context
	 * @return
	 */
	public Result cmdStart(long _cid, EGuildDungeon_StartType _startType, long _dungeonId, NPPlayerContext _context)
	{
		//检查副本配置数据
		GuildDungeonSetInfo setInfo = _m_alDungeonSetMgr.lookup(_dungeonId);
		if(null == setInfo)
		{
			return GuildErr.GUILD_DUNGEON_NOT_FOUND;
		}
		if(!setInfo.isUnlock())
		{
			return GuildErr.GUILD_DUNGEON_NOT_UNLOCK;
		}

		//检查是否还有正在运行的副本
		if(_m_mgrDungeonInstanceMgr.hasDungeon(_dungeonId))
		{
			return GuildErr.GUILD_DUNGEON_STARTED;
		}
		
		//检查对应的怪物数据
		ArrayList<GuildDungeon_Monster> monsterList = setInfo.buildMonsterList();
		if(null == monsterList || monsterList.isEmpty())
		{
			return GuildErr.GUILD_DUNGEON_ERR;
		}
		
		//检查消耗
		long startCost = 0;
        //扣除对应的物品
        if(EGuildDungeon_StartType.ALLIANCE_WEALTH == _startType)
        {
        	startCost = setInfo.getLvlRef().star_cost_guild_wealth;

        	if(setInfo.getLvlRef().star_cost_guild_wealth > _m_giGuildInfo.getWealth())
        		return CommErr.ITEM_NOT_ENOUGH;
        	
        	if(!_m_giGuildInfo.spendWealth(setInfo.getLvlRef().star_cost_guild_wealth, _context))
        		return CommErr.CONSUME_FAIL;
        }
        else if(EGuildDungeon_StartType.COMMON_ITEM == _startType)
        {
        	startCost = setInfo.getLvlRef().star_cost_item.getCount();
			//物品消耗不在这里处理，在US已经做了处理
        }
        else
        {
        	return CommErr.PARAM_ERROR;
        }
		
		//开启副本
		GuildDungeonInstanceInfo instance = _m_mgrDungeonInstanceMgr.start(setInfo.getRef(), setInfo.getLvlRef(), monsterList, _context);
		if(null == instance)
		{
			//更新失败，退回花费
			NPPlayerContext returnContext = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_START_FAIL);
			returnContext.setGuid(_context.getGuid());
			if(EGuildDungeon_StartType.ALLIANCE_WEALTH == _startType)
	        {
				_m_giGuildInfo.gainWealth(setInfo.getLvlRef().star_cost_guild_wealth, returnContext);
	        }
			else if(EGuildDungeon_StartType.COMMON_ITEM == _startType)
			{
				//物品消耗不在这里处理，在US处理
			}
			
			return GuildErr.GUILD_DUNGEON_START_FAIL;
		}

		//推送数据
		ALSynTaskManager.getInstance().regTask(()->
		{
			_m_giGuildInfo.getMemberMgr().broadcastMsg(US2GCWriter_037_GuildDungeonOp.make_051_OnDungeonInstanceChg(instance));
		});
		
		//公会副本日志
		GuildDungeon_LogStart dungeonLog = new GuildDungeon_LogStart();
		dungeonLog.setCid(_cid);
		dungeonLog.setDungeonId(_dungeonId);
		dungeonLog.setStartType(_startType);
		dungeonLog.setStartCost(startCost);
		
		instance.getLogMgr().addLog(EGuildDungeon_LogType.START, dungeonLog);
		
		return Result.SUCC;
	}
}
