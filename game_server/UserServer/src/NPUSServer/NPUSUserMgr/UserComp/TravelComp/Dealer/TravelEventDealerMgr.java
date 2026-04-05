package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.TravelEnum.ETravelEventType;
import NPCommon.ErrMain.TravelErr;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;

public class TravelEventDealerMgr 
{
	//全局单例模式
	private static TravelEventDealerMgr _g_instance = new TravelEventDealerMgr();
	public static TravelEventDealerMgr getInstance() 
	{
		return _g_instance;
	}
	
	//处理dealer数组
	@SuppressWarnings("rawtypes")
	private _ATravelEventDealer[] _m_arrDealerArr;
	
	public TravelEventDealerMgr()
	{
		_m_arrDealerArr = new _ATravelEventDealer[ETravelEventType.ETravelEventType_Length];
		
		regDealer(new TravelEventDealer_Reward());//奖励事件
		regDealer(new TravelEventDealer_ConsortLike());//妃子好感度事件
		regDealer(new TravelEventDealer_ConsortIntimacy());//妃子亲密度事件
		regDealer(new TravelEventDealer_AddPower());//增加大臣国力事件
		regDealer(new TravelEventDealer_Change());//交换物品事件
		regDealer(new TravelEventDealer_Invitation());//妃子邀约事件
		regDealer(new TravelEventDealer_Giftde());//必生卷王子嗣事件
		regDealer(new TravelEventDealer_ConsortBar());//妃子酒馆事件
		regDealer(new TravelEventDealer_Gambling());//博彩事件
	}
	
	/**
	 * 注册处理dealer
	 * @param _dealer
	 */
	@SuppressWarnings("rawtypes")
	public void regDealer(_ATravelEventDealer _dealer)
	{
		_m_arrDealerArr[_dealer.getType().ordinal()] = _dealer;
	}
	/**
	 * 获取对应dealer
	 * @param _type
	 * @return
	 */
	@SuppressWarnings("rawtypes")
	public _ATravelEventDealer getDealer(ETravelEventType _type)
	{
		return _m_arrDealerArr[_type.ordinal()];
	}

	/**
	 * 获取指定dealer并处理事件
	 * @param _eventType
	 * @param _info
	 * @param _typeId
	 * @param _context
	 * @return
	 */
	@SuppressWarnings({ "rawtypes", "unchecked"})
	public <T extends _IALProtocolStructure> TravelEventDealerResult dealEvent(ETravelEventType _eventType, TravelCanDealEventInfo _info, T _msgParam, NPPlayerContext _context)
	{
		//结果数据
		TravelEventDealerResult result = new TravelEventDealerResult(_info.getEventId());
		
		//获取对应的dealer对象
		_ATravelEventDealer dealer = getDealer(_eventType);
		if(null == dealer)
		{
			result.setErrCode(TravelErr.TRAVEL_EVENT_NOT_DEAL.getCode());
			return result;
		}
		
		//处理流程
		 dealer.deal(result, _info, _msgParam, _context);
		 
		 //处理成功，增加游历次数计数
		 if(result.isSucc())
		{
			 _info.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.TRAVEL_COUNT, 1, _context);
		}
		 
		 return result;
	}
}
