package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 排期相关报错
 ****/
 
public class ScheduleErr implements _IErrHolder
{
    public static final Result CROSS_SERVER_GROUP_LIST_IS_EMPTY = Result.constInit(280001,"没有预备跨服分组信息");
    public static final Result CROSS_SERVER_GROUP_ID_HAD_USE = Result.constInit(280002,"跨服分组ID在往期已被使用");
    public static final Result CROSS_SERVER_GROUP_US_IN_OTHER_GROUP = Result.constInit(280003,"玩家服务器已经在其他跨服分组");
    public static final Result CROSS_SERVER_GROUP_ID_REPEAT = Result.constInit(280004,"跨服分组ID已存在");
    public static final Result CROSS_SERVER_GROUP_NOT_FOUND = Result.constInit(280005,"跨服分组不存在");
    public static final Result SCHEDULE_HAD_ACTIVE_CANT_UPDATE = Result.constInit(280006,"排期已激活不能更新");
}
