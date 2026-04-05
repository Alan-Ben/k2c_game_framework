package NPUSServer.Guild.GuildDungeon;

import Common.Common_LongList;
import Common.GuildDungeonEnum.EGuildDungeon_MonsterType;
import Common.GuildDungeonObj.GuildDungeon_DungeonMonster;
import Common.GuildDungeonObj.GuildDungeon_Monster;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.GuildDungeon.GuildDungeonMonsterKillReward;
import NPGameRes.Refs.GuildDungeon.RefGuildDungeonMonster;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GuildDungeonMonsterBO;

import java.nio.ByteBuffer;
import java.util.HashSet;

public class GuildDungeonMonsterInfo 
{
	//公会数据
	private GuildDungeonInstanceInfo _m_diDungeonInfo;
	
	//数据bo
	private GuildDungeonMonsterBO _m_bo;
	//已经领取奖励的玩家CID列表
	private HashSet<Long> _m_hsGainedCidSet;
	
	//怪物配置数据
	private RefGuildDungeonMonster _m_ref;
	
	public GuildDungeonMonsterInfo(GuildDungeonInstanceInfo _dungeon, GuildDungeonMonsterBO _bo)
	{
		_m_diDungeonInfo = _dungeon;
		
		_m_bo = _bo;
		
		_m_hsGainedCidSet = new HashSet<>();
		
		_init();
	}
	
	private void _init()
	{
		_m_ref = RefGuildDungeonMonster.getMgr().get(_m_bo.getMonsterId());
		if(null == _m_ref)
		{
			USLog.error(_m_diDungeonInfo.getGuild().getGuildMgr().getServer(), "guild:{} dungeon:{} monster:{} ref not found.", 
					_m_diDungeonInfo.getGuildId(), _m_diDungeonInfo.getDungeonId(), _m_bo.getMonsterId());
		}
		
		if(null != _m_bo.getGainedCidList())
		{
			ByteBuffer buff = ByteBuffer.wrap(_m_bo.getGainedCidList());
			Common_LongList listObj = new Common_LongList();
			listObj.readPackage(buff);
			
			_m_hsGainedCidSet.addAll(listObj.getValueList());
		}
	}
	
	public GuildDungeonInstanceInfo getDungeon() {return _m_diDungeonInfo;}
	public NPUserServer getUSServer() {return _m_diDungeonInfo.getGuild().getGuildMgr().getServer();}
	public boolean isSettled() {return _m_diDungeonInfo.isSettled();}
	
	public GuildDungeonMonsterBO getBo() {return _m_bo;}
	public RefGuildDungeonMonster getRef() {return _m_ref;}

	public long getMonsterId() {return _m_bo.getMonsterId();}
	public long getHp() {return _m_bo.getHp();}
	public boolean isTag() {return _m_bo.getIsTag();}

	/**
	 * boss怪物 或 有带奖励的其他怪物
	 * @return
	 */
	public boolean isReward() 
	{
		return isBoss() || _m_bo.getIsReward();
	}
	
	public EGuildDungeon_MonsterType getMonsterType() {return null == _m_ref ? EGuildDungeon_MonsterType.NONE : _m_ref.monster_type;}
	
	public boolean isBoss()
	{
		return null != _m_ref ? _m_ref.monster_type == EGuildDungeon_MonsterType.BOSS : false;
	}
	
	public boolean isKilled()
	{
		return null != _m_ref && getHp() <= 0;
	}
	
	public boolean canAttack()
	{
		return null != _m_ref && getHp() > 0;
	}
	
	/**
	 * 已领取奖励判断
	 * @param _cid
	 * @return
	 */
	protected boolean _isGainedReward(long _cid) 
	{
		return _m_hsGainedCidSet.contains(_cid);
	}
	
	/**
	 * 指定玩家是否可以领取奖励
	 * @param _cid
	 * @return
	 */
	protected boolean _canReward(long _cid) 
	{
		return isKilled() && isReward() && !_isGainedReward(_cid);
	}

	/**
	 * 指定玩家是否可以发送结算奖励
	 * @param _cid
	 * @return
	 */
	protected boolean _canSettled(long _cid) 
	{
		return isSettled() && isKilled() && isReward() && !_isGainedReward(_cid);
	}
	
	/**
	 * 计算获取的的物品
	 * @param _cid
	 * @return
	 */
	protected NPCommonCostItem _gainReward(long _cid) 
	{
		if(!_canReward(_cid))
			return null;
		
		if(!_addGainedCid(_cid))
			return null;
		
		if(isBoss())
		{
			return _m_diDungeonInfo.getRef().kill_boss_reward;
		}
		else
		{
			GuildDungeonMonsterKillReward reward = _m_diDungeonInfo.getRef().kill_reward.lookReward(getMonsterType());
			if(null == reward)
			{
				USLog.error(getUSServer(), "guild:{} dungeon:{} instanceId:{} monster:{} type:{} not get kill item.", 
						getDungeon().getGuildId(), getDungeon().getDungeonId(), getDungeon().getId(), getMonsterId(), getMonsterType());
				return null;
			}
			
			return reward.getGainItem();
		}		
	}
	
	/**
	 * 副本怪物数据协议
	 * @return
	 */
	public GuildDungeon_Monster toProto()
	{
		GuildDungeon_Monster proto = new GuildDungeon_Monster();
		proto.setMonsterId(getMonsterId());
		proto.setHp(getHp());
		proto.setIsReward(isReward());
		
		return proto;
	}

	/**
	 * 副本怪物关系数据协议
	 * @return
	 */
	public GuildDungeon_DungeonMonster toDungeonMonsterProto()
	{
		GuildDungeon_DungeonMonster proto = new GuildDungeon_DungeonMonster();
		proto.setId(_m_diDungeonInfo.getId());
		proto.setMonsterId(getMonsterId());
		
		return proto;
	}
	
	/**
	 * 增加已领取玩家CID
	 * @param _cid
	 */
	protected boolean _addGainedCid(long _cid) 
	{
		if(!_m_hsGainedCidSet.add(_cid))
			return false;
		
		Common_LongList listObj = new Common_LongList();
		listObj.getValueList().addAll(_m_hsGainedCidSet);
		_m_bo.saveGainedCidList(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(listObj.makePackage()));
		
		return true;
	}
	
	/**
	 * 降低血量
	 * @param _value
	 */
	protected long _reduceHp(long _value) 
	{
		long oriHp = getHp();
		long curHp = (int) Math.max(getHp() - _value, 0);
		
		_m_bo.saveHp(getUSServer().getBM(), curHp);
		
		return oriHp - getHp();
	}
	
	/**
	 * 设置标签
	 * @param _isTag
	 */
	protected void _setTag(boolean _isTag) 
	{
		_m_bo.saveIsTag(getUSServer().getBM(), _isTag);
	}
}
