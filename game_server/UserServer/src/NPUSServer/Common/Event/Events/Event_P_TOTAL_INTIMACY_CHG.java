package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
玩家亲密度记录
****/
@EventDesc(id=34,name="P_TOTAL_INTIMACY_CHG",params={"NUM"})
public class Event_P_TOTAL_INTIMACY_CHG extends _ALogicEventBase
{
	public static final int ID =34;
	public Event_P_TOTAL_INTIMACY_CHG(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_TOTAL_INTIMACY_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//亲密度
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}
