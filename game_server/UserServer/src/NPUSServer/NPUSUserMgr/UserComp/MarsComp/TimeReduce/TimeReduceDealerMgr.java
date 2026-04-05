package NPUSServer.NPUSUserMgr.UserComp.MarsComp.TimeReduce;

import Common.MarsEnum.EMarsBagItemUseTimeType;

/**
 * 减少时间道具处理对象管理
 * @author mj
 *
 */
public class TimeReduceDealerMgr 
{
	private static TimeReduceDealerMgr _g_instance = new TimeReduceDealerMgr();
	public static TimeReduceDealerMgr getInstance() {return _g_instance;}
	
	private _ATimeReduceDealer[] _m_arrDealerArr;
	
	public TimeReduceDealerMgr()
	{
		_m_arrDealerArr = new _ATimeReduceDealer[EMarsBagItemUseTimeType.EMarsBagItemUseTimeType_Length];
		
		regDealer(new TimeReduceDealer_MARS_BUILDING());
		regDealer(new TimeReduceDealer_MARS_TECH());
		regDealer(new TimeReduceDealer_MARS_TEAM_REPAIR());
	}
	
	public void regDealer(_ATimeReduceDealer _dealer)
	{
		_m_arrDealerArr[_dealer.getType().ordinal()] = _dealer;
	}
	
	public _ATimeReduceDealer getDealer(EMarsBagItemUseTimeType _type)
	{
		return _m_arrDealerArr[_type.ordinal()];
	}
}
