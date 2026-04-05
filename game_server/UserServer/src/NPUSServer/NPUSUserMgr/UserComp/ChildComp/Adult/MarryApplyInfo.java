package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult;

import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerAdultMarryApplyBO;

public class MarryApplyInfo 
{
	//子嗣数据
	private _AAdultInfo _m_adultInfo;
	
	//bo数据
	private PlayerAdultMarryApplyBO _m_bo;
	
	public MarryApplyInfo(_AAdultInfo _adult, PlayerAdultMarryApplyBO _bo)
	{
		_m_adultInfo = _adult;
		
		_m_bo = _bo;
	}
	
	//子嗣数据
	public _AAdultInfo getAdult() {return _m_adultInfo;}
	
	//玩家数据
	public NPUSUserData getUserData() {return _m_adultInfo.getComp().getUserData();}
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//申请数据
	public PlayerAdultMarryApplyBO getBo() {return _m_bo;}
	public int getApplyExpiredTs() {return getBo().getApplyExpiredTs();}
	
	//目标玩家数据
	public long getTargetCid() {return getBo().getTargetCid();}
	
	protected void _discard() 
	{
		_m_bo.del(getUSServer().getBM());
	}
}
