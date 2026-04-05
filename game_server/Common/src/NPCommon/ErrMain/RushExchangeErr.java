package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 急速兑换系统错误
 ****/
 
public class RushExchangeErr implements _IErrHolder
{
    public static final Result EXCHANGE_COUNT_NOT_ENOUGH = Result.constInit(600001,"今日兑换次数已达上限");
    public static final Result EXCHANGE_IN_PROGRESS = Result.constInit(600002,"已有进行中的兑换");
    public static final Result EXCHANGE_TIME_NOT_REACH = Result.constInit(600003,"未到达可领奖时间");
    public static final Result EXCHANGE_NOT_FOUND = Result.constInit(600004,"没有兑换记录");
    public static final Result EXCHANGE_ALREADY_REWARDED = Result.constInit(600005,"已经领取过奖励");
    public static final Result EXCHANGE_CANNOT_REFRESH = Result.constInit(600006,"还没到刷新时间");
}
