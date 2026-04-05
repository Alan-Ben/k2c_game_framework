package MarryMatchServer.MarryMatchMgr;

import ALBasicServer.ALBasicMutex.MutexObject;

import java.util.HashMap;

public class MarryMatchMgr 
{
	private static MarryMatchMgr _g_instance = new MarryMatchMgr();
	public static MarryMatchMgr getInstance() {return _g_instance;}
	
	//匹配分组Map，key-GroupId
	private HashMap<Long, MarryMatchGroup> _m_hmMarryGroupMap;
	
	private MutexObject _m_mutex;
	
	public MarryMatchMgr()
	{
		_m_hmMarryGroupMap = new HashMap<>();
		
		_m_mutex = new MutexObject();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public boolean init()
	{
		return true;
	}
	
	/**
	 * 查找分组
	 * @param _groupId
	 * @return
	 */
	public MarryMatchGroup lookup(long _groupId)
	{
		_lock();
		
		try
		{
			return _m_hmMarryGroupMap.get(_groupId);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造分组数据对象
	 * @param _groupId
	 * @return
	 */
	public MarryMatchGroup ensure(long _groupId)
	{
		_lock();
		
		try
		{
			return _m_hmMarryGroupMap.computeIfAbsent(_groupId, k -> new MarryMatchGroup(k));
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 解散分组
	 * @param _groupId
	 */
	public void discard(long _groupId)
	{
		_lock();
		
		try
		{
			//迁移源分组数据
			MarryMatchGroup discardGroup = _m_hmMarryGroupMap.remove(_groupId);
			if(null == discardGroup)
				return;
			
			discardGroup.discard();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 检查是否可以销毁分组数据，如果匹配数据已经为空，则销毁该分组
	 * @param _groupId
	 */
	public void checkDiscard(long _groupId)
	{
		_lock();
		
		try
		{
			MarryMatchGroup group = _m_hmMarryGroupMap.get(_groupId);
			
			if(null != group && group.checkEmpty())
			{
				_m_hmMarryGroupMap.remove(_groupId);
				
				group.discard();
			}
		}
		finally
		{
			_unlock();
		}
	}
}
