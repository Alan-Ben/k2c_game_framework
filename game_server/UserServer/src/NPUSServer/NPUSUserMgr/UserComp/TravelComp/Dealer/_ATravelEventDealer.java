package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import CommonEnum.ECurrency;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.USLog;

public abstract class _ATravelEventDealer <T extends _IALProtocolStructure>
{
	/**
	 * 事件类型，用于区分对应的dealer
	 * @return
	 */
	abstract public ETravelEventType getType();
	
	/**
	 * dealer额外处理
	 * @param _result
	 * @param _info
	 * @param _msgParam
	 * @param _context
	 */
	abstract protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, T _msgParam, NPPlayerContext _context);
	
	/**
	 * 是否可以在一键游历中特殊处理，true-调用_dealAkeySpec方法
	 */
	abstract public boolean canAkeySpecDeal();
	
	/**
	 * 一次性事件场景下的特殊处理
	 * @param _userData
	 * @param _ref
	 * @param _context
	 * @return
	 */
	abstract public Travel_EventResult dealAkeySpec(NPUSUserData _userData, RefTravelEvent _ref, NPPlayerContext _context);

	/**
	 * 处理游历事件
	 * @param _result
	 * @param _info
	 * @param _msgParam
	 * @param _context
	 */
	public void deal(TravelEventDealerResult _result, TravelCanDealEventInfo _info, T _msgParam, NPPlayerContext _context)
	{
		//检查配表
		if(null == _info.getRef())
		{
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}
		
		//各自dealer额外处理
		_dealExt(_result, _info, _msgParam, _context);
		
		//处理失败
		if(!_result.isSucc())
		{
			USLog.error(_info.getUSServer(), "player:{} event:{} travel event deal fail, err:{}.", _info.getUserData().getCid(), _info.getEventId(), _result.getErrCode());
			return;
		}
		
		//获得事件物品列表
		_info.getUserData().gainItemList(_info.getRef().event_item_list, _context);
		//获得对应的经验
		long travelEventGainPlayerExpAdd = _info.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.TRAVEL_EVENT_GAIN_PLAYER_EXP_ADD);
		long gainPlayerExp = _info.getRef().gain_player_exp + travelEventGainPlayerExpAdd;
		_info.getUserData().gainItem(ENPItemType.CURRENCY, ECurrency.P_EXP.ordinal(),gainPlayerExp, _context);
		//补充获得物品列表
		_context.getCollector().fillProtoList(_result.getEventResult().getItemList());
	}	
}
