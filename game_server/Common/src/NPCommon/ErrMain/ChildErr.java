package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 子嗣系统错误
 ****/
 
public class ChildErr implements _IErrHolder
{
    public static final Result CHILD_NOT_EXISTS = Result.constInit(50001,"子嗣不存在");
    public static final Result CHILD_NAME_ERROR = Result.constInit(50002,"子嗣名称不符合要求");
    public static final Result CHILD_NAME_EXIST = Result.constInit(50003,"子嗣名称已存在");
    public static final Result CHILD_NAME_NOT_SET = Result.constInit(50004,"子嗣未命名");
    public static final Result CHILD_SEAT_NOT_EXIST = Result.constInit(50005,"子嗣训练位不存在");
    public static final Result CHILD_SEAT_NOT_UNLOCK = Result.constInit(50006,"子嗣训练位未解锁");
    public static final Result CHILD_SEAT_NOT_ENERGY = Result.constInit(50007,"子嗣训练位没有脑力值");
    public static final Result CHILD_LVL_FULL = Result.constInit(50008,"子嗣已满级");
    public static final Result CHILD_NOT_LVL_FULL = Result.constInit(50009,"子嗣未满级");
    public static final Result ADULT_COUNT_UNMARRY_FULL = Result.constInit(50010,"成年未婚子嗣数量到达上限");
    public static final Result TO_ME_APPLY_NOT_EXISTS = Result.constInit(50011,"联姻请求不存在");
    public static final Result ADULT_NOT_IDLE = Result.constInit(50012,"子嗣状态不是空闲");
    public static final Result ADULT_MARRY_FAIL = Result.constInit(50013,"子嗣联姻失败");
    public static final Result ADULT_NOT_APPLY_PLAYER = Result.constInit(50014,"子嗣状态不是指定联姻请求中");
    public static final Result ADULT_POOL_GROUP_NOT_MATCH = Result.constInit(50015,"子嗣联姻池不匹配");
    public static final Result ADULT_NOT_MATCH_SELF = Result.constInit(50016,"不能与自身子嗣进行联姻");
    public static final Result ADULT_POOL_NOT_FIND = Result.constInit(50017,"联姻池子嗣不存在");
    public static final Result ADULT_NOT_IN_POOL = Result.constInit(50018,"子嗣不存在联姻池");
    public static final Result NO_FREE_SEAT = Result.constInit(50019,"没有空闲位置");
    public static final Result RAND_GAIN_CHILD_FAIL = Result.constInit(50020,"随机获得子嗣失败");
    public static final Result ADULT_MATCH_MIN = Result.constInit(50021,"匹配子嗣需要超过最小值");
}
