package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
子嗣毕业
****/
@EventDesc(id=60,name="P_TRAN_ADULT",params={"QUALITY","COUNT"})
public class Event_P_TRAN_ADULT extends _ALogicEventBase
{
	public static final int ID =60;
	public Event_P_TRAN_ADULT(_IContext _context,long _QUALITY,long _COUNT)
	{
		super(_context);
		
		set_QUALITY(_QUALITY);
		set_COUNT(_COUNT);

	}
	public Event_P_TRAN_ADULT(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//品质
	public void set_QUALITY(long _QUALITY){ setParamValue(0, _QUALITY); }
	public long get_QUALITY(){ return getParamValue(0); }
	//数量
	public void set_COUNT(long _COUNT){ setParamValue(1, _COUNT); }
	public long get_COUNT(){ return getParamValue(1); }

}
