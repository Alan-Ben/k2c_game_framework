package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
只用于测试，只对服务端内部开放
****/
@EventDesc(id=0,name="S_ONLY_TEST",params={})
public class Event_S_ONLY_TEST extends _ALogicEventBase
{
	public static final int ID =0;
	public Event_S_ONLY_TEST(_IContext _context)
	{
		super(_context);
		

	}
	public Event_S_ONLY_TEST(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}
