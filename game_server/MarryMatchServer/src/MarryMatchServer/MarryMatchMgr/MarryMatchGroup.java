package MarryMatchServer.MarryMatchMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

public class MarryMatchGroup 
{
	//分组实例ID
	private long _m_lGroupId;
	
	//当前分组的US集合
	private HashSet<Integer> _m_hsUsIdSet;
	
	//匹配池子嗣Map，key-子嗣实例ID
	private HashMap<Long, MarryMatchItem> _m_hmMatchItemMap;
	//匹配池子嗣List
	private ArrayList<MarryMatchItem> _m_alMatchItemList;
	//获取可以匹配的列表，key-匹配ID
	private HashMap<Integer, ArrayList<MarryMatchItem>> _m_hmMatchItemListMap;
	
	//锁对象
	private MutexAtom _m_mutex;
	
	public MarryMatchGroup(long _groupId)
	{
		_m_lGroupId = _groupId;
		
		_m_hsUsIdSet = new HashSet<>();
		
		_m_hmMatchItemMap = new HashMap<>();
		_m_alMatchItemList = new ArrayList<>();
		_m_hmMatchItemListMap = new HashMap<>();
		
		_m_mutex = new MutexAtom();
	}
	
	public long getGroupId() {return _m_lGroupId;}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	/**
	 * 检查是否匹配数据清空
	 * @return
	 */
	public boolean checkEmpty()
	{
		_lock();
		
		try
		{
			return _m_hsUsIdSet.isEmpty();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取全部匹配子嗣数据
	 * @return
	 */
	public ArrayList<MarryMatchItem> getAllItemList()
	{
		_lock();
		
		try
		{
			return new ArrayList<>(_m_alMatchItemList);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取指定的子嗣数据
	 * @param _applyAdultId
	 * @return
	 */
	public MarryMatchItem lookup(long _applyAdultId)
	{
		_lock();
		
		try
		{
			return _m_hmMatchItemMap.get(_applyAdultId);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 新增匹配池数据
	 * @param _adultApply
	 */
	public void addItem(ServerObj_AdultMarryGroupApplyInfo _adultApply)
	{
		_lock();
		
		try
		{
			//已经存在，不再重复处理
			if(_m_hmMatchItemMap.containsKey(_adultApply.getApplyAdultId()))
				return;
			
			MarryMatchItem item = new MarryMatchItem(_adultApply);
			
			_m_hsUsIdSet.add(item.getUsId());
			
			_m_hmMatchItemMap.put(item.getApplyAdultId(), item);
			_m_alMatchItemList.add(item);
			_m_hmMatchItemListMap.computeIfAbsent(item.getMatchId(), k -> new ArrayList<>()).add(item);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除匹配池内数据
	 * @param _applyAdultId
	 */
	public void removeItem(long _applyAdultId)
	{
		_lock();
		
		try
		{
			MarryMatchItem item = _m_hmMatchItemMap.get(_applyAdultId);
			if(null == item)
				return;
			
			_m_alMatchItemList.remove(item);
			_m_hmMatchItemListMap.get(item.getMatchId()).remove(item);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除指定US的所有子嗣数据
	 * @param _usId
	 */
	public void removeItemByUs(int _usId)
	{
		_lock();
		
		try
		{
			_m_hsUsIdSet.remove(_usId);
			
			for(int i = _m_alMatchItemList.size() - 1; i >= 0; i--)
			{
				MarryMatchItem item = _m_alMatchItemList.get(i);
				if(null == item)
					continue;
				
				if(item.getUsId() == _usId)
				{
					_m_alMatchItemList.remove(i);
					_m_hmMatchItemMap.remove(item.getApplyAdultId());
					_m_hmMatchItemListMap.get(item.getMatchId()).remove(item);
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 获取匹配列表数据
	 * @param _adultId
	 * @param _cid
	 * @param _matchId
     * @param _bonus
	 * @param _count
	 * @return
	 */
	public ArrayList<MarryMatchItem> getMatchItemList(long _adultId, long _cid, int _matchId, long _bonus, int _count)
	{
		_lock();
		
		try
		{
			//先获取对应的匹配子嗣数据列表
			ArrayList<MarryMatchItem> matchItemList = _m_hmMatchItemListMap.get(_matchId);
			if(null == matchItemList)
				return null;
			
			//再一次过滤匹配子嗣数据
			ArrayList<MarryMatchItem> realMatchItemList = null;
			for(int i = 0; i < matchItemList.size(); i++)
			{
				MarryMatchItem item = matchItemList.get(i);
				if(null == item)
					continue;
				
				//过滤玩家自身子嗣
				if(item.getApplyCid() == _cid)
					continue;
				
				//过滤需要排除的子嗣
				if(item.getApplyAdultId() == _adultId)
					continue;

				if(null == realMatchItemList)
					realMatchItemList = new ArrayList<>();
				
				realMatchItemList.add(item);
			}
			
			//无对应匹配子嗣
			if(null == realMatchItemList)
				return null;
			
			if(_count >= realMatchItemList.size())
			{
				return realMatchItemList;
			}
			else //从实际随机池中筛选首个用户数据
			{
				ArrayList<MarryMatchItem> resultItemList = new ArrayList<>();
				//当前随机出的是首个数据
				int idx = CommonFunc.randomInt_noInclude(realMatchItemList.size());
				for(int i = 0; i < _count; i++)
				{
					int realIdx = idx % realMatchItemList.size();
					
					MarryMatchItem item = realMatchItemList.get(realIdx);
					resultItemList.add(item);
					
					idx++;
				}
				
				return resultItemList;
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 销毁数据
	 */
	public void discard()
	{
		_lock();
		
		try
		{
			_m_hmMatchItemMap.clear();
			_m_alMatchItemList.clear();
			_m_hmMatchItemListMap.clear();
		}
		finally
		{
			_unlock();
		}
	}
}
