using GOE;
namespace Common
{
/****
 急速兑换系统错误
 ****/
    class RushExchangeErr
    {
        static RushExchangeErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(600001, "今日兑换次数已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(600002, "已有进行中的兑换"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(600003, "未到达可领奖时间"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(600004, "没有兑换记录"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(600005, "已经领取过奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(600006, "还没到刷新时间"));
        }
    }
}
