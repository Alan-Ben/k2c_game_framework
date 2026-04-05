package NPUSServer.Guild.GuildDungeon;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import USDB.Bo.GuildDungeonGlobalSetBO;

/**
 * 公会副本全局配置数据
 * @author mj
 *
 */
public class GuildDungeonGlobalSetInfo 
{
	//公会数据
	private GuildInfo _m_giGuildInfo;
	
	//数据实例ID
	private long _m_lId;
	
	//自动开启时间：小时
	private int _m_iAutoHour;
	//自动开启时间：分钟
	private int _m_iAutoMin;
	
	//上次自动开启时间（毫秒）
	private long _m_lLastAutoStartedMs;
	//副本自动开启时间
	private long _m_lNextAutoStartMs;
	
	//上次自动结算时间（毫秒）
	private long _m_lLastAutoSettledMs;
	//副本自动结算时间
	private long _m_lNextAutoSettleMs;
	
	//锁对象
    private MutexAtom _m_mutex;
    
	public GuildDungeonGlobalSetInfo(GuildInfo _guildInfo)
	{
		_m_giGuildInfo = _guildInfo;
		
		_m_mutex = new MutexAtom();
	}
	
	protected void _lock() {_m_mutex.lock();}
	protected void _unlock() {_m_mutex.unlock();}

	public GuildInfo getGuild() {return _m_giGuildInfo;}
	public NPUserServer getUSServer() {return _m_giGuildInfo.getGuildMgr().getServer();}
	
	public long getId() {return _m_lId;}
	
	public int getAutoHour() {return _m_iAutoHour;}
	public int getAutoMin() {return _m_iAutoMin;}
	
	public long getLastAutoStartedMs() {return _m_lLastAutoStartedMs;}
	public long getNextAutoStartMs() {return _m_lNextAutoStartMs;}
	public void setNextAutoStartMs(long _value) {_m_lNextAutoStartMs = _value;}
	
	public long getLastAutoSettledMs() {return _m_lLastAutoSettledMs;}
	public long getNextAutoSettleMs() {return _m_lNextAutoSettleMs;}
	public void setNextAutoSettleMs(long _value) {_m_lNextAutoSettleMs = _value;}
	
	/**
	 * 加载数据
	 * @param _bo
	 */
	public void _loadBo(GuildDungeonGlobalSetBO _bo)
	{
		_m_lId = _bo.getId();
		
		_m_iAutoHour = _bo.getAutoHour();
		_m_iAutoMin = _bo.getAutoMin();
		
		_m_lLastAutoStartedMs = _bo.getLastAutoStartedMs();
		_m_lLastAutoSettledMs = _bo.getLastAutoSettledMs();
	}
	
	public void _onInited()
	{
		//开启时间，有设置开启时间才需要计算下次自动开启时间
		if(_m_lLastAutoStartedMs > 0)
		{
			//计算下次自动开启时间
			_m_lNextAutoStartMs = CommonFunc.getNextAssignTimeMs(_m_lLastAutoStartedMs, getAutoHour(), getAutoMin());
		}
		
		//结算时间
		if(_m_lLastAutoSettledMs <= 0)
		{
			_m_lLastAutoSettledMs = CommonFunc.getNowTimeMS();
		}
		//计算下次自动开启时间
		_m_lNextAutoSettleMs = RefGeneral.Ref().guild_dungeon_auto_settle_time.getNextFreshTimeTagMS(_m_lLastAutoSettledMs);
	}
	
	/**
	 * 保存数据
	 */
	private void _save()
	{
		BM bmObj = getUSServer().getBM();
		
		if(_m_lId == 0)
		{
			GuildDungeonGlobalSetBO bo = new GuildDungeonGlobalSetBO();
			bo.setGuildId(bmObj, getGuild().getGuildId());
			bo.setAutoHour(bmObj, _m_iAutoHour);
			bo.setAutoMin(bmObj, _m_iAutoMin);
			bo.setLastAutoStartedMs(bmObj, _m_lLastAutoStartedMs);
			bo.setLastAutoSettledMs(bmObj, _m_lLastAutoSettledMs);
			bo.insert(bmObj);
			
			_m_lId = bo.getId();
		}
		else
		{
	        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
	        updateValue.addValueObj("autoHour", _m_iAutoHour);
	        updateValue.addValueObj("autoMin", _m_iAutoMin);
	        updateValue.addValueObj("lastAutoStartedMs", _m_lLastAutoStartedMs);
	        updateValue.addValueObj("lastAutoSettledMs", _m_lLastAutoSettledMs);
	        
	        bmObj.getBM(GuildDungeonGlobalSetBO.class).update("id", _m_lId, updateValue);
		}
	}
	
