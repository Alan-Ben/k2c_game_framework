using GOE;
namespace Common
{
/****
 倒计时事件相关错误
 ****/
    class CountdownEventErr
    {
        static CountdownEventErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(420001, "没有进行中的倒计时事件"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(420002, "倒计时事件相关的任务不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(420004, "倒计时事件未超时"));
        }
    }
}
