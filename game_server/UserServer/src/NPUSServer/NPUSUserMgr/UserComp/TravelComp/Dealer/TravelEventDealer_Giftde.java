package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import GC2GS.p008_TravelOp.GC2GS_008_010_ReqDealGiftedTravel;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventGiftde;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;

public class TravelEventDealer_Giftde extends _ATravelEventDealer <GC2GS_008_010_ReqDealGiftedTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.GIFTDE;
	}

	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_010_ReqDealGiftedTravel _msgParam, NPPlayerContext _context) 
	{
		RefTravelEventGiftde ref = _info.getRef().giftdeEventRef;
		if(null == ref)
		{
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}
		
		_info.getUserData().incParam(ENPPlayerParam.BIRTH_GIFTDE_COUM, 1);
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
