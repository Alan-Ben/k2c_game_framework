using GOE;
namespace Common
{
/****
 游历错误
 ****/
    class TravelErr
    {
        static TravelErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(170001, "游历事件创建失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(170002, "游历事件未找到"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(170003, "游历事件未处理"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(170004, "游历事件不属于指定妃子列表"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(170005, "游历事件消耗不足"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(170006, "游历事件消耗失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(170007, "博彩押注额度不合法"));
        }
    }
}
