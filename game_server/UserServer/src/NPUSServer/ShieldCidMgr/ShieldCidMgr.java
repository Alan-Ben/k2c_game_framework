package NPUSServer.ShieldCidMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsShieldBO;

import java.util.HashMap;
import java.util.List;

public class ShieldCidMgr 
{
	//对应的US服务器实例对象
	private NPUserServer _m_usUserServer;
	
	//玩家屏蔽数据，key-玩家CID
	private HashMap<Long, ShieldCidInfo> _m_hmShieldCidMap;
	//锁对象
	private MutexObject _m_mutex;
	
	public ShieldCidMgr(NPUserServer _userServer)
	{
		_m_usUserServer = _userServer;
		
		_m_hmShieldCidMap = new HashMap<>();
		_m_mutex = new MutexObject();
	}
	
	public NPUserServer getUserServer() {return _m_usUserServer;}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public boolean initFromDB()
	{
		List<UsShieldBO> boList = getUserServer().getBM().getBM(UsShieldBO.class).s_findAll();
		if(null == boList)
		{
			USLog.error(getUserServer(), "Shield Cid Bo init fail.");
			return false;
		}
		
		for(int i = 0; i < boList.size(); i++)
		{
			UsShieldBO bo = boList.get(i);
			if(null == bo)
				continue;
			
			ShieldCidInfo shield = new ShieldCidInfo(_m_usUserServer, bo);
			_m_hmShieldCidMap.put(bo.getCid(), shield);
		}
		
		return true;
	}
	
	/**
	 * 查找指定玩家的屏蔽数据
	 * @param _cid
	 * @return
	 */
	public ShieldCidInfo lookup(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hmShieldCidMap.get(_cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取玩家屏蔽数据，如果不存在就重新构造
	 * @param _cid
	 * @return
	 */
	public ShieldCidInfo getShield(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hmShieldCidMap.computeIfAbsent(_cid, k -> new ShieldCidInfo(getUserServer(), _cid));
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除指定玩家数据
	 * @param _cid
	 */
	public void delCid(long _cid)
	{
		_lock();
		
		try
		{
			_m_hmShieldCidMap.remove(_cid);
			
			getUserServer().getBM().getBM(UsShieldBO.class).delAll("cid", _cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 清空全部屏蔽数据
	 */
	public void clear()
	{
		_lock();
		
		try
		{
			_m_hmShieldCidMap.clear();
			
			getUserServer().getBM().getBM(UsShieldBO.class).delAll();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查指定玩家是否屏蔽另一个指定玩家
	 * @param _checkCid
	 * @param _shieldCid
	 * @return
	 */
	public boolean checkShieldCid(long _checkCid, long _shieldCid)
	{
		ShieldCidInfo shield = lookup(_checkCid);
		if(null == shield)
			return false;
		
		return shield.hasShieldCid(_shieldCid);
	}
}
