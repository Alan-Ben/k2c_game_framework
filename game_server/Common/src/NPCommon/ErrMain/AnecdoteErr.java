package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 政务错误
 ****/
 
public class AnecdoteErr implements _IErrHolder
{
    public static final Result EVENT_NOT_EXIST = Result.constInit(150001,"政务事件不存在");
    public static final Result EVENT_EARNINGS_NOT_REACH = Result.constInit(150002,"不满足政务事件所需赚速");
    public static final Result EARNINGS_DIDNT_DRAW_FIRST_REWARD = Result.constInit(150003,"未领取首次奖励");
    public static final Result EARNINGS_HAD_DRAW_FIRST_REWARD = Result.constInit(150004,"已领取首次奖励");
    public static final Result EARNINGS_HAD_DRAW_FINAL_REWARD = Result.constInit(150005,"已领取最终奖励");
}
