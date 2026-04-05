package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "七日任务相关", name = "sevenGoals")
public class CmdSevenDayGoals extends UsCmdBase
{
    @ACommand(comment = "增加计数[任务id][数值]")
    public String addCounter(long _taskId,long _count)
    {
        return getOwner().getSevenDayGoalsComponent().addCounter(_taskId, _count, getContext()).toString();
    }

    @ACommand(comment = "重置计数[任务id]")
    public String resetCounter(long _taskId)
    {
        return getOwner().getSevenDayGoalsComponent().resetCounter(_taskId, getContext()).toString();
    }

    @ACommand(comment = "增加阶段奖励分数[分数]")
    public String addScore(int _score)
    {
        getOwner().getSevenDayGoalsComponent().gainScore(_score, getContext());
        return "done";
    }

}
