package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import GC2GS.p008_TravelOp.GC2GS_008_005_ReqDealChangeTravel;
import NPCommon.ErrMain.CommErr;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventChange;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;

public class TravelEventDealer_Change extends _ATravelEventDealer <GC2GS_008_005_ReqDealChangeTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.CHANGE;
	}

	//交换事件
	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_005_ReqDealChangeTravel _msgParam, NPPlayerContext _context) 
	{
		//玩家选择不兑换，则无需处理
		if(!_msgParam.getIsChange())
			return;
		
		RefTravelEventChange ref = _info.getRef().changeEventRef;
		if(null == ref)
		{
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}
		
		if(!_info.getUserData().hasItem(ref.event_cost))
		{
			_result.setErrCode(CommErr.ITEM_NOT_ENOUGH.getCode());
			return;
		}
		
		//消耗物品
		if(!_info.getUserData().spendItem(ref.event_cost, _context))
		{
			_result.setErrCode(CommErr.CONSUME_FAIL.getCode());
			return;
		}
		
		//获得物品
		_info.getUserData().gainItemList(ref.exchange_item_list, _context);
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
