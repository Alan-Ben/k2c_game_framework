package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.CollectResult;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import USDB.Bo.PlayerMarsTeamCollectResultBO;

import java.util.ArrayList;

public class CollectResultMgr 
{
	//队伍数据
	private MarsExploreTeam _m_team;
	//采集结果数据列表
	private ArrayList<CollectResultInfo> _m_alCollectResultList;
	//锁对象
	private MutexAtom _m_mutex;
	
	public CollectResultMgr(MarsExploreTeam _team)
	{
		_m_team = _team;
		
		_m_alCollectResultList = new ArrayList<>();
		
		_m_mutex = new MutexAtom();
	}
	
	private void _lock() {_m_mutex.lock();}
	private void _unlock() {_m_mutex.unlock();}
	
	public MarsExploreTeam getTeam() {return _m_team;}
	
	public void _initFromBo(PlayerMarsTeamCollectResultBO _bo)
	{
		CollectResultInfo info = new CollectResultInfo(getTeam(), _bo);
		_m_alCollectResultList.add(info);
	}
	
	/**
	 * 提交资源
	 * @param _refId
	 * @param _resNum
	 */
	public void submitResult(long _refId, long _resNum)
	{
		_lock();
		
		try
		{
			PlayerMarsTeamCollectResultBO bo = new PlayerMarsTeamCollectResultBO();
			bo.setCid(getTeam().getBM(), getTeam().getCid());
			bo.setTeamId(getTeam().getBM(), getTeam().getTeamId());
			bo.setRefId(getTeam().getBM(), _refId);
			bo.setResNum(getTeam().getBM(), _resNum);
			bo.insert(getTeam().getBM());
			
			CollectResultInfo info = new CollectResultInfo(getTeam(), bo);
			_m_alCollectResultList.add(info);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 下发所有奖励
	 * @param _context
	 */
	public void dispatchAll(NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			if(_m_alCollectResultList.isEmpty())
				return;
			
			//清空原有数据
			ArrayList<CollectResultInfo> list = new ArrayList<>(_m_alCollectResultList);
			
			_m_alCollectResultList.clear();
			getTeam().getBM().getBM(PlayerMarsTeamCollectResultBO.class).delAll("cid", getTeam().getCid());
			
			//下发奖励
			for(int i = 0; i < list.size(); i++)
			{
				CollectResultInfo info = list.get(i);
				if(null == info)
					continue;
				
				info._dispatch(_context);
			}
		}
		finally
		{
			_unlock();
		}
	}
}
