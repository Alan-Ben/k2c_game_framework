package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 好友错误
 ****/
 
public class FriendErr implements _IErrHolder
{
    public static final Result FRIEND_SYS_ERROR = Result.constInit(120001,"好友系统错误");
    public static final Result FRIEND_EXISTED_ERROR = Result.constInit(120002,"已经是好友");
    public static final Result FRIEND_LIMIT_ERROR = Result.constInit(120003,"好友数量上限");
    public static final Result TARGET_FRIEND_LIMIT_ERROR = Result.constInit(120004,"对方好友数量上限");
    public static final Result FRIEND_APPLY_NOT_EXIST = Result.constInit(120005,"好友申请不存在错误");
    public static final Result FRIEND_NOT_EXISTED_ERROR = Result.constInit(120006,"找不到指定好友");
    public static final Result TARGET_FRIEND_APPLY_LIMIT_ERROR = Result.constInit(120008,"对方好友申请数量上限");
    public static final Result FRIEND_GROUP_NOT_FOUND = Result.constInit(120009,"找不到好友分组");
    public static final Result FRIEND_GROUP_REACH_LIMIT = Result.constInit(120010,"好友分组数量达到上限");
}
