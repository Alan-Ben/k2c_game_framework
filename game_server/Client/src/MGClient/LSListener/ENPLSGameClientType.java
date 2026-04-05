package MGClient.LSListener;

public enum ENPLSGameClientType
{
    NONE,
    USER,                   //正常的用户名登录
    USER_CHECK_CODE,        //用户使用验证串登录
    VISITORS,               //游客登录
    CHEAT,                  //作弊登录，username为uid
    USER_W_LIST,            //白名单登录
    USER_CHECK_CODE_W_LIST, //白名单验证码登录
}
