package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 阶段奖励相关错误
 ****/
 
public class StepRewardErr implements _IErrHolder
{
    public static final Result STEP_REWARD_NO_EXIST = Result.constInit(220001,"阶段奖励对象不存在");
    public static final Result STEP_EVENT_SCORE_GT = Result.constInit(220002,"阶段奖励事件分数需要更大值");
    public static final Result STEP_EVENT_SCORE_MAX = Result.constInit(220003,"阶段奖励事件分数达到最大值");
}
