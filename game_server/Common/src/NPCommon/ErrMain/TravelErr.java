package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 游历错误
 ****/
 
public class TravelErr implements _IErrHolder
{
    public static final Result TRAVEL_EVENT_CREATE_FAIL = Result.constInit(170001,"游历事件创建失败");
    public static final Result TRAVEL_EVENT_NOT_FOUND = Result.constInit(170002,"游历事件未找到");
    public static final Result TRAVEL_EVENT_NOT_DEAL = Result.constInit(170003,"游历事件未处理");
    public static final Result TRAVEL_EVENT_CONSORT_NOT_IN = Result.constInit(170004,"游历事件不属于指定妃子列表");
    public static final Result TRAVEL_COST_NOT_ENOUGH = Result.constInit(170005,"游历事件消耗不足");
    public static final Result TRAVEL_COST_FAIL = Result.constInit(170006,"游历事件消耗失败");
    public static final Result TRAVEL_GAMBLE_BET_INVALID = Result.constInit(170007,"博彩押注额度不合法");
}
