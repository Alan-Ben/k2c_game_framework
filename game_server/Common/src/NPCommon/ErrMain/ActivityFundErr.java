package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 活动基金
 ****/
 
public class ActivityFundErr implements _IErrHolder
{
    public static final Result FUND_NOT_FOUND = Result.constInit(590001,"基金不存在");
    public static final Result ALREADY_DRAWN = Result.constInit(590002,"已经领取过该阶段奖励");
    public static final Result LEVEL_NOT_FOUND = Result.constInit(590003,"等级配置不存在");
}
