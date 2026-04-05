using GOE;
namespace Common
{
/****
 七日目标相关错误
 ****/
    class SevenDayGoalsErr
    {
        static SevenDayGoalsErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(400001, "任务未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(400002, "任务奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(400003, "服务器开服天数未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(400004, "阶段奖励未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(400005, "阶段奖励已领取"));
        }
    }
}
