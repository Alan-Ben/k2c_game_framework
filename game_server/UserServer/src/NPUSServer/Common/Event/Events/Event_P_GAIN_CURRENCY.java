package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
获取货币
****/
@EventDesc(id=53,name="P_GAIN_CURRENCY",params={"TYPE","NUM"})
public class Event_P_GAIN_CURRENCY extends _ALogicEventBase
{
	public static final int ID =53;
	public Event_P_GAIN_CURRENCY(_IContext _context,long _TYPE,long _NUM)
	{
		super(_context);
		
		set_TYPE(_TYPE);
		set_NUM(_NUM);

	}
	public Event_P_GAIN_CURRENCY(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//货币类型
	public void set_TYPE(long _TYPE){ setParamValue(0, _TYPE); }
	public long get_TYPE(){ return getParamValue(0); }
	//数量
	public void set_NUM(long _NUM){ setParamValue(1, _NUM); }
	public long get_NUM(){ return getParamValue(1); }

}
