package NPUSServer.CommonActivityMgr.Core.Rank;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.Common_LongList;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import USDB.Bo.ActivityRankBO;

import java.nio.ByteBuffer;
import java.util.HashSet;

/**
 * 玩家已领取记录数据管理
 * @author mj
 *
 */
public class ActivityRankPlayerRewardedMgr 
{
	//排行榜数据
	private ActivityRankInfo _m_riRankInfo;
	//已领取奖励的玩家CID数据集合
	private HashSet<Long> _m_hsRewardedCidSet;
	
	//数据Bo对象
	private ActivityRankBO _m_bo;
	
    private MutexAtom _m_mutex;
	
	public ActivityRankPlayerRewardedMgr(ActivityRankInfo _rank, ActivityRankBO _bo)
	{
		_m_riRankInfo = _rank;
		_m_hsRewardedCidSet = new HashSet<>();

		_m_bo = _bo;
		if(null != _m_bo.getRewardedCidSet())
		{
			ByteBuffer buff = ByteBuffer.wrap(_bo.getRewardedCidSet());
			Common_LongList obj = new Common_LongList();
			obj.readPackage(buff);
			
			_m_hsRewardedCidSet.addAll(obj.getValueList());
		}
		
		_m_mutex = new MutexAtom();
	}
	
	public ActivityRankInfo getRank() {return _m_riRankInfo;}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	/**
	 * 是否已领取奖励
	 * @param _cid
	 * @return
	 */
	public boolean isRewarded(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hsRewardedCidSet.contains(_cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加玩家领取记录
	 * @param _cid
	 * @return
	 */
	public boolean addRewarded(long _cid)
	{
		_lock();
		
		try
		{
			if(!_m_hsRewardedCidSet.add(_cid))
				return false;
			
			Common_LongList obj = new Common_LongList();
			obj.getValueList().addAll(_m_hsRewardedCidSet);
			
			BM bmObj = _m_riRankInfo.getActivity().getUSServer().getBM();
			_m_bo.saveRewardedCidSet(bmObj, CommonFunc.ByteBfferToBytes(obj.makePackage()));
			
			return true;
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 增加邮件领取的玩家记录，本操作只增加内存，不做bo数据处理
	 * @param _cid
	 */
	public boolean addMailedRewardedCid(long _cid)
	{
		_lock();
		
		try
		{
			return _m_hsRewardedCidSet.add(_cid);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 更新已领取记录
	 */
	public void saveRewardedList()
	{
		_lock();
		
		try
		{
			Common_LongList obj = new Common_LongList();
			obj.getValueList().addAll(_m_hsRewardedCidSet);
			
			BM bmObj = _m_riRankInfo.getActivity().getUSServer().getBM();
			_m_bo.saveRewardedCidSet(bmObj, CommonFunc.ByteBfferToBytes(obj.makePackage()));
		}
		finally
		{
			_unlock();
		}
	}
}
