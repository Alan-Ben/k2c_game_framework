using GOE;
namespace Common
{
/****
 政务错误
 ****/
    class AnecdoteErr
    {
        static AnecdoteErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(150001, "政务事件不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(150002, "不满足政务事件所需赚速"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(150003, "未领取首次奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(150004, "已领取首次奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(150005, "已领取最终奖励"));
        }
    }
}
