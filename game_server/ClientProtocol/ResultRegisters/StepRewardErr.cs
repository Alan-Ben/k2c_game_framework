using GOE;
namespace Common
{
/****
 阶段奖励相关错误
 ****/
    class StepRewardErr
    {
        static StepRewardErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(220001, "阶段奖励对象不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(220002, "阶段奖励事件分数需要更大值"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(220003, "阶段奖励事件分数达到最大值"));
        }
    }
}
