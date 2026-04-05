package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import GC2GS.p008_TravelOp.GC2GS_008_004_ReqDealConsortBarTravel;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.TravelErr;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventConsortBar;
import NPGameRes.Refs.Travel.RefTravelEventConsortBarCost;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;

public class TravelEventDealer_ConsortBar extends _ATravelEventDealer <GC2GS_008_004_ReqDealConsortBarTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.CONSORT_BAR;
	}

	//已婚妃子给亲密度，未婚妃子给好感度
	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_004_ReqDealConsortBarTravel _msgParam, NPPlayerContext _context) 
	{
		RefTravelEventConsortBar ref = _info.getRef().consortBarEventRef;
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
		
		RefTravelEventConsortBarCost costRef = RefTravelEventConsortBarCost.getMgr().get(_msgParam.getCostId());
		if(null == costRef)
		{
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}
		
		if(!_info.getUserData().hasItem(costRef.cost))
		{
			_result.setErrCode(CommErr.ITEM_NOT_ENOUGH.getCode());
			return;
		}
		
		if(!_info.getUserData().spendItem(costRef.cost, _context))
		{
			_result.setErrCode(CommErr.CONSUME_FAIL.getCode());
			return;
		}
		
		if(!ref.trigger_consort_id_list.contains(_msgParam.getConsortId()))
		{
			_result.setErrCode(TravelErr.TRAVEL_EVENT_CONSORT_NOT_IN.getCode());
			return;
		}
		
		ConsortInfo consort = _info.getUserData().getConsortComponent().lookup(_msgParam.getConsortId());
		if(null != consort) //存在已婚妃子，增加亲密度
		{
			consort.incrIntimacy(costRef.add_intimacy, _context);
		}
		else //不存在已婚妃子，增加好感度
		{
			_info.getUserData().getTravelComponent().consortAddLike(_msgParam.getConsortId(), costRef.add_like, _context);
		}
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
