package NPUSServer.Common.Event.Events;

import NPCommon.Context._IContext;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;

/****
玩家升级
****/
@EventDesc(id=20,name="P_PLAYER_UPGRADE",params={})
public class Event_P_PLAYER_UPGRADE extends _ALogicEventBase
{
	public static final int ID =20;
	public Event_P_PLAYER_UPGRADE(_IContext _context)
	{
		super(_context);
		

	}
	public Event_P_PLAYER_UPGRADE(_IContext _context, long[] _params)
	{
		super(_params,_context);
	}
	

}
