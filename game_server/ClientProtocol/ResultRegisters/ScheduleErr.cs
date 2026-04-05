using GOE;
namespace Common
{
/****
 排期相关报错
 ****/
    class ScheduleErr
    {
        static ScheduleErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(280001, "没有预备跨服分组信息"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(280002, "跨服分组ID在往期已被使用"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(280003, "玩家服务器已经在其他跨服分组"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(280004, "跨服分组ID已存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(280005, "跨服分组不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(280006, "排期已激活不能更新"));
        }
    }
}
