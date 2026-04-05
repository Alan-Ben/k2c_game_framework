package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.Common.Event.Events.Event_P_GAIN_CHILD;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.ChildSystem;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildSeatInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;

@ACommander(comment = "子嗣系统", name = "child")
public class CmdChild extends UsCmdBase
{
	@ACommand(comment = "子嗣数据")
    public String info()
    {
		StringBuilder sb = new StringBuilder();

		sb.append("\n============ player =============");
		sb.append("\nall child bonus:").append(getOwner().getChildComponent().getBonusSum());
		
		sb.append("\n============ child =============");
		sb.append(getOwner().getChildComponent().getChildMgr().toString());
		
		sb.append("\n============ adult =============");
		sb.append(getOwner().getChildComponent().getAdultMgr().toString());
		
		return sb.toString();
    }
	
	@ACommand(comment = "获取随机子嗣[是否卷王]")
    public String gainRndChild(boolean _isGiftde)
    {
		ConsortInfo consort = getOwner().getConsortComponent().lookupRnd();
		if(null == consort)
		{
			return "fail, not find consort.";
		}
		
		ChildInfo child = ChildSystem.createChild(consort, _isGiftde, null, getContext());
		if(null == child)
		{
			return "fail, create child fail.";
		}

        //获得子嗣事件
        Event_P_GAIN_CHILD evt = new Event_P_GAIN_CHILD(getContext(), 1);
        getOwner().onLogicEvent(evt);
		
        return "ok, child id: " + child.getChildId();
    }
	
	@ACommand(comment = "设置子嗣基础收益[子嗣ID，基础收益数值]")
    public String setBaseBonus(long _childId, long _baseBonus)
    {
		ChildInfo child = getOwner().getChildComponent().getChildMgr().lookupChild(_childId);
		if(null == child)
			return "fail, not find child.";
		
		child.getBo().saveBaseBonus(getOwner().getUSServer().getBM(), _baseBonus);
		
		return "ok";
    }
	
	@ACommand(comment = "增加脑力值[座位ID, 增加值]")
    public String incrSeatEnery(long _seatId, int _addValue)
    {
		ChildSeatInfo seat = getOwner().getChildComponent().getChildMgr().lookupSeat(_seatId);
		if(null == seat)
		{
			return "fail, not find seat.";
		}
		
		seat.incrEnergy(_addValue, getContext());
		
		return "ok";
    }
	
	@ACommand(comment = "移除训练位上的子嗣[训练位ID 0-全部删除]")
    public String delChildBySeat(long _seatId)
    {
		getOwner().getChildComponent().getChildMgr().cmdDelChildBySeat(_seatId, getContext());
		
        return "ok";
    }
}
