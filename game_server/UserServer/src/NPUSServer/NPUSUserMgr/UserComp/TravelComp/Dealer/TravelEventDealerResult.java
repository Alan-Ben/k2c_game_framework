package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelObj.Travel_EventResult;

/**
 * 游历事件结果数据
 * @author mj
 *
 */
public class TravelEventDealerResult 
{
	private int _m_iErrCode;
	private Travel_EventResult _m_erEventResult;
	
	public TravelEventDealerResult(long _eventId)
	{
		_m_erEventResult = new Travel_EventResult();
		_m_erEventResult.setEventId(_eventId);
	}
	
	public int getErrCode() {return _m_iErrCode;}
	public void setErrCode(int _errCode) {_m_iErrCode = _errCode;}
	
	public Travel_EventResult getEventResult() {return _m_erEventResult;}
	
	public boolean isSucc() {return 0 == _m_iErrCode;}
}
