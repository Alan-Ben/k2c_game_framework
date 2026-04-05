package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
全服邮件变更
****/
@EventDesc(id=2,name="S_ALL_SERVER_MAIL_CHG",params={"MAX_MAIL_ID"})
public class Event_S_ALL_SERVER_MAIL_CHG extends _ALogicEventBase
{
	public static final int ID =2;
	public Event_S_ALL_SERVER_MAIL_CHG(_IContext _context,long _MAX_MAIL_ID)
	{
		super(_context);
		
		set_MAX_MAIL_ID(_MAX_MAIL_ID);

	}
	public Event_S_ALL_SERVER_MAIL_CHG(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	
	//最大邮件id
	public void set_MAX_MAIL_ID(long _MAX_MAIL_ID){ setParamValue(0, _MAX_MAIL_ID); }
	public long get_MAX_MAIL_ID(){ return getParamValue(0); }

}
