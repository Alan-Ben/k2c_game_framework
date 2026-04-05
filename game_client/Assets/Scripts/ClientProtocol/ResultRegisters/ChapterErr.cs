using GOE;
namespace Common
{
/****
 关卡系统错误
 ****/
    class ChapterErr
    {
        static ChapterErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30001, "关卡BOSS未击败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30002, "关卡点位不符"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30003, "关卡BOSS已击败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30004, "关卡攻击BOSS战力不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30005, "关卡没有在攻击BOSS"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30006, "关卡未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30007, "关卡事件未完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30008, "没有可以处理的关卡事件"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(30009, "关卡剧情奖励已领取"));
        }
    }
}
