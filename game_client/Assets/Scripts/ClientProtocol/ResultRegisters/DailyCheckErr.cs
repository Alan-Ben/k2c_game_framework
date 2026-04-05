using GOE;
namespace Common
{
/****
 每日签到错误
 ****/
    class DailyCheckErr
    {
        static DailyCheckErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(160001, "每日签到今天已签到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(160002, "每日签到选择甜品存在"));
        }
    }
}
