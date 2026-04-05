package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 成就系统错误
 ****/
 
public class AchieveErr implements _IErrHolder
{
    public static final Result ACHIEVE_POINT_NOT_EXIST = Result.constInit(90001,"成就点类型不存在");
    public static final Result ACHIEVE_POINT_STEP_REWARD_HAD_DRAW = Result.constInit(90002,"成就点阶段奖励已领取");
    public static final Result ACHIEVE_POINT_NOT_ENOUGH = Result.constInit(90003,"成就点点数不够");
    public static final Result ACHIEVE_NOT_ENOUGH = Result.constInit(90004,"成就不存在");
    public static final Result ACHIEVE_STEP_REWARD_HAD_DRAW = Result.constInit(90005,"成就阶段奖励已领取");
    public static final Result ACHIEVE_STEP_NOT_SATISFIED = Result.constInit(90006,"成就阶段分数不满足");
}
