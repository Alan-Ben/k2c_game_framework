package DinnerServer.DinnerPool;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.DinnerObj.Dinner_Idx;
import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Dinner.DinnerGetIdxListResult;
import NPGameRes.GameObjs.Dinner.DinnerGetIdxResult;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;

/**
 * 跨服宴会池数据管理
 * @author mj
 *
 */
public class DinnerCrossGroup
{
	//分组ID
	private long _m_lGroupId;
	
	//跨服宴会数据Map，key-宴会实例ID
	private HashMap<Long, DinnerCrossGroupDinner> _m_hmDinnerMap;
	//跨服宴会数据List
	private ArrayList<DinnerCrossGroupDinner> _m_alDinnerList;
	
	//锁对象
	private MutexObject _m_mutex;
	
	public DinnerCrossGroup(long _groupId)
	{
		_m_lGroupId = _groupId;
		
		_m_hmDinnerMap = new HashMap<>();
		_m_alDinnerList = new ArrayList<>();
		
		_m_mutex = new MutexObject();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public long getGroupId() {return _m_lGroupId;}
	
	/**
	 * 检查当前跨服分组数据是否为空
	 */
	protected boolean _isEmpty()
	{
		_lock();
		
		try
		{
			return _m_alDinnerList.isEmpty();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
     * 用于展示的遍历
     * 排序规则：
     *  第一优先级：未参加宴会>已参加宴会（在玩家发起请求时处理）
     *  第二优先级：根据宴会类型排序，豪华宴会>普通宴会>家人宴会>庆功宴
     *  第三优先级：根据宴会开宴时间，从新到旧排序
     * 注：本服池才需要进行排序
     */
    protected void _sortDinner()
    {
    	CommonFunc.sortAscList(_m_alDinnerList, new Comparator<DinnerCrossGroupDinner>() 
    	{
			@Override
			public int compare(DinnerCrossGroupDinner o1, DinnerCrossGroupDinner o2) 
			{
				//第二优先级：根据宴会类型排序，豪华宴会>普通宴会>家人宴会>庆功宴
				if(o1.getSortId() != o2.getSortId())
					return Integer.compare(o1.getSortId(), o2.getSortId());
				
				//第三优先级：根据宴会开宴时间，从新到旧排序
				return Integer.compare(o2.getStartTs(), o1.getStartTs());
			}
		});
    }
	
	/**
	 * 查找宴会数据
	 * @param _instanceId
	 * @return
	 */
	public DinnerCrossGroupDinner lookup(long _instanceId)
	{
		_lock();
		
		try
		{
			return _m_hmDinnerMap.get(_instanceId);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加宴会数据
	 * @param _instanceId
	 * @param _startTs
	 * @param _dinnerType
	 * @param _ownerCid
	 * @param _seatCount
	 * @param _joinerCidSet
	 */
	public void addDinner(Dinner_Idx _dinnerIdx, int _startTs, int _sortId, ArrayList<Long> _joinerCidList)
	{
		_lock();
		
		try
		{
			//不再重复加入重复宴会
			if(_m_hmDinnerMap.containsKey(_dinnerIdx.getInstanceId()))
				return;
			
			//构造宴会数据
			DinnerCrossGroupDinner dinner = new DinnerCrossGroupDinner(_dinnerIdx, _startTs, _sortId, _joinerCidList);
			
			_m_hmDinnerMap.put(_dinnerIdx.getInstanceId(), dinner);
			_m_alDinnerList.add(dinner);
			
			//宴会排序
			_sortDinner();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除指定宴会数据
	 * @param _instanceId
	 */
	public void removeDinner(long _instanceId)
	{
		_lock();
		
		try
		{
			if(!_m_hmDinnerMap.containsKey(_instanceId))
				return;
			
			DinnerCrossGroupDinner dinner = _m_hmDinnerMap.remove(_instanceId);
			_m_alDinnerList.remove(dinner);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 移除指定US的所有宴会数据
	 * @param _usId
	 */
	public void removeDinnerByUs(int _usId)
	{
		_lock();
		
		try
		{
			for(int i = _m_alDinnerList.size() - 1; i >= 0; i--)
			{
				DinnerCrossGroupDinner dinner = _m_alDinnerList.get(i);
				if(null == dinner)
					continue;
				
				if(dinner.getUsId() == _usId)
				{
					_m_alDinnerList.remove(i);
					_m_hmDinnerMap.remove(dinner.getInstanceId());
				}
			}
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造玩家的宴会索引列表
	 * @param _cid
	 * @param _page
	 * @param _num
	 * @param _callback
	 */
	public void makeIdxList(long _cid, int _page, int _num, _ICallBackIntT<DinnerGetIdxListResult> _callback)
    {
    	_lock();
    	
    	try
    	{
    		//检查参数
    		if(_page < 1 || _num < 1)
    		{
    			_callback.onRunOver(CommErr.PARAM_ERROR.getCode(), null);
    			return;
    		}
    		
    		//单次请求数量检查
    		if(_num > 100)
    		{
    			CommLog.error("player:{} num:{} want to get dinner idx > 100.", _cid, _num);
    			_num = 100;
    		}

    		//宴会索引数据结果
    		DinnerGetIdxListResult result = new DinnerGetIdxListResult();
    		//计算当前页的前一条位置
    		int lastNum = _page * _num - 1;
    		int startNum = lastNum - _num;
    		//当前请求数量超过已存在的数量
    		if(startNum > _m_alDinnerList.size())
    		{
    			_callback.onRunOver(0, result);
    			return;
    		}
    		
    		//玩家参与的宴会列表
    		ArrayList<DinnerCrossGroupDinner> joinedDinnerList = null;
    		//先过滤玩家举办或参加的宴会数据
    		int canCount = 0;
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerCrossGroupDinner info = _m_alDinnerList.get(i);
    			if(null == info)
    				continue;
    			
    			//过滤自己开的宴会
    			if(info.getOwnerCid() == _cid)
    				continue;
    			
    			//过滤赴宴的数据
    			if(info.isJoined(_cid))
    			{
    				if(null == joinedDinnerList)
    					joinedDinnerList = new ArrayList<>();
    				
    				joinedDinnerList.add(info);
    				continue;
    			}
    			
    			//可以参加的宴会进行计数，用于确认分页的起始数据
    			canCount++;
    			if(canCount <= startNum)
    				continue;
    			
    			//数量足够则直接返回结果
    			if(result.getIdxList().size() >= _num)
    			{
    				result.setHasNext();
    				break;
    			}
    			
    			//符合宴会数据
    			result.addDinnerIdx(info.toProto(_cid));
    		}
    		//数量不足用则继续补充已经赴宴的宴会
    		if(result.getIdxList().size() < _num && null != joinedDinnerList)
    		{
    			for(int i = 0; i < joinedDinnerList.size(); i++)
        		{
    				DinnerCrossGroupDinner info = joinedDinnerList.get(i);
        			
        			//可以参加的宴会进行计数，用于确认分页的起始数据
        			canCount++;
        			if(canCount <= startNum)
        				continue;
        			
        			//数量足够则直接返回结果
        			if(result.getIdxList().size() >= _num)
        			{
        				result.setHasNext();
        				break;
        			}
        			
        			//补充宴会数据
        			result.addDinnerIdx(info.toProto(_cid));
        		}
    		}
    		
    		_callback.onRunOver(0, result);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
	
	/**
	 * 获取指定宴会数据
	 * @param _result
	 * @param _instanceId
	 * @param _cid
	 */
	public void makeResult(DinnerGetIdxResult _result, long _instanceId, long _cid)
    {
    	_lock();
    	
    	try
    	{
    		boolean hasPre = false;
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerCrossGroupDinner dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//后一条数据
    			if(null != _result.getDinnerIdx())
    			{
    				if(dinner.getOwnerCid() == _cid)
    					continue;
    				
    				_result.setHasNext();
    				break;
    			}
    			//当前一条数据
    			if(_instanceId == dinner.getInstanceId())
    			{
    				//设置宴会数据
    				_result.setDinnerIdx(dinner.toProto(_cid));
    				//当前排序序列号
    				_result.setIdx(i);
    				
    				//上一条宴会数据存在标志
    				if(hasPre)
    					_result.setHasPre();
    			}
    			
    			//上一条数据标志
    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() != _cid)
    				hasPre = true;
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
	
	/**
     * 获取前一个宴会数据
     * @param _result
     * @param _sortSerial
     * @param _cid
     */
    public void makePreResult(DinnerGetIdxResult _result, long _instanceId, int _idx, long _cid)
    {
    	_lock();
    	
    	try
    	{
    		if(_m_alDinnerList.isEmpty())
    			return;
    		
    		boolean canTake = false;
    		//检查是否存在指定宴会数据
    		for(int i = _m_alDinnerList.size() - 1; i >= 0; i--)
    		{
    			DinnerCrossGroupDinner dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//前一条数据
    			if(null != _result.getDinnerIdx())
    			{
    				_result.setHasPre();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
    				//设置选中的宴会结果
    				_result.setDinnerIdx(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置后一个标志位
    			_result.setHasNext();
    			
    			//检查当前数据
    			if(_instanceId == dinner.getInstanceId())
    			{
    				canTake = true;
    				continue;
    			}
    		}
    		
    		//已经存在宴会，无需再次查找
    		if(null != _result.getDinnerIdx())
    			return;
    		
    		//如果没有找到合适的，从排序idx下标开始找起
    		//重置检查标志位
    		canTake = false;
    		//重置结果数据
    		_result.reset();
    		//检查是否存在指定宴会数据
    		for(int i = _m_alDinnerList.size() - 1; i >= 0; i--)
    		{
    			DinnerCrossGroupDinner dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//后一条数据
    			if(null != _result.getDinnerIdx())
    			{
    				_result.setHasPre();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
    				//设置选中的宴会结果
    				_result.setDinnerIdx(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置前一个标志位
    			_result.setHasNext();
    			
    			//检查当前数据
    			if(i <= _idx)
    			{
    				canTake = true;
    				continue;
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 获取下一个宴会数据
     * @param _result
     * @param _instanceId
     * @param _idx
     * @param _cid
     */
    public void makeNextResult(DinnerGetIdxResult _result, long _instanceId, int _idx, long _cid)
    {
    	_lock();
    	
    	try
    	{
    		if(_m_alDinnerList.isEmpty())
    			return;
    		
    		boolean canTake = false;
    		//检查是否存在指定宴会数据
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerCrossGroupDinner dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//后一条数据
    			if(null != _result.getDinnerIdx())
    			{
    				_result.setHasNext();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
    				//设置选中的宴会结果
    				_result.setDinnerIdx(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置前一个标志位
    			_result.setHasPre();
    			
    			//检查当前数据
    			if(_instanceId == dinner.getInstanceId())
    			{
    				canTake = true;
    				continue;
    			}
    		}

    		//已经存在宴会，无需再次查找
    		if(null != _result.getDinnerIdx())
    			return;
    		
    		//如果没有找到合适的，从排序idx下标开始找起
    		//重置检查标志位
    		canTake = false;
    		//重置结果数据
    		_result.reset();
    		//检查是否存在指定宴会数据
    		for(int i = 0; i < _m_alDinnerList.size(); i++)
    		{
    			DinnerCrossGroupDinner dinner = _m_alDinnerList.get(i);
    			if(null == dinner)
    				continue;

    			//过滤玩家开启的宴会
    			if(dinner.getOwnerCid() == _cid)
    				continue;
    			
    			//后一条数据
    			if(null != _result.getDinnerIdx())
    			{
    				_result.setHasNext();
    				break;
    			}
    			
    			//当前数据
    			if(canTake)
    			{
    				//设置选中的宴会结果
    				_result.setDinnerIdx(dinner.toProto(_cid));
    				_result.setIdx(i);
    				
    				continue;
    			}
    			
    			//设置前一个标志位
    			_result.setHasPre();
    			
    			//检查当前数据
    			if(i >= _idx)
    			{
    				canTake = true;
    				continue;
    			}
    		}
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
			
			sb.append("size:").append(_m_alDinnerList.size());
			for(int i = 0; i < _m_alDinnerList.size(); i++)
			{
				DinnerCrossGroupDinner dinner = _m_alDinnerList.get(i);
				if(null == dinner)
					continue;
				
				sb.append(dinner.getInstanceId()).append(", ");
			}
			
			return sb.toString();
		}
		finally
		{
			_unlock();
		}
	}
}
