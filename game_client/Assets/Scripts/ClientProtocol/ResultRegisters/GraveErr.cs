using GOE;
namespace Common
{
/****
 杰出者系统错误
 ****/
    class GraveErr
    {
        static GraveErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(550001, "无法找到记录"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(550002, "今日无膜拜次数"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(550003, "无可以祝贺的新晋杰出者"));
        }
    }
}
