package NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer;

import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSORT_INTIMACY_UP;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPRecordExtDealer_PLAYER_CONSORT_INTIMACY_ADD extends _ANPRecordExtDealer
{
	@Override
	public ENPPlayerRecordParam getRecordParam() 
	{
		return ENPPlayerRecordParam.PLAYER_CONSORT_INTIMACY_ADD;
	}

	@Override
	public void onCountChg(NPUSUserData _userData, long _oriCount, long _curCount, NPPlayerContext _context) 
	{
		if(_curCount > _oriCount)
		{
			Event_P_CONSORT_INTIMACY_UP evt = new Event_P_CONSORT_INTIMACY_UP(_context, (_curCount - _oriCount));
			_userData.onLogicEvent(evt);
		}
	}
}
