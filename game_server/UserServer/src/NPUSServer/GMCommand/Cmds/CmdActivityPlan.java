package NPUSServer.GMCommand.Cmds;

import NPCommon.Enum.EUsParam;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "活动计划", name = "activityPlan")
public class CmdActivityPlan extends UsCmdBase
{
    @ACommand(comment = "设置禁用状态[计划ID][是否禁用]")
    public String setPlanDisableState(long planId, boolean state)
    {
        return getUserServer().getActivityPlanMgr().cmdSetPlanDisableState(planId, state).toString();
    }

    @ACommand(comment = "删除计划[计划ID]")
    public String delPlan(long planId)
    {
        return getUserServer().getActivityPlanMgr().cmdDelPlan(planId).toString();
    }

    @ACommand(comment = "修改活动计划[计划ID][开始时间][结束时间][结算时间][关闭时间]")
    public String modifyPlan(long planId, long startTime, long endTime, long settleTime, long closeTime)
    {
        return getUserServer().getActivityPlanMgr().cmdModifyPlan(planId, startTime, endTime, settleTime, closeTime).toString();
    }

    @ACommand(comment = "增加活动计划[活动ID][开始时间][结束时间][结算时间][关闭时间]")
    public String addPlan(long activityId, long startTime, long endTime, long settleTime, long closeTime)
    {
        return getUserServer().getActivityPlanMgr().cmdAddPlan(activityId, startTime, endTime, closeTime).toString();
    }

    @ACommand(comment = "修改开服日期[日期(例:20250501)]")
    public String chgServerStartDate(int _date)
    {
        getUserServer().getUSParams().setParam(EUsParam.GM_SERVER_START_DATE, _date);
        getUserServer().getUsUserMgr().onServerStartDateChg();
        return "done";
    }

    @ACommand(comment = "修改初始化状态[是否初始化]")
    public String chgInitState(boolean state)
    {
        getUserServer().getUSParams().setParam(EUsParam.IS_ACTIVITY_SCHEDULE_INIT, state ? 1 : 0);
        return "done";
    }

    @ACommand(comment = "清除所有计划")
    public String clearAllPlan()
    {
        getUserServer().getActivityPlanMgr().cmdClearAllPlan();
        return "done";
    }

    @ACommand(comment = "打印所有计划")
    public String showAll()
    {
        return getUserServer().getActivityPlanMgr().toString();
    }
}
