package <%PackageName%>;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
<%Comment%>
****/
@EventDesc(id=<%id%>,name="<%EventName%>",params={<%ParamNameList%>})
public class Event_<%EventName%> extends _ALogicEventBase
{
	public static final int ID =<%id%>;
	public Event_<%EventName%>(_IContext _context<%paramList%>)
	{
		super(_context);
		
<%InitParams%>
	}
	public Event_<%EventName%>(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
<%GetterSetter%>
}
