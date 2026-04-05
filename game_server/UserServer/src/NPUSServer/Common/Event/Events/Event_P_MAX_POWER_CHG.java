package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
最大战力变更
****/
@EventDesc(id=25,name="P_MAX_POWER_CHG",params={"POWER","INCREASE_VALUE"})
public class Event_P_MAX_POWER_CHG extends _ALogicEventBase
{
	public static final int ID =25;
	public Event_P_MAX_POWER_CHG(_IContext _context,long _POWER,long _INCREASE_VALUE)
	{
		super(_context);
		
		set_POWER(_POWER);
		set_INCREASE_VALUE(_INCREASE_VALUE);

	}
	public Event_P_MAX_POWER_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//战力
	public void set_POWER(long _POWER){ setParamValue(0, _POWER); }
	public long get_POWER(){ return getParamValue(0); }
	//增量
	public void set_INCREASE_VALUE(long _INCREASE_VALUE){ setParamValue(1, _INCREASE_VALUE); }
	public long get_INCREASE_VALUE(){ return getParamValue(1); }

}
