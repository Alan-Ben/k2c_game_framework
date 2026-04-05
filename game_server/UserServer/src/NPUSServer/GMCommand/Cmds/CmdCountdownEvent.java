package NPUSServer.GMCommand.Cmds;

import Common.PlayerEnum.EPlayerEventRecordType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @description: 倒计时事件作弊命令
 */
@ACommander(comment = "倒计时事件作弊命令", name = "CountdownEvent")
public class CmdCountdownEvent extends UsCmdBase
{
    @ACommand(comment = "移除当前正在进行的倒计时事件")
    public String removeOngoingEvent()
    {
        return getOwner().getCountdownEventComponent().removeOngoingEvent(getContext()).toString();
    }

    @ACommand(comment = "增加事件[事件id][数量]")
    public String addEvent(long _eventId,long _count)
    {
        getOwner().getCountdownEventComponent().addEvent(_eventId, _count, getContext(),true);
        return "ok";
    }

    @ACommand(comment = "重置事件可触发次数[事件id]")
    public String resetCanTriggerTimes(long _eventId)
    {
        getOwner().getEventRecordComp().setRecord(EPlayerEventRecordType.COUNTDOWN_EVENT_ADD_TIMES.ordinal(), _eventId, 0);
        return "ok";
    }
}
