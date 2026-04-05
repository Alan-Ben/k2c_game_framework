using GOE;
namespace Common
{
/****
 阶段目标系统错误
 ****/
    class StageGoalErr
    {
        static StageGoalErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360001, "阶段目标阶段奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360002, "阶段目标任务不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360003, "阶段目标任务已完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360004, "阶段目标任务未完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360005, "阶段目标大阶段奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360006, "阶段目标阶段未完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360007, "阶段目标大阶段首次达成奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360008, "阶段目标大阶段首次达成奖励未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360009, "阶段目标服务器开服天数不足以解锁下个阶段"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(360010, "阶段目标解锁条件不满足解锁下个阶段"));
        }
    }
}
