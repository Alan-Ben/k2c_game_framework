package NPUSServer.UserCounterMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerCounterBO;

import java.util.HashMap;
import java.util.List;

/**
 * 玩家计数管理
 * @author mj
 *
 */
public class UserCounterMgr 
{
	private NPUserServer _m_usUSServer;
	//玩家计数Map，key-玩家CID
	private HashMap<Long, UserCounterInfo> _m_hmUserCounterMap;
	//锁对象
	private MutexObject _m_mutex;
	
	public UserCounterMgr(NPUserServer _usServer)
	{
		_m_usUSServer = _usServer;

		_m_hmUserCounterMap = new HashMap<>();
		_m_mutex = new MutexObject();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}

	public NPUserServer getUSServer() {return _m_usUSServer;}
	
	public boolean initFromDB()
	{
		boolean checkBoCounter = false;
		List<PlayerCounterBO> boList = getUSServer().getBM().getBM(PlayerCounterBO.class).s_findAll();
		if(null == boList)
		{
			USLog.error(_m_usUSServer, "UserCounterMgr init bo list fail.");
			return false;
		}
		for(int i = 0; i < boList.size(); i++)
		{
			PlayerCounterBO bo = boList.get(i);
			if(null == bo)
				continue;
			
			//只需要首次检查一次就可以，EPlayerCounterEnum枚举 >= PlayerCounterBO.counter.length
			if(!checkBoCounter)
			{
				//数据表counter字段长度小于枚举EPlayerCounterEnum长度
				if(bo.getCounterSize() < EPlayerCounterEnum.EPlayerCounterEnum_Length)
				{
					USLog.error(_m_usUSServer, "UserCounterMgr init counter length:{} < EPlayerCounterEnum.length:{}  error."
							, bo.getCounterSize(), EPlayerCounterEnum.EPlayerCounterEnum_Length);
					return false;
				}
				
				checkBoCounter = true;
			}
			
			UserCounterInfo counter = new UserCounterInfo(getUSServer(), bo);
			_m_hmUserCounterMap.put(counter.getCid(), counter);
		}
		
		return true;
	}
	
	/**
	 * 构造玩家计数对象
	 * @param _cid
	 * @return
	 */
	public UserCounterInfo ensure(long _cid)
	{
		_lock();
		
		try
		{
			UserCounterInfo counter = _m_hmUserCounterMap.get(_cid);
			if(null == counter)
			{
				PlayerCounterBO bo = new PlayerCounterBO();
				bo.setCid(getUSServer().getBM(), _cid);
				bo.insert(getUSServer().getBM());
				
				counter = new UserCounterInfo(getUSServer(), bo);
				_m_hmUserCounterMap.put(counter.getCid(), counter);
			}
			
			return counter;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取玩家指定类型的计数
	 * @param _cid
	 * @param _counterType
	 * @return
	 */
	public long getPlayerCounter(long _cid, EPlayerCounterEnum _counterType)
	{
		_lock();
		
		try
		{
			UserCounterInfo counter = _m_hmUserCounterMap.get(_cid);
			if(null == counter)
				return 0;
			
			return counter.getCounter(_counterType);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 设置玩家计数，对于 尚未创建的玩家计数 && counter-0 的数据不予创建
	 * @param _cid
	 * @param _counterType
	 * @param _counter
	 */
	public void setPlayerCounter(long _cid, EPlayerCounterEnum _counterType, long _counter)
	{
		_lock();
		
		try
		{
			UserCounterInfo counter = _m_hmUserCounterMap.get(_cid);
			if(null == counter && _counter == 0)
				return;
			
			ensure(_cid).setCounter(_counterType, _counter);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 指定玩家指定类型计数+1
	 * @param _cid
	 * @param _counterType
	 */
	public void incrPlayerCounter(long _cid, EPlayerCounterEnum _counterType)
	{
		_lock();
		
		try
		{
			UserCounterInfo counter = _m_hmUserCounterMap.get(_cid);
			if(null == counter)
				return;
			
			ensure(_cid).incrtCounter(_counterType);
		}
		finally
		{
			_unlock();
		}
	}
}
