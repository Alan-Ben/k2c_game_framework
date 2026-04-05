using GOE;
namespace Common
{
/****
 通用服务器相关报错
 ****/
    class CSErr
    {
        static CSErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(290001, "找不到合适的负载对象"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(290002, "服务器对象不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(290003, "玩家服务器信息列表未就绪"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(290004, "玩家服务器信息不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(290006, "US信息设置从文件获取"));
        }
    }
}
