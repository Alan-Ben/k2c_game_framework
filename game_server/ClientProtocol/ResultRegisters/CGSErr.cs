using GOE;
namespace Common
{
/****
 跨服游戏服务器相关报错
 ****/
    class CGSErr
    {
        static CGSErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(310001, "跨服游戏模块不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(310002, "跨服游戏实例不存在"));
        }
    }
}
