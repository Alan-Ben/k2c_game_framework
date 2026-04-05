package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
妃子送礼
****/
@EventDesc(id=15,name="P_CONSORT_SEND_GIFT",params={"NUM"})
public class Event_P_CONSORT_SEND_GIFT extends _ALogicEventBase
{
	public static final int ID =15;
	public Event_P_CONSORT_SEND_GIFT(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_CONSORT_SEND_GIFT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//数量
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}
