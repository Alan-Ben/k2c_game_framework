package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 订单相关错误
 ****/
 
public class OrderErr implements _IErrHolder
{
    public static final Result ORDER_CANT_OPERATE = Result.constInit(470001,"订单已失效");
    public static final Result ORDER_NOT_EXIST = Result.constInit(470002,"订单不存在");
    public static final Result NOT_SUPPORT_PAY_TYPE = Result.constInit(470003,"不支持现金购买");
    public static final Result PURCHASE_LIMIT_REACHED = Result.constInit(470004,"购买次数已达上限");
    public static final Result VOUCHER_NOT_ENOUGH = Result.constInit(470005,"代金券不足");
    public static final Result RELATIVE_ACTIVITY_NOT_OPEN = Result.constInit(470006,"礼包关联活动未开启");
    public static final Result PLAT_NOTIFY_PAY_UID_NOT_MATCH = Result.constInit(470007,"平台回调支付用户ID不匹配");
    public static final Result CREATE_ORDER_FAILED = Result.constInit(470008,"创建订单失败");
    public static final Result PLAT_NOTIFY_PAY_INFO_NOT_MATCH = Result.constInit(470009,"平台回调支付信息不匹配");
    public static final Result PUSH_GIFT_TRIGGER_NOT_EXPIRED = Result.constInit(470010,"推送礼包未过期");
    public static final Result PUSH_GIFT_TRIGGER_COOLDOWN = Result.constInit(470011,"触发冷却中");
    public static final Result PUSH_GIFT_NOT_ACTIVE = Result.constInit(470012,"推送礼包未激活");
    public static final Result PUSH_GIFT_CANT_AUTO_TRIGGER = Result.constInit(470013,"推送礼包无法自动触发");
    public static final Result PUSH_GIFT_NO_NEXT_CAN_TRIGGER = Result.constInit(470014,"推送礼包没有下一个可触发礼包");
}
