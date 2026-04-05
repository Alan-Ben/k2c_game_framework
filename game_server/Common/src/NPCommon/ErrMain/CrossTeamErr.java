package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 跨服组队系统错误
 ****/
 
public class CrossTeamErr implements _IErrHolder
{
    public static final Result GROUP_NOT_FOUND = Result.constInit(60001,"分组不存在");
    public static final Result TEAM_NOT_FOUND = Result.constInit(60002,"队伍不存在");
    public static final Result TEAM_MEMBER_NOT_FOUND = Result.constInit(60003,"未加入队伍");
    public static final Result TEAM_MEMBER_EXISTED = Result.constInit(60004,"已加入队伍");
    public static final Result TEAM_CREATE_FAIL = Result.constInit(60005,"创建队伍失败");
    public static final Result TEAM_JOIN_FAIL = Result.constInit(60006,"加入队伍失败");
    public static final Result TEAM_MEMBER_NOT_LEADER = Result.constInit(60007,"不是队长");
    public static final Result TEAM_NOT_FREE_JOIN = Result.constInit(60008,"不允许自由加入队伍");
    public static final Result TEAM_MEMBER_FULL = Result.constInit(60009,"队伍已满员");
    public static final Result TEAM_LEADER_NOT_QUIT = Result.constInit(60010,"队长不能退出队伍");
    public static final Result TEAM_APPLY_COND_FAIL = Result.constInit(60011,"不满足队伍申请条件");
    public static final Result TEAM_APPLY_NOT_FOUND = Result.constInit(60012,"申请数据不存在");
    public static final Result TEAM_APPLY_COND_ERROR = Result.constInit(60013,"申请条件类型错误");
    public static final Result TEAM_APPLY_ALREADY_EXIST = Result.constInit(60014,"已申请加入队伍");
    public static final Result TEAM_FORBID_JOIN = Result.constInit(60015,"队伍已禁止加入");
}
