package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 后台相关报错
 ****/
 
public class HttpErr implements _IErrHolder
{
    public static final Result HTTP_RESPONSE_ERROR = Result.constInit(270001,"HTTP请求响应失败");
    public static final Result PLATFORM_RESPONSE_ERROR = Result.constInit(270002,"HTTP请求平台返回错误码");
    public static final Result PLATFORM_RESPONSE_DATA_ERROR = Result.constInit(270003,"HTTP请求平台返回数据");
    public static final Result PLATFORM_DECODE_DATA_ERROR = Result.constInit(270004,"HTTP请求平台返回数据解析错误");
    public static final Result ANNOUNCEMENT_NOT_FOUND = Result.constInit(270005,"找不到公告");
    public static final Result ANNOUNCEMENT_HAD_DRAW = Result.constInit(270006,"公告奖励已领取");
    public static final Result QUESTIONNAIRE_REWARD_NOT_FOUND = Result.constInit(270007,"找不到问卷奖励");
    public static final Result QUESTIONNAIRE_ACTIVITY_NOT_FOUND = Result.constInit(270008,"问卷活动不存在（玩家不可见）");
    public static final Result QUESTIONNAIRE_REWARD_HAD_DRAW = Result.constInit(270009,"问卷奖励已领取");
    public static final Result QUESTIONNAIRE_REWARD_ADD_REPEAT = Result.constInit(270010,"问卷奖励重复添加（玩家不可见）");
    public static final Result NOTIFY_ROLE_GAME_EVENT_TYPE_NOT_FOUND = Result.constInit(270011,"通知角色游戏事件类型找不到");
    public static final Result QUESTIONNAIRE_REWARD_LIST_PARSE_FAILED = Result.constInit(270012,"问卷奖励列表解析失败");
    public static final Result QUESTIONNAIRE_ACTIVITY_REPEATED = Result.constInit(270013,"问卷活动重复");
    public static final Result QUESTIONNAIRE_ACTIVITY_CLOSED = Result.constInit(270014,"问卷活动已关闭");
    public static final Result PLATFORM_DEAL_MSG_ERROR = Result.constInit(270015,"平台消息处理错误");
}
