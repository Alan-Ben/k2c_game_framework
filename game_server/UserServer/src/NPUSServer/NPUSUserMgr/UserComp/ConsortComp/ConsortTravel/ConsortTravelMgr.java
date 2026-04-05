package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTravel;

import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Consort.RefConsortTravel;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortTravelBO;

import java.util.ArrayList;

/****
 * 针对需要计数的指定邀约数据管理，记录下一次更新时间，指定邀约的次数
 * @author mj
 *
 */
public class ConsortTravelMgr 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//出游方式的相关数据
	private ArrayList<ConsortTravelInfo> _m_alConsortTravelList;
	
	public ConsortTravelMgr(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
		
		_m_alConsortTravelList = new ArrayList<>();
		
		_initDefault();
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_udUserData;}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	private void _initDefault()
	{
		for(int i = 0; i < RefConsortTravel.getMgr().getList().size(); i++)
		{
			RefConsortTravel ref = RefConsortTravel.getMgr().getList().get(i);
			if(null == ref)
				continue;
			
			ConsortTravelInfo info = new ConsortTravelInfo(_m_udUserData, ref);
			_m_alConsortTravelList.add(info);
		}
	}
	
	public void _initBo(PlayerConsortTravelBO _bo)
	{
		ConsortTravelInfo info = lookup(_bo.getTravelId());
		if(null == info)
		{
			USLog.error(getUSServer(), "player:{} init consort travel:{} fail, not find info.", getUserData().getCid(), _bo.getTravelId());
			return;
		}
		
		info._setBo(_bo);
	}
	
	/**
	 * 刷新周期时间
	 * @param _context
	 */
	public void refreshGemCallCount(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long nowMs = CommonFunc.getNowTimeMS();
			long nextRefreshEndMs = getUserData().getParam(ENPPlayerParam.CONSORT_CALL_NEXT_REFRESH_MS);
			
			//尚未到达刷新截至时间，不处理
			if(nowMs < nextRefreshEndMs)
				return;
			
			//更新刷新时间
			if (RefGeneral.Ref().consort_call_gem_reset == null)
	        {
	            USLog.error(getUSServer(), "player:{} generl.consort_call_gem_reset null error. ", getUserData().getCid());
	            return;
	        }
			
			long curNextRefreshEndMs = RefGeneral.Ref().consort_call_gem_reset.getNextFreshTimeTagMS(nowMs);
			getUserData().setParam(ENPPlayerParam.CONSORT_CALL_NEXT_REFRESH_MS, curNextRefreshEndMs);
			
			//重置次数
			for(int i = 0; i < _m_alConsortTravelList.size(); i++)
			{
				ConsortTravelInfo info = _m_alConsortTravelList.get(i);
				if(null == info)
					continue;
				
				info._clean();
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定出游数据
	 * @param _travelId
	 * @return
	 */
	public ConsortTravelInfo lookup(long _travelId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alConsortTravelList.size(); i++)
			{
				ConsortTravelInfo info = _m_alConsortTravelList.get(i);
				if(null == info)
					continue;
				
				if(info.getTravelId() == _travelId)
					return info;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/*********
	 * 检查并设置出游计数
	 * @param _travelId
	 * @param _travelCount
	 * @param _context
	 */
	public void checkAndSet(long _travelId, int _travelCount, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			ConsortTravelInfo info = lookup(_travelId);
			if(null == info)
				return;
		
			refreshGemCallCount(_context);
			
			info._setCount(_travelCount);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 检查并增加出游计数
	 * @param _travelId
	 * @param _context
	 */
	public void checkAndIncr(long _travelId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			ConsortTravelInfo info = lookup(_travelId);
			if(null == info)
				return;
			
			refreshGemCallCount(_context);
			
			info._incrCount();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
