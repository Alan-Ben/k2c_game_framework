package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "周卡", name = "weekcard")
public class CmdWeekCard extends UsCmdBase
{
    @ACommand(comment = "获得周卡[时长 秒]")
    public String gainTimeSec(int _value)
    {
        getOwner().getWeekCardComponent().gainTimeSec(_value, getContext());
        return "remain time sec:" + getOwner().getWeekCardComponent().getRemainTimeMs() / 1000;
    }

    @ACommand(comment = "移除周卡[时长 秒]")
    public String consumeTimeSec(int _value)
    {
        getOwner().getWeekCardComponent().consumeTimeSec(_value, getContext());
        return "remain time sec:" + getOwner().getWeekCardComponent().getRemainTimeMs() / 1000;
    }

    @ACommand(comment = "移除周卡")
    public String cleanTime()
    {
        getOwner().getWeekCardComponent().consumeTimeSec(Integer.MAX_VALUE, getContext());
        return "remain time sec:" + getOwner().getWeekCardComponent().getRemainTimeMs() / 1000;
    }

    @ACommand(comment = "重置试用标记")
    public String resetTrailFlag()
    {
        getOwner().getWeekCardComponent().resetTrailFlag(getContext());
        return "ok";
    }
}
