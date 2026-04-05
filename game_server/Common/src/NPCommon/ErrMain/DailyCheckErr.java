package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 每日签到错误
 ****/
 
public class DailyCheckErr implements _IErrHolder
{
    public static final Result DAILY_CHECK_HAS_CHECK = Result.constInit(160001,"每日签到今天已签到");
    public static final Result DAILY_CHECK_DESSERT_NOT_FOUND = Result.constInit(160002,"每日签到选择甜品存在");
}
