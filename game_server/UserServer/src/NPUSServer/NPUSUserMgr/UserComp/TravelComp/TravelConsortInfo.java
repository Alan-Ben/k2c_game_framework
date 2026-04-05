package NPUSServer.NPUSUserMgr.UserComp.TravelComp;

import Common.TravelObj.Travel_Consort;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerTravelConsortBO;

public class TravelConsortInfo 
{
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	
	//数据bo
	private PlayerTravelConsortBO _m_bo;
	
	public TravelConsortInfo(NPUSUserData _userData, PlayerTravelConsortBO _bo)
	{
		_m_usUserData = _userData;
		_m_bo = _bo;
	}

	//玩家数据
	public NPUSUserData getUserData() {return _m_usUserData;}
	//US服务器
	public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}
	
	//数据bo
	public PlayerTravelConsortBO getBo() {return _m_bo;}
	//妃子ID
	public long getConsortId() {return _m_bo.getConsortId();}
	//好感度
	public int getLike() {return _m_bo.getLike();}
	
	/**
	 * 构造游历妃子数据
	 * @return
	 */
	public Travel_Consort toProto()
	{
		Travel_Consort proto = new Travel_Consort();
		proto.setConsortId(getConsortId());
		proto.setLike(getLike());
		
		return proto;
	}
	
	/**
	 * 设置妃子好感度
	 * @param _addValue
	 */
	protected void _setLike(int _addValue) 
	{
		_m_bo.setLike(getUSServer().getBM(), _addValue);
		_m_bo.saveAll(getUSServer().getBM());
	}
	
	/**
	 * 增加妃子好感度
	 * @param _addValue
	 */
	protected void _addLike(int _addValue) 
	{
		_setLike(getLike() + _addValue);
	}
	
	/**
	 * 销毁数据
	 */
	protected void _discard() 
	{
		_m_bo.del(getUSServer().getBM());
	}
}
