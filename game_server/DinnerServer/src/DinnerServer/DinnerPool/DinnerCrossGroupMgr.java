package DinnerServer.DinnerPool;

import ALBasicServer.ALBasicMutex.MutexManager;

import java.util.HashMap;

/******
 * 跨服宴会分组管理器，管理所有宴会分组DinnerCrossGroup数据
 */
public class DinnerCrossGroupMgr
{
	private static DinnerCrossGroupMgr _g_instance = new DinnerCrossGroupMgr();
	public static DinnerCrossGroupMgr getInstance() {return _g_instance;}
	
	//宴会池Map，key-分组ID
	private HashMap<Long, DinnerCrossGroup> _m_hmDinnerCrossGroupMap;
	//锁对象
	private MutexManager _m_mutex;
	
	public DinnerCrossGroupMgr()
	{
		_m_hmDinnerCrossGroupMap = new HashMap<>();
		
		_m_mutex = new MutexManager();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	/**
	 * 查找指定跨服宴会分组
	 * @param _instanceId
	 * @return
	 */
	public DinnerCrossGroup lookup(long _instanceId)
	{
		_lock();
		
		try
		{
			return _m_hmDinnerCrossGroupMap.get(_instanceId);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 确保获取指定分组跨服宴会
	 * @param _instanceId
	 * @return
	 */
	public DinnerCrossGroup ensure(long _instanceId)
	{
		_lock();
		
		try
		{
			return _m_hmDinnerCrossGroupMap.computeIfAbsent(_instanceId, k -> new DinnerCrossGroup(k));
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 删除指定US的分组数据
	 * @param _groupId
	 * @param _usId
	 */
	public void discardByUs(long _groupId, int _usId)
	{
		_lock();
		
		try
		{
			DinnerCrossGroup group = _m_hmDinnerCrossGroupMap.get(_groupId);
			if(null == group)
				return;
			
			//移除指定US的所有宴会分组数据
			group.removeDinnerByUs(_usId);
		}
		finally
		{
			_unlock();
		}
	}
}

