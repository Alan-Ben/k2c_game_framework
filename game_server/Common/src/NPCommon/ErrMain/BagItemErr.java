package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 背包道具错误
 ****/
 
public class BagItemErr implements _IErrHolder
{
    public static final Result BAG_ITEM_ERROR = Result.constInit(330001,"背包物品错误");
    public static final Result BAG_ITEM_NOT_EXISTS_ERROR = Result.constInit(330002,"背包物品不存在");
    public static final Result BAG_ITEM_CAN_NOT_SELL_ERROR = Result.constInit(330003,"背包物品不能出售");
    public static final Result BAG_ITEM_CAN_NOT_USE_ERROR = Result.constInit(330004,"背包物品不能使用");
    public static final Result BAG_ITEM_USE_CONDITION_ERROR = Result.constInit(330005,"背包物品使用条件错误");
    public static final Result BAG_ITEM_USE_OPTION_ERROR = Result.constInit(330006,"背包物品使用下多选奖励物品错误");
    public static final Result BAG_ITEM_A_KEY_CONVERT_CHECK_FAIL = Result.constInit(330007,"背包道具一键转换校验失败");
}
