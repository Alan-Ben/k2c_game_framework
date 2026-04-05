package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import GC2GS.p008_TravelOp.GC2GS_008_006_ReqDealInvitationTravel;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventInvitation;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;

public class TravelEventDealer_Invitation extends _ATravelEventDealer <GC2GS_008_006_ReqDealInvitationTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.INVITATION;
	}

	//随机邀约指定妃子事件
	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_006_ReqDealInvitationTravel _msgParam, NPPlayerContext _context) 
	{
		RefTravelEventInvitation ref = _info.getRef().invitationEventRef;
		if(null == ref)
		{
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}
		
		if(null == _msgParam)
		{
			_result.setErrCode(CommErr.PARAM_ERROR.getCode());
			return;
		}
		
		ConsortInfo consort = _info.getUserData().getConsortComponent().lookup(_msgParam.getConsortId());
		if(null == consort)
		{
			_result.setErrCode(ConsortErr.CONSORT_NOT_EXISTS.getCode());
			return;
		}
		
		_info.getUserData().getConsortComponent().addRandCallConsortId(_msgParam.getConsortId(), _context);
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
