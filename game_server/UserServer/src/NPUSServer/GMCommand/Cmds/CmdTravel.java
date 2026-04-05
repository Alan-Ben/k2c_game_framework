package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @description: 游历相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "游历", name = "travel")
public class CmdTravel extends UsCmdBase
{
	@ACommand(comment = "获取游历详细数据")
    public String info()
    {
        return getOwner().getTravelComponent().toString();
    }
	
    @ACommand(comment = "创建指定事件[事件ID，位置ID]")
    public String createEvent(long _eventId, long _posId)
    {
    	RefTravelEvent ref = RefTravelEvent.getMgr().get(_eventId);
    	if(null == ref)
    		return "fail, not find ref.";

    	getOwner().getTravelComponent().createEvent(_eventId, _posId, getContext());

        return "ok";
    }

    @ACommand(comment = "删除指定事件[事件ID]")
    public String delEvent(long _instanceId)
    {
    	getOwner().getTravelComponent().delEvent(_instanceId, getContext());
    	
    	return "ok";
    }
    
    @ACommand(comment = "删除指定事件的所有事件数据[事件ID]")
    public String delEventByEventId(long _eventId)
    {
    	getOwner().getTravelComponent().delEventByEventId(_eventId, getContext());
    	
    	return "ok";
    }
    
    @ACommand(comment = "清空所有已完成的一次性事件数据")
    public String clearAkeyDoneEvent()
    {
    	getOwner().getTravelComponent().clearAkeyDoneEvent(getContext());
    	
    	return "ok";
    }
    
    @ACommand(comment = "删除指定已完成的一次性事件数据[事件ID]")
    public String delAkeyDoneEvent(long _eventId)
    {
    	getOwner().getTravelComponent().delAkeyDoneEvent(_eventId, getContext());
    	
    	return "ok";
    }
    
    @ACommand(comment = "设置指定妃子的好感度[妃子ID，好感度]")
    public String setConsortLike(long _consortId, int _likeValue)
    {
    	getOwner().getTravelComponent().cmdConsortSetLike(_consortId, _likeValue, getContext());
    	
    	return "ok";
    }
}
