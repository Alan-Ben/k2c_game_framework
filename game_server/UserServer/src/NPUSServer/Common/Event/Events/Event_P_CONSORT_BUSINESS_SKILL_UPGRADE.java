package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
妃子经营技能升级
****/
@EventDesc(id=14,name="P_CONSORT_BUSINESS_SKILL_UPGRADE",params={})
public class Event_P_CONSORT_BUSINESS_SKILL_UPGRADE extends _ALogicEventBase
{
	public static final int ID =14;
	public Event_P_CONSORT_BUSINESS_SKILL_UPGRADE(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_CONSORT_BUSINESS_SKILL_UPGRADE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}
