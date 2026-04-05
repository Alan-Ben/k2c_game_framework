package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
常驻排行榜点赞
****/
@EventDesc(id=24,name="P_RANK_FIXED_LIKE",params={"NUM"})
public class Event_P_RANK_FIXED_LIKE extends _ALogicEventBase
{
	public static final int ID =24;
	public Event_P_RANK_FIXED_LIKE(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_RANK_FIXED_LIKE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//点赞数量
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}
