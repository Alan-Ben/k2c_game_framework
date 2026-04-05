package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import GC2GS.p008_TravelOp.GC2GS_008_003_ReqDealRewardTravel;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;

public class TravelEventDealer_Reward extends _ATravelEventDealer <GC2GS_008_003_ReqDealRewardTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.REWARD;
	}

	//无需额外处理
	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_003_ReqDealRewardTravel _msgParam, NPPlayerContext _context) 
	{
	}

	@Override
	public boolean canAkeySpecDeal() 
	{
		return false;
	}
	
	@Override
	public Travel_EventResult dealAkeySpec(NPUSUserData _userData, RefTravelEvent _ref, NPPlayerContext _context) 
	{
		return null;
	}
}