	/**
	 * 更新下次刷新时间
	 */
	public void refreshNextAutoStartTime()
	{
		_lock();
		
		try
		{
			//更新上次计算时间
			_m_lLastAutoStartedMs = CommonFunc.getNowTimeMS();
			
			_save();
			
			//设置下次自动开启时间
			_m_lNextAutoStartMs = CommonFunc.getNextAssignTimeMs(_m_lLastAutoStartedMs, _m_iAutoHour, _m_iAutoMin);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 更新下次结算时间
	 */
	public void refreshNextSettleTime()
	{
		_lock();
		
		try
		{
			//更新上次计算时间
			_m_lLastAutoSettledMs = CommonFunc.getNowTimeMS();
			
			_save();
			
			//设置下次自动开启时间
			_m_lNextAutoSettleMs = RefGeneral.Ref().guild_dungeon_auto_settle_time.getNextFreshTimeTagMS(_m_lLastAutoSettledMs);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 是否可以自动开启
	 * @param _nowTimeMs
	 * @return
	 */
	public boolean canAutoStart(long _nowTimeMs)
	{
		_lock();
		
		try
		{
			//公会已经销毁
			if(_m_giGuildInfo.isDissolve())
				return false;
			
			//尚未设置数据
			if(_m_lLastAutoStartedMs <= 0)
				return false;
			
			//尚未到自动开启时间
			if(_m_lNextAutoStartMs > _nowTimeMs)
				return false;
			
			//更新最后一次时间
			_m_lLastAutoStartedMs = CommonFunc.getNowTimeMS();
			_save();
			
			//重新计算下一次自动开启时间
			_m_lNextAutoStartMs = CommonFunc.getNextAssignTimeMs(_m_lLastAutoStartedMs, getAutoHour(), getAutoMin()); 
			
			return true;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 是否可以自动结算
	 * @param _nowTimeMs
	 * @return
	 */
	public boolean canAutoSettle(long _nowTimeMs)
	{
		_lock();
		
		try
		{
			//公会已经销毁
			if(_m_giGuildInfo.isDissolve())
				return false;
			
			//尚未到自动开启时间
			if(_m_lNextAutoSettleMs > _nowTimeMs)
				return false;
			
			//更新最后一次时间
			_m_lLastAutoSettledMs = CommonFunc.getNowTimeMS();
			_save();
			
			//重新计算下一次自动开启时间
			_m_lNextAutoSettleMs = RefGeneral.Ref().guild_dungeon_auto_settle_time.getNextFreshTimeTagMS(_m_lLastAutoSettledMs);
			
			return true;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置自动启动
	 * @param _hour
	 * @param _min
	 */
	public void cmdSetAutoStartTime(int _hour, int _min)
	{
		_lock();
		
		try
		{
			//更新自动开启数据
			_m_iAutoHour = _hour;
			_m_iAutoMin = _min;
			_m_lLastAutoStartedMs = CommonFunc.getNowTimeMS();
			
			_save();
			
			//设置下次自动开启时间
			_m_lNextAutoStartMs = CommonFunc.getNextAssignTimeMs(_m_lLastAutoStartedMs, _m_iAutoHour, _m_iAutoMin);
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
			getUSServer().getBM().getBM(GuildDungeonGlobalSetBO.class).delAll("guildId", getGuild().getGuildId());
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
			
			sb.append("\nauto time:").append(_m_iAutoHour).append(" - ").append(_m_iAutoMin);
			
			if(_m_lLastAutoStartedMs > 0)
				sb.append("\nlast start:").append(CommonFunc.getTimeStringMs(_m_lLastAutoStartedMs));
			else
				sb.append("\nlast start:").append(0);
			
			if(_m_lLastAutoSettledMs > 0)
				sb.append("\nlast settle:").append(CommonFunc.getTimeStringMs(_m_lLastAutoSettledMs));
			else
				sb.append("\nlast settle:").append(0);
			
			if(_m_lNextAutoStartMs > 0)
				sb.append("\nnext start:").append(CommonFunc.getTimeStringMs(_m_lNextAutoStartMs));
			else
				sb.append("\nnext start:").append(0);

			if(_m_lNextAutoSettleMs > 0)
				sb.append("\nnext settle:").append(CommonFunc.getTimeStringMs(_m_lNextAutoSettleMs));
			else
				sb.append("\nnext settle:").append(0);
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
