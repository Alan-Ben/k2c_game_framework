using GOE;
namespace Common
{
/****
 任务错误
 ****/
    class QuestErr
    {
        static QuestErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140001, "日常任务不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140002, "日常任务已经完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140003, "日常任务领取不满足分数需要"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140004, "日常任务尚未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140005, "日常任务活跃度不够"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140006, "日常任务活跃奖励已被领取"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140007, "日常任务刷新序列号不匹配"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140008, "日常任务一键完成功能未解锁"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140009, "任务不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140010, "任务完成次数超过限制错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140011, "任务开启条件未通过"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140012, "任务开启消耗物品不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140013, "任务开启消耗物品失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140014, "任务完成失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140015, "任务放弃失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(140016, "修改任务计数失败"));
        }
    }
}
