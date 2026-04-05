using GOE;
namespace Common
{
/****
 成就系统错误
 ****/
    class AchieveErr
    {
        static AchieveErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(90001, "成就点类型不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(90002, "成就点阶段奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(90003, "成就点点数不够"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(90004, "成就不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(90005, "成就阶段奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(90006, "成就阶段分数不满足"));
        }
    }
}
