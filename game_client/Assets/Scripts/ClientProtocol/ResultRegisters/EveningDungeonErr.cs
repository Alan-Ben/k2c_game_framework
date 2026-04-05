using GOE;
namespace Common
{
/****
 晚间副本相关错误
 ****/
    class EveningDungeonErr
    {
        static EveningDungeonErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(390001, "大臣使用次数已达上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(390002, "BOSS已死亡"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(390003, "未在战斗时间内"));
        }
    }
}
