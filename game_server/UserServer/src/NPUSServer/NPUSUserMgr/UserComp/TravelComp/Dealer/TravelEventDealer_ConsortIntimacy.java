package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import Common.TravelObj.Travel_EventResultExt_ConsoleIntimacy;
import CommonEnum.ECurrency;
import GC2GS.p008_TravelOp.GC2GS_008_009_ReqDealConsortIntimacyTravel;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventConsortIntimacy;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.USLog;

public class TravelEventDealer_ConsortIntimacy extends _ATravelEventDealer <GC2GS_008_009_ReqDealConsortIntimacyTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.CONSORT_INTIMACY;
	}

	//增加妃子亲密度
	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_009_ReqDealConsortIntimacyTravel _msgParam, NPPlayerContext _context) 
	{
		RefTravelEventConsortIntimacy ref = _info.getRef().consortIntimacyEventRef;
		if(null == ref)
		{	
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}
		
		ConsortInfo consort = _info.getUserData().getConsortComponent().lookup(ref.consort_id);
		if(null == consort)
		{
			_result.setErrCode(ConsortErr.CONSORT_NOT_EXISTS.getCode());
			return;
		}
		
		//需要记录之前的亲密度
		long preIntimacy = consort.getIntimacy();
		Travel_EventResultExt_ConsoleIntimacy ext = new Travel_EventResultExt_ConsoleIntimacy();
		ext.setPreIntimacy(preIntimacy);
		_result.getEventResult().setExt(ext.makePackage());
		
		consort.incrIntimacy(ref.add_intimacy, _context);
	}

	@Override
	public boolean canAkeySpecDeal() 
	{
		return true;
	}

	@Override
	public Travel_EventResult dealAkeySpec(NPUSUserData _userData, RefTravelEvent _ref, NPPlayerContext _context) 
	{
		RefTravelEventConsortIntimacy ref = _ref.consortIntimacyEventRef;
		if(null == ref)
		{
			USLog.error(_userData.getUSServer(), "player:{} event:{} deal travel consort intimacy event error, not find sub ref.", _userData.getCid(), _ref.event_id);
			return null;
		}

		
		ConsortInfo consort = _userData.getConsortComponent().lookup(ref.consort_id);
		if(null == consort)
		{
			USLog.error(_userData.getUSServer(), "player:{} event:{} consort:{} deal travel consort intimacy event error, consort already existed.", _userData.getCid(), _ref.event_id, ref.consort_id);
			return null;
		}

		//处理结果
		Travel_EventResult result = new Travel_EventResult();
		result.setEventId(_ref.event_id);
		
		//需要记录之前的亲密度
		long preIntimacy = consort.getIntimacy();
		Travel_EventResultExt_ConsoleIntimacy ext = new Travel_EventResultExt_ConsoleIntimacy();
		ext.setPreIntimacy(preIntimacy);
		result.setExt(ext.makePackage());
		
		consort.incrIntimacy(ref.add_intimacy, _context);

		//获得事件物品列表
		_userData.gainItemList(_ref.event_item_list, _context);
		//获得对应的经验
		long travelEventGainPlayerExpAdd = _userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.TRAVEL_EVENT_GAIN_PLAYER_EXP_ADD);
		long gainPlayerExp = _ref.gain_player_exp + travelEventGainPlayerExpAdd;
		_userData.gainItem(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal(),gainPlayerExp, _context);
		//补充获得物品列表
		_context.getCollector().fillProtoList(result.getItemList());
		
		return result;
	}
}
