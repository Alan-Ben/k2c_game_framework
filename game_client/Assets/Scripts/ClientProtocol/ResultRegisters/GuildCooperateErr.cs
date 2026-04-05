using GOE;
namespace Common
{
/****
 联盟协作相关错误
 ****/
    class GuildCooperateErr
    {
        static GuildCooperateErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480001, "属性据点不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480002, "奖励据点不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480003, "奖励据点已击败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480004, "区域未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480006, "大臣已使用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480007, "大臣未使用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480008, "奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480009, "奖励据点未击败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(480010, "属性据点已击败"));
        }
    }
}
