package DinnerServer.DinnerPool;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.DinnerObj.Dinner_Idx;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.HashSet;

/**
 * 跨服池里的宴会基础数据（用于排序所需 + 静态数据）
 * @author mj
 *
 */
public class DinnerCrossGroupDinner
{
	//宴会索引信息
	private Dinner_Idx _m_diDinnerIdx;

	//宴会开启时间
	private int _m_iStartTs;
	//宴会排序ID
	private int _m_iSortId;
	
	//玩家数据集合
	private HashSet<Long> _m_hsJoinerCidSet;
	
	//US服务器ID
	private int _m_iUsId;
	
	//锁对象
	private MutexAtom _m_mutex;
	
	public DinnerCrossGroupDinner(Dinner_Idx _dinnerIdx, int _startTs, int _sortId, ArrayList<Long> _joinerCidList)
	{
		_m_diDinnerIdx = _dinnerIdx;

		_m_iStartTs = _startTs;
		_m_iSortId = _sortId;

		_m_hsJoinerCidSet = new HashSet<>(_joinerCidList);
		
		_m_iUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_diDinnerIdx.getInstanceId());
		
		_m_mutex = new MutexAtom();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	//宴会数据
	public long getInstanceId() {return _m_diDinnerIdx.getInstanceId();}
	public long getDinnerId() {return _m_diDinnerIdx.getDinnerId();}
	public long getOwnerCid() {return _m_diDinnerIdx.getOwnerCid();}
	
	public int getJoinerCount() {return _m_diDinnerIdx.getJoinerCount();}
	public void setJoinerCount(int _joinerCount) {_m_diDinnerIdx.setJoinerCount(_joinerCount);}
	
	public int getEndTs() {return _m_diDinnerIdx.getEndTs();}
	
	public long getScore() {return _m_diDinnerIdx.getScore();}
	public void setScore(long _score) {_m_diDinnerIdx.setScore(_score);}
	public void addScore(long _score) {_m_diDinnerIdx.setScore(getScore() + _score);}
	
	//排序规则
	public int getSortId() {return _m_iSortId;}
	public void setSortId(int _sortId) {_m_iSortId = _sortId;}
	
	public int getStartTs() {return _m_iStartTs;}
	public void setStartTs(int _startTs) {_m_iStartTs = _startTs;}
	
	//US服务器ID
	public int getUsId() {return _m_iUsId;}
	
	/**
	 * 指定玩家是否参与宴会
	 * @param _cid
	 * @return
	 */
	public boolean isJoined(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hsJoinerCidSet.contains(_cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 玩家加入宴会
	 * @param _cid
	 */
	public void addJoiner(long _cid)
	{
		_lock();
		
		try
		{
			if(_m_hsJoinerCidSet.contains(_cid))
				return;
			
			_m_hsJoinerCidSet.add(_cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 构造宴会索引数据
	 * @param _cid
	 * @return
	 */
	public Dinner_Idx toProto(long _cid)
	{
		_m_diDinnerIdx.setIsJoined(isJoined(_cid));
		
		return _m_diDinnerIdx;
	}
}
