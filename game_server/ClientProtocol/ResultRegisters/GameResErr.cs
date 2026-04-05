using GOE;
namespace Common
{
/****
 服务器资源相关报错
 ****/
    class GameResErr
    {
        static GameResErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(260001, "游戏资源GM修改失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(260002, "游戏资源配表找不到"));
        }
    }
}
