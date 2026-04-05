package NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer;

import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_TOTAL_INTIMACY_CHG;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPRecordExtDealer_PLAYER_CONSORT_INTIMACY extends _ANPRecordExtDealer
{
	@Override
	public ENPPlayerRecordParam getRecordParam() 
	{
		return ENPPlayerRecordParam.PLAYER_CONSORT_INTIMACY;
	}

	@Override
	public void onCountChg(NPUSUserData _userData, long _oriCount, long _curCount, NPPlayerContext _context) 
	{
		Event_P_TOTAL_INTIMACY_CHG evt = new Event_P_TOTAL_INTIMACY_CHG(_context, _curCount);
		_userData.onLogicEvent(evt);
	}
}
