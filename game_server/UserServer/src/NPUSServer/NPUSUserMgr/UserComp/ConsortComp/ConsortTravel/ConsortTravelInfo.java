package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTravel;

import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Consort.RefConsortTravel;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerConsortTravelBO;

public class ConsortTravelInfo 
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//出游配置
	private RefConsortTravel _m_refConsortTravel;
	
	//出游相关数据
	private long _m_lLastTravelMs;
	private int _m_iTravelCount;
	
	//Bo相关数据
	private PlayerConsortTravelBO _m_boConsortTravel;
	
	public ConsortTravelInfo(NPUSUserData _userData, RefConsortTravel _ref)
	{
		_m_udUserData = _userData;
		
		_m_refConsortTravel = _ref;
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_udUserData;}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//配置数据
	public RefConsortTravel getRef() {return _m_refConsortTravel;}
	public long getTravelId() {return _m_refConsortTravel.id;}
	
	//出游数据
	public long getLastTravelMs() {return _m_lLastTravelMs;}
	public int getTravelCount() {return _m_iTravelCount;}
	
	protected void _setBo(PlayerConsortTravelBO _bo)
	{
		_m_boConsortTravel = _bo;
		
		_m_lLastTravelMs = _bo.getLastTravelMs();
		_m_iTravelCount = _bo.getTravelCount();
	}
	
	/**
	 * 增加数量
	 */
	protected void _incrCount() 
	{
		_m_lLastTravelMs = CommonFunc.getNowTimeMS();
		_m_iTravelCount++;
		
		_save();
	}
	
	/**
	 * 设置数量
	 * @param _count
	 */
	protected void _setCount(int _count) 
	{
		_m_lLastTravelMs = CommonFunc.getNowTimeMS();
		_m_iTravelCount = _count;
		
		_save();
	}
	
	/**
	 * 重置数据
	 */
	protected void _clean() 
	{
		_m_lLastTravelMs = 0;
		_m_iTravelCount = 0;
		
		_save();
	}
	
	/********
	 * 保存bo数据
	 */
	private void _save()
	{
		BM bmObj = getUSServer().getBM();
		
		if(null == _m_boConsortTravel)
		{
			PlayerConsortTravelBO bo = new PlayerConsortTravelBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setTravelId(bmObj, getTravelId());
			bo.setLastTravelMs(bmObj, _m_lLastTravelMs);
			bo.setTravelCount(bmObj, _m_iTravelCount);
			bo.insert(bmObj);
			
			_m_boConsortTravel = bo;
		}
		else
		{
			_m_boConsortTravel.setLastTravelMs(bmObj, _m_lLastTravelMs);
			_m_boConsortTravel.setTravelCount(bmObj, _m_iTravelCount);
			_m_boConsortTravel.saveAll(bmObj);
		}
	}
}
