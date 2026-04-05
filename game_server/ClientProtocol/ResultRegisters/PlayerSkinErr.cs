using GOE;
namespace Common
{
/****
 玩家形象相关错误
 ****/
    class PlayerSkinErr
    {
        static PlayerSkinErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(180001, "未找到玩家称号"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(180002, "玩家称号已存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(180003, "未找到玩家皮肤"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(180004, "玩家皮肤已存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(180005, "玩家皮肤已满级"));
        }
    }
}
