using GOE;
namespace Common
{
/****
 平台服务器相关报错
 ****/
    class PSErr
    {
        static PSErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(300001, "未找到区域信息"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(300002, "未找到GS"));
        }
    }
}
