package NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer;

import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_MARS_BAG_TIME_HELP_BUILDING_UP;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPRecordExtDealer_MARS_BUILD_TIME_REDUCED_MIN_SUM extends _ANPRecordExtDealer
{
	@Override
	public ENPPlayerRecordParam getRecordParam() 
	{
		return ENPPlayerRecordParam.MARS_BUILD_TIME_REDUCED_MIN_SUM;
	}

	@Override
	public void onCountChg(NPUSUserData _userData, long _oriCount, long _curCount, NPPlayerContext _context) 
	{
		long addCount = _curCount - _oriCount;
		if(addCount > 0)
		{
			Event_P_MARS_BAG_TIME_HELP_BUILDING_UP evt = new Event_P_MARS_BAG_TIME_HELP_BUILDING_UP(_context, addCount);
			_userData.onLogicEvent(evt);
		}
	}
}
