package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
玩家亲密度记录涨幅
****/
@EventDesc(id=31,name="P_CONSORT_INTIMACY_UP",params={"NUM"})
public class Event_P_CONSORT_INTIMACY_UP extends _ALogicEventBase
{
	public static final int ID =31;
	public Event_P_CONSORT_INTIMACY_UP(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_CONSORT_INTIMACY_UP(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//涨幅值
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}
