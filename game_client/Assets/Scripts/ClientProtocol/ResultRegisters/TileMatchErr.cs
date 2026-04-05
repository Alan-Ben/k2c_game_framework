using GOE;
namespace Common
{
/****
 三消相关错误
 ****/
    class TileMatchErr
    {
        static TileMatchErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(410001, "三消交换不合法"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(410002, "三消模式解锁失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(410003, "三消阶段奖励为空"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(410004, "三消未死局"));
        }
    }
}
