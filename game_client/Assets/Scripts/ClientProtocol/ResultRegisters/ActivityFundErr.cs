using GOE;
namespace Common
{
/****
 活动基金
 ****/
    class ActivityFundErr
    {
        static ActivityFundErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(590001, "基金不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(590002, "已经领取过该阶段奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(590003, "等级配置不存在"));
        }
    }
}
