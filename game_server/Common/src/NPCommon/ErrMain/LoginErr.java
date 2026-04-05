package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 玩家登录相关错误
 ****/
 
public class LoginErr implements _IErrHolder
{
    public static final Result LOGIN_SDK_ENTER_SERVICE_UNAVAILABLE = Result.constInit(240001,"SDK登录服务不可用");
    public static final Result LOGIN_CHECK_DB_UNAVAILABLE = Result.constInit(240002,"登录校验数据库不可用");
    public static final Result LOGIN_NO_USER_INFO = Result.constInit(240003,"登录找不到用户信息");
    public static final Result LOGIN_ADD_ACC_INFO_FAIL = Result.constInit(240004,"增加account信息错误");
    public static final Result LOGIN_CHECK_CODE_ERR = Result.constInit(240005,"登录校验码错误");
    public static final Result LOGIN_SDK_RESPONSE_ERR = Result.constInit(240006,"SDK返回内容错误");
    public static final Result PLAYER_DATA_LOAD_FAIL = Result.constInit(240007,"玩家数据加载失败");
    public static final Result GS_GEN_USER_CODE_ERR = Result.constInit(240008,"GS生成用户验证码错误");
    public static final Result LOGIN_WHEN_US_NOT_READY = Result.constInit(240009,"玩家服务器状态未就绪时登录");
    public static final Result RECOMMEND_SERVER_NOT_FOUND = Result.constInit(240010,"找不到推荐服务器");
}
