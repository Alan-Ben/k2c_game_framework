using GOE;
namespace Common
{
/****
 博物馆相关错误
 ****/
    class MuseumErr
    {
        static MuseumErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(440001, "博物馆藏品已激活"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(440002, "博物馆藏品未激活"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(440003, "博物馆藏品未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(440004, "博物馆藏品等级已达上限"));
        }
    }
}
