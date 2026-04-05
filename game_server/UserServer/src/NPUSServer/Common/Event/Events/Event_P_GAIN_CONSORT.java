package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
获取妃子
****/
@EventDesc(id=38,name="P_GAIN_CONSORT",params={"CONSORT_ID"})
public class Event_P_GAIN_CONSORT extends _ALogicEventBase
{
	public static final int ID =38;
	public Event_P_GAIN_CONSORT(_IContext _context,long _CONSORT_ID)
	{
		super(_context);
		
		set_CONSORT_ID(_CONSORT_ID);

	}
	public Event_P_GAIN_CONSORT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//妃子ID
	public void set_CONSORT_ID(long _CONSORT_ID){ setParamValue(0, _CONSORT_ID); }
	public long get_CONSORT_ID(){ return getParamValue(0); }

}
