package NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer;

import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPRecordExtDealer_MARS_TEAM_MAX_POWER extends _ANPRecordExtDealer
{
	@Override
	public ENPPlayerRecordParam getRecordParam() 
	{
		return ENPPlayerRecordParam.MARS_TEAM_MAX_POWER;
	}

	@Override
	public void onCountChg(NPUSUserData _userData, long _oriCount, long _curCount, NPPlayerContext _context) 
	{
		long addCount = _curCount - _oriCount;
		if(addCount > 0)
		{
			_userData.getMarsComponent().doLazyCalMarsPower();
		}
	}
}
