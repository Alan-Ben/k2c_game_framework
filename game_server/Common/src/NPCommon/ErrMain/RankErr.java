package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 排行榜错误
 ****/
 
public class RankErr implements _IErrHolder
{
    public static final Result INSTANCE_NO_EXIST = Result.constInit(100001,"跨服分组实例不存在");
    public static final Result RANK_NO_EXIST = Result.constInit(100002,"排行对象不存在");
    public static final Result RANK_OP_FAIL = Result.constInit(100003,"排行榜操作失败");
    public static final Result CROSS_RANK_HANDLE_FAIL = Result.constInit(100004,"跨服排行榜服务器承载失败");
    public static final Result RANK_FIXED_NOT_FOUND = Result.constInit(100005,"常驻排行榜未找到");
    public static final Result RANK_FIXED_LIKE_LIST_EMPTY = Result.constInit(100006,"常驻排行榜找不到点赞对象");
    public static final Result RANK_FIXED_LIKE_CD_NOT_ENOUGH = Result.constInit(100007,"常驻排行榜点赞次数不足");
    public static final Result RANK_FIXED_A_KEY_LIKE_NOT_UNLOCK = Result.constInit(100008,"常驻排行榜一键点赞功能未解锁");
    public static final Result RANK_FIXED_CANT_LIKE = Result.constInit(100009,"该常驻排行榜不支持点赞");
    public static final Result RANK_FIXED_NO_TARGET_CAN_LIKE = Result.constInit(100010,"常驻排行榜没有可点赞目标");
    public static final Result RANK_ITEM_NO_EXIST = Result.constInit(100011,"排行item数据不存在");
    public static final Result RANK_NOT_JOIN = Result.constInit(100012,"未参加排行");
}
