package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
经营建筑雇佣员工
****/
@EventDesc(id=9,name="P_BUSSINESS_HIRE_EMPLOYEE",params={"NUM"})
public class Event_P_BUSSINESS_HIRE_EMPLOYEE extends _ALogicEventBase
{
	public static final int ID =9;
	public Event_P_BUSSINESS_HIRE_EMPLOYEE(_IContext _context,long _NUM)
	{
		super(_context);
		
		set_NUM(_NUM);

	}
	public Event_P_BUSSINESS_HIRE_EMPLOYEE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//数量
	public void set_NUM(long _NUM){ setParamValue(0, _NUM); }
	public long get_NUM(){ return getParamValue(0); }

}
