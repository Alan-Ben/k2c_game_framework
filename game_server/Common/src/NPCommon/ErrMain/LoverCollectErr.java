package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 情人收集错误
 ****/
 
public class LoverCollectErr implements _IErrHolder
{
    public static final Result ALREADY_HAS_TARGET = Result.constInit(610001,"已选择情人目标，不可修改");
    public static final Result NO_TARGET = Result.constInit(610002,"未选择情人目标");
    public static final Result ALREADY_CLAIMED = Result.constInit(610003,"情人已领取");
    public static final Result EARN_SPEED_NOT_ENOUGH = Result.constInit(610004,"赚速不足");
    public static final Result INVALID_LOVER_ID = Result.constInit(610005,"情人ID不在可选列表中");
}
