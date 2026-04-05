using GOE;
namespace Common
{
/****
 活动相关错误
 ****/
    class ActivityErr
    {
        static ActivityErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350001, "活动排行榜奖励还在结算中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350002, "活动排行榜没有奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350003, "活动排行榜奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350004, "活动阶段奖励已领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350005, "活动阶段奖励阶段未达成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350006, "活动不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350007, "活动不在领奖期"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350008, "活动排行榜不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350009, "活动阶段奖励不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350010, "活动商店不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350011, "活动商店还没到刷新时间"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350012, "活动钻石礼包还没到刷新时间"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350013, "活动钻石礼包达到购买上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350014, "活动礼包组不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350015, "活动关联排期不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350016, "活动类型错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350017, "不能踢出自己"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(350018, "玩家退出群组"));
        }
    }
}
