package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 七日目标相关错误
 ****/
 
public class SevenDayGoalsErr implements _IErrHolder
{
    public static final Result TASK_NOT_REACH = Result.constInit(400001,"任务未达成");
    public static final Result TASK_REWARD_HAD_DRAW = Result.constInit(400002,"任务奖励已领取");
    public static final Result SERVER_START_DAY_NOT_REACH = Result.constInit(400003,"服务器开服天数未达成");
    public static final Result STEP_REWARD_NOT_REACH = Result.constInit(400004,"阶段奖励未达成");
    public static final Result STEP_REWARD_HAD_DRAW = Result.constInit(400005,"阶段奖励已领取");
}
