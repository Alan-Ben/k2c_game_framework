package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 大学系统错误
 ****/
 
public class CollegeErr implements _IErrHolder
{
    public static final Result COLLEGE_EXTRA_SEAT_NUM_REACH_LIMIT = Result.constInit(70001,"大学额外座位数量达到上限");
    public static final Result COLLEGE_EXTRA_SEAT_UNLOCK_FEE_CALCULATE_FAIL = Result.constInit(70002,"大学解锁额外座位费用计算错误");
    public static final Result COLLEGE_SEAT_NOT_UNLOCK = Result.constInit(70003,"大学座位未解锁");
    public static final Result COLLEGE_SEAT_HAS_OCCUPY = Result.constInit(70004,"大学座位已被占用");
    public static final Result COLLEGE_SEAT_IS_EMPTY = Result.constInit(70005,"大学座位上没有大臣在学习");
    public static final Result COLLEGE_SEAT_NOT_REACH_FINISH_TIME = Result.constInit(70006,"大学还没有达到完成时间");
    public static final Result COLLEGE_A_KEY_FUNC_NOT_UNLOCK = Result.constInit(70007,"大学一键功能未解锁");
    public static final Result COLLEGE_ALL_SEAT_ARE_OCCUPY = Result.constInit(70008,"大学没有可用位置");
    public static final Result COLLEGE_HERO_ALREADY_IN_SEAT = Result.constInit(70009,"大学大臣已经在学习了");
}
