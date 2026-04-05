using GOE;
namespace Common
{
/****
 午间副本相关错误
 ****/
    class MiddayDungeonErr
    {
        static MiddayDungeonErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380001, "大臣使用次数已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380002, "已经借用过联盟大臣"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380003, "借用大臣次数已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380004, "已经领取过宝箱"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380005, "领取宝箱次数已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380006, "宝箱已过期"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380007, "宝箱已领取完"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(380008, "未在战斗时间内"));
        }
    }
}
