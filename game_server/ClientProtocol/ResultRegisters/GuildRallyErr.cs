using GOE;
namespace Common
{
/****
 联盟集结相关错误
 ****/
    class GuildRallyErr
    {
        static GuildRallyErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620001, "集结不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620002, "成员已在集结中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620003, "集结操作无效"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620004, "集结已存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620005, "集结人数已满"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620006, "集结已过期"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620007, "队伍状态非法"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620008, "队伍已在其他集结中"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620009, "战力不足无法加入集结"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(620010, "无权限执行集结操作"));
        }
    }
}
