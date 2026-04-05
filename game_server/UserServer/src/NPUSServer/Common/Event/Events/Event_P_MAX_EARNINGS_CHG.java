package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
最大赚速变更
****/
@EventDesc(id=21,name="P_MAX_EARNINGS_CHG",params={"EARNINGS","INCREASE_VALUE"})
public class Event_P_MAX_EARNINGS_CHG extends _ALogicEventBase
{
	public static final int ID =21;
	public Event_P_MAX_EARNINGS_CHG(_IContext _context,long _EARNINGS,long _INCREASE_VALUE)
	{
		super(_context);
		
		set_EARNINGS(_EARNINGS);
		set_INCREASE_VALUE(_INCREASE_VALUE);

	}
	public Event_P_MAX_EARNINGS_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//赚速
	public void set_EARNINGS(long _EARNINGS){ setParamValue(0, _EARNINGS); }
	public long get_EARNINGS(){ return getParamValue(0); }
	//增量
	public void set_INCREASE_VALUE(long _INCREASE_VALUE){ setParamValue(1, _INCREASE_VALUE); }
	public long get_INCREASE_VALUE(){ return getParamValue(1); }

}
