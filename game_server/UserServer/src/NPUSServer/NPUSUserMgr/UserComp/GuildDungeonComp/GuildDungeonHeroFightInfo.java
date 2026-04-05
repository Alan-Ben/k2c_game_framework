package NPUSServer.NPUSUserMgr.UserComp.GuildDungeonComp;

import Common.GuildDungeonObj.GuildDungeon_FightHero;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;
import USDB.Bo.PlayerGuildDungeonHeroBO;

/**
 * 大臣出战数据
 * @author mj
 *
 */
public class GuildDungeonHeroFightInfo 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	//大臣出战数据
	private PlayerGuildDungeonHeroBO _m_bo;
	//大臣下次恢复时间
	private long _m_lNextAutoRecoverMs;
	
	public GuildDungeonHeroFightInfo(NPUSUserData _userData, PlayerGuildDungeonHeroBO _bo)
	{
		_m_udUserData = _userData;
		
		_m_bo = _bo;
		
		//有发起攻击记录，则需要计算下次重置时间
		if(_m_bo.getLastFightedMs() > 0)
		{
			_m_lNextAutoRecoverMs = RefGeneral.Ref().guild_dungeon_auto_settle_time.getNextFreshTimeTagMS(_m_bo.getLastFightedMs());
		}
		
		//刷新数据
		_refresh(false);
	}

    public NPUSUserData getUserData() {return _m_udUserData;}
	public PlayerGuildDungeonHeroBO getBo() {return _m_bo;}
	
	public long getHeroId() {return _m_bo.getHeroId();}
	public int getFightedCount() {return _m_bo.getFightedCount();}
	public int getRecoveredCount() {return _m_bo.getRecoveredCount();}
	public long getLastFightedMs() {return _m_bo.getLastFightedMs();}
	
	public GuildDungeon_FightHero toProto()
	{
		GuildDungeon_FightHero proto = new GuildDungeon_FightHero();
		proto.setHeroId(getHeroId());
		proto.setFightedCount(getFightedCount());
		proto.setRecoveredCount(getRecoveredCount());
		proto.setLastFightedMs(getLastFightedMs());
		
		return proto;
	}
	
	/**
	 * 刷新大臣出战数据
	 */
	protected void _refresh(boolean _push) 
	{
		//尚未有攻击次数
		if(getFightedCount() <= 0)
			return;
		
		long nowTimeMs = CommonFunc.getNowTimeMS();
		//下次恢复时间尚未到达，无需处理
		if(_m_lNextAutoRecoverMs > nowTimeMs)
			return;
		
		_m_bo.setLastFightedMs(getUserData().getUSServer().getBM(), nowTimeMs);
		//重置次数
		_m_bo.setFightedCount(getUserData().getUSServer().getBM(), 0);
		_m_bo.setRecoveredCount(getUserData().getUSServer().getBM(), 0);
		_m_bo.saveAllMarked(getUserData().getUSServer().getBM());
		//计算下次恢复时间
		_m_lNextAutoRecoverMs = RefGeneral.Ref().guild_dungeon_auto_settle_time.getNextFreshTimeTagMS(getLastFightedMs());
		
		//推送数据
		if(_push)
		{
    		//推送数据
    		getUserData().sendMsgToGC(US2GCWriter_037_GuildDungeonOp.make_054_OnDungeonHeroFightChg(this));
		}
	}
	
	/**
	 * 检查是否可以出战
	 * @return
	 */
	protected boolean _canAttack() 
	{
		_refresh(false);
		
		return (getFightedCount() - getRecoveredCount() <= 0);
	}
	
	/**
	 * 设置攻击次数
	 * @param _count
	 */
	protected void _setFightedCount(int _count) 
	{
		//避免出现负数情况
		int count = Math.max(_count, 0);
		
		_m_bo.setFightedCount(getUserData().getUSServer().getBM(), count);
		_m_bo.setLastFightedMs(getUserData().getUSServer().getBM(), CommonFunc.getNowTimeMS());
		_m_bo.saveAll(getUserData().getUSServer().getBM());
	}
	
	/**
	 * 增加攻击次数
	 */
	protected void _incrFightedCount()
	{
		_setFightedCount(getFightedCount() + 1);
		//计算下次恢复时间
		_m_lNextAutoRecoverMs = RefGeneral.Ref().guild_dungeon_auto_settle_time.getNextFreshTimeTagMS(getLastFightedMs());
	}
	
	/**
	 * 设置攻击次数
	 * @param _count
	 */
	protected void _setRecoveredCount(int _count) 
	{
		_m_bo.saveRecoveredCount(getUserData().getUSServer().getBM(), _count);
	}
}
