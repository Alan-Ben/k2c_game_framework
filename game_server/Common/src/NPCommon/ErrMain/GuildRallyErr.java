package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 联盟集结相关错误
 ****/
 
public class GuildRallyErr implements _IErrHolder
{
    public static final Result RALLY_NOT_FOUND = Result.constInit(620001,"集结不存在");
    public static final Result RALLY_MEMBER_EXIST = Result.constInit(620002,"成员已在集结中");
    public static final Result RALLY_OP_DISABLE = Result.constInit(620003,"集结操作无效");
    public static final Result RALLY_ALREADY_EXIST = Result.constInit(620004,"集结已存在");
    public static final Result RALLY_MEMBER_FULL = Result.constInit(620005,"集结人数已满");
    public static final Result RALLY_EXPIRED = Result.constInit(620006,"集结已过期");
    public static final Result RALLY_TEAM_STATE_INVALID = Result.constInit(620007,"队伍状态非法");
    public static final Result RALLY_TEAM_IN_OTHER_RALLY = Result.constInit(620008,"队伍已在其他集结中");
    public static final Result RALLY_POWER_NOT_ENOUGH = Result.constInit(620009,"战力不足无法加入集结");
    public static final Result RALLY_PERMISSION_DENIED = Result.constInit(620010,"无权限执行集结操作");
}
