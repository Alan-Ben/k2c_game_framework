package NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer;

import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_MAX_POWER_UP;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPRecordExtDealer_MARS_MAX_POWER extends _ANPRecordExtDealer
{
	@Override
	public ENPPlayerRecordParam getRecordParam() 
	{
		return ENPPlayerRecordParam.MARS_MAX_POWER;
	}

	@Override
	public void onCountChg(NPUSUserData _userData, long _oriCount, long _curCount, NPPlayerContext _context) 
	{
		long addCount = _curCount - _oriCount;
		if(addCount > 0)
		{
			//@EventDesc(id=54,name="P_MARS_MAX_POWER_UP",params={"ADD","NUM"})
			Event_P_MARS_MAX_POWER_UP evt = new Event_P_MARS_MAX_POWER_UP(_context, addCount, _curCount);
			_userData.onLogicEvent(evt);
		}
	}
}
