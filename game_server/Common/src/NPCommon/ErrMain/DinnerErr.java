package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 宴会错误
 ****/
 
public class DinnerErr implements _IErrHolder
{
    public static final Result DINNER_OWNER_REWARD = Result.constInit(110001,"宴会未领取开宴奖励");
    public static final Result DINNER_STARTED = Result.constInit(110002,"宴会已开启");
    public static final Result DINNER_NOT_PERMIT = Result.constInit(110003,"宴会不允许开启");
    public static final Result DINNER_PERMIT_NOT_FOUND = Result.constInit(110004,"宴会凭证不存在");
    public static final Result DINNER_NOT_FOUND = Result.constInit(110005,"宴会不存在");
    public static final Result DINNER_END = Result.constInit(110006,"宴会已结束");
    public static final Result DINNER_FULL = Result.constInit(110007,"宴会已经满座");
    public static final Result DINNER_JOINED = Result.constInit(110008,"已经加入宴会");
    public static final Result DINNER_IS_OWN = Result.constInit(110009,"是自己举办的宴会");
    public static final Result DINNER_OWNER_REWARD_NOT_FOUND = Result.constInit(110010,"没有开宴奖励");
    public static final Result DINNER_OWNER_REWARD_FAIL = Result.constInit(110011,"开宴奖励领取失败");
    public static final Result DINNER_JOIN_CD_NOT_ENOUGH = Result.constInit(110012,"赴宴次数限制");
}
