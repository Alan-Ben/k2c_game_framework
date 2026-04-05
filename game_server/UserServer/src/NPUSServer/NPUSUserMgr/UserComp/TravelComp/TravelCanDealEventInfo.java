package NPUSServer.NPUSUserMgr.UserComp.TravelComp;

import Common.TravelObj.Travel_Event;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerTravelCanDealBO;

public class TravelCanDealEventInfo 
{
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	
	//bo数据
	private PlayerTravelCanDealBO _m_boTravelCanDeal;
	
	//配置数据
	private RefTravelEvent _m_refTravelEvent;

	public TravelCanDealEventInfo(NPUSUserData _userData, PlayerTravelCanDealBO _bo)
	{
		_m_usUserData = _userData;
		_m_boTravelCanDeal = _bo;
		
		_m_refTravelEvent = RefTravelEvent.getMgr().get(_m_boTravelCanDeal.getEventId());
		if(null == _m_refTravelEvent)
		{
			USLog.error(_m_usUserData.getUSServer(), "player:{} trave:{} not find ref.", _m_usUserData.getCid(), _m_boTravelCanDeal.getEventId());
		}
	}
	public TravelCanDealEventInfo(NPUSUserData _userData, PlayerTravelCanDealBO _bo, RefTravelEvent _ref)
	{
		_m_usUserData = _userData;
		_m_boTravelCanDeal = _bo;
		_m_refTravelEvent = _ref;
	}
	
	//玩家数据
	public NPUSUserData getUserData() {return _m_usUserData;}
	//US服务器
	public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}
	
	//事件数据
	public PlayerTravelCanDealBO getBo() {return _m_boTravelCanDeal;}
	public long getInstanceId() {return _m_boTravelCanDeal.getId();}
	public long getEventId() {return _m_boTravelCanDeal.getEventId();}
	public long getPosId() {return _m_boTravelCanDeal.getPosId();}
	
	//配置数据
	public RefTravelEvent getRef() {return _m_refTravelEvent;}
	
	/**
	 * 构造游历事件数据协议
	 * @return
	 */
	public Travel_Event toProto()
	{
		Travel_Event proto = new Travel_Event();
		proto.setInstanceId(getInstanceId());
		proto.setEventId(getEventId());
		proto.setPos(getPosId());
		
		return proto;
	}
	
	/**
	 * 移除指定事件
	 */
	protected void _discard() 
	{
		getBo().del(getUSServer().getBM());
	}
}
