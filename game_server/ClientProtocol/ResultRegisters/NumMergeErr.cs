using GOE;
namespace Common
{
/****
 数字合并相关错误
 ****/
    class NumMergeErr
    {
        static NumMergeErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570001, "数字合并模式配置不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570002, "数字合并无效移动"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570003, "数字合并游戏结束（死局）"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570004, "数字合并活动不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570005, "数字合并活动未开启"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570006, "数字合并模式解锁失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570007, "数字合并未死局"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570008, "方块索引无效"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570009, "目标位置无方块"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570010, "宝箱配置不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(570011, "宝箱积分不足"));
        }
    }
}
