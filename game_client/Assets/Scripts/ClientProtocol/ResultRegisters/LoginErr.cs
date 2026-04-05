using GOE;
namespace Common
{
/****
 玩家登录相关错误
 ****/
    class LoginErr
    {
        static LoginErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240001, "SDK登录服务不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240002, "登录校验数据库不可用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240003, "登录找不到用户信息"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240004, "增加account信息错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240005, "登录校验码错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240006, "SDK返回内容错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240007, "玩家数据加载失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240008, "GS生成用户验证码错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240009, "玩家服务器状态未就绪时登录"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(240010, "找不到推荐服务器"));
        }
    }
}
