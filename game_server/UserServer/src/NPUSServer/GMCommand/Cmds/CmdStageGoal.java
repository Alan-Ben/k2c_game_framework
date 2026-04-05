package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.StageGlobalComp.StageGoalTaskInfo;

/**
 * @description: 阶段目标相关命令
 * @author: mark
 * @date: 2022-04-27 14:17:59
 */
@ACommander(comment = "阶段目标", name = "stageGoal")
public class CmdStageGoal extends UsCmdBase
{
    @ACommand(comment = "当前阶段目标数据")
    public String info()
    {
        return getOwner().getStageGoalComponent().toString();
    }
    
    @ACommand(comment = "更新当前阶段目标[任务ID，任务计数]")
    public String setTaskCounter(long _taskId, long _counter)
    {
    	StageGoalTaskInfo task = getOwner().getStageGoalComponent().lookupTask(_taskId);
    	if(null == task)
    		return "fail, not find task";
    	
    	task.setCounter(_counter, getContext());
    	
    	return "ok";
    }
    
    @ACommand(comment = "更新当前阶段目标")
    public String setStage(long _step)
    {
    	if(getOwner().getStageGoalComponent().cmdSetStep(_step, getContext()))
    		return "ok";

    	return "fail";
    }

    @ACommand(comment = "清空所有首达记录")
    public String clearFirstReach()
    {
        getUserServer().getStageGoalFirstReachMgr().clearAllRecords();
        return "done";
    }

    @ACommand(comment = "清空大阶段奖励领取记录")
    public String clearBigStepDrawRecords()
    {
        getOwner().getStageGoalComponent().clearBigStepDrawRecords();
        return "done";
    }

    @ACommand(comment = "清空大阶段首达奖励领取记录")
    public String clearBigStepFirstReachDrawRecords()
    {
        getOwner().getStageGoalComponent().clearBigStepFirstReachDrawRecords();
        return "done";
    }
}
