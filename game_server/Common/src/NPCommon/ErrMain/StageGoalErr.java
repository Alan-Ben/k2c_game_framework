package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 阶段目标系统错误
 ****/
 
public class StageGoalErr implements _IErrHolder
{
    public static final Result STAGE_GOAL_STEP_REWARD_HAD_DRAW = Result.constInit(360001,"阶段目标阶段奖励已领取");
    public static final Result STAGE_GOAL_TASK_NOT_FOUND = Result.constInit(360002,"阶段目标任务不存在");
    public static final Result STAGE_GOAL_TASK_ALREADY_DONE = Result.constInit(360003,"阶段目标任务已完成");
    public static final Result STAGE_GOAL_TASK_NOT_DONE = Result.constInit(360004,"阶段目标任务未完成");
    public static final Result STAGE_GOAL_BIG_STEP_REWARD_HAD_DRAW = Result.constInit(360005,"阶段目标大阶段奖励已领取");
    public static final Result STAGE_GOAL_STEP_NOT_DONE = Result.constInit(360006,"阶段目标阶段未完成");
    public static final Result STAGE_GOAL_BIG_STEP_FIRST_REACH_REWARD_HAD_DRAW = Result.constInit(360007,"阶段目标大阶段首次达成奖励已领取");
    public static final Result STAGE_GOAL_BIG_STEP_FIRST_REACH_REWARD_NOT_UNLOCK = Result.constInit(360008,"阶段目标大阶段首次达成奖励未解锁");
    public static final Result STAGE_GOAL_SERVER_START_DAY_NOT_ENOUGH_TO_NEXT_STEP = Result.constInit(360009,"阶段目标服务器开服天数不足以解锁下个阶段");
    public static final Result STAGE_GOAL_UNLOCK_CONDITION_NOT_MEET_TO_NEXT_STEP = Result.constInit(360010,"阶段目标解锁条件不满足解锁下个阶段");
}
