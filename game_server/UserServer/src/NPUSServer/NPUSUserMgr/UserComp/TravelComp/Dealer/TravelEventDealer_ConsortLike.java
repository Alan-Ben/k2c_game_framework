package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import Common.TravelObj.Travel_EventResultExt_ConsoleLike;
import CommonEnum.ECurrency;
import GC2GS.p008_TravelOp.GC2GS_008_008_ReqDealConsortLikeTravel;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventConsortLike;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelConsortInfo;
import NPUSServer.USLog;

public class TravelEventDealer_ConsortLike extends _ATravelEventDealer <GC2GS_008_008_ReqDealConsortLikeTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.CONSORT_LIKE;
	}

	//增加妃子好感度
	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_008_ReqDealConsortLikeTravel _msgParam, NPPlayerContext _context) 
	{
		RefTravelEventConsortLike ref = _info.getRef().consortLikeEventRef;
		if(null == ref)
		{
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}

		//检查对应妃子
		if(_info.getUserData().getConsortComponent().hasConsort(ref.consort_id))
		{
			_result.setErrCode(ConsortErr.CONSORT_EXISTED.getCode());
			return;
		}
		
		//需要记录之前的好感度
		long preLike = 0;
		TravelConsortInfo consort = _info.getUserData().getTravelComponent().lookupConsort(ref.consort_id);
		if(null != consort)
		{
			preLike = consort.getLike();
		}
		
		Travel_EventResultExt_ConsoleLike ext = new Travel_EventResultExt_ConsoleLike();
		ext.setPreLike(preLike);
		_result.getEventResult().setExt(ext.makePackage());
		
		_info.getUserData().getTravelComponent().consortAddLike(ref.consort_id, ref.add_like, _context);
	}

	@Override
	public boolean canAkeySpecDeal() 
	{
		return true;
	}

	@Override
	public Travel_EventResult dealAkeySpec(NPUSUserData _userData, RefTravelEvent _ref, NPPlayerContext _context) 
	{
		RefTravelEventConsortLike ref = _ref.consortLikeEventRef;
		if(null == ref)
		{
			USLog.error(_userData.getUSServer(), "player:{} event:{} deal travel consort like event error, not find sub ref.", _userData.getCid(), _ref.event_id);
			return null;
		}

		//检查对应妃子
		if(_userData.getConsortComponent().hasConsort(ref.consort_id))
		{
			USLog.error(_userData.getUSServer(), "player:{} event:{} consort:{} deal travel consort like event error, consort already existed.", _userData.getCid(), _ref.event_id, ref.consort_id);
			return null;
		}

		//处理结果
		Travel_EventResult result = new Travel_EventResult();
		result.setEventId(_ref.event_id);
		
		//需要记录之前的好感度
		long preLike = 0;
		TravelConsortInfo consort = _userData.getTravelComponent().lookupConsort(ref.consort_id);
		if(null != consort)
		{
			preLike = consort.getLike();
		}
		
		Travel_EventResultExt_ConsoleLike ext = new Travel_EventResultExt_ConsoleLike();
		ext.setPreLike(preLike);
		result.setExt(ext.makePackage());
		
		_userData.getTravelComponent().consortAddLike(ref.consort_id, ref.add_like, _context);

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
