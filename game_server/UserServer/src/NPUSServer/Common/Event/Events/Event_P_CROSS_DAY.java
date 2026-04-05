package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
玩家跨天登录
****/
@EventDesc(id=1,name="P_CROSS_DAY",params={"DAY_TAG"})
public class Event_P_CROSS_DAY extends _ALogicEventBase
{
	public static final int ID =1;
	public Event_P_CROSS_DAY(_IContext _context,long _DAY_TAG)
	{
		super(_context);
		
		set_DAY_TAG(_DAY_TAG);

	}
	public Event_P_CROSS_DAY(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//登录日期 例：20210812
	public void set_DAY_TAG(long _DAY_TAG){ setParamValue(0, _DAY_TAG); }
	public long get_DAY_TAG(){ return getParamValue(0); }

}
