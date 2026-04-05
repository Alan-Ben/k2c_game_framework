using GOE;
namespace Common
{
/****
 宴会错误
 ****/
    class DinnerErr
    {
        static DinnerErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110001, "宴会未领取开宴奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110002, "宴会已开启"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110003, "宴会不允许开启"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110004, "宴会凭证不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110005, "宴会不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110006, "宴会已结束"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110007, "宴会已经满座"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110008, "已经加入宴会"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110009, "是自己举办的宴会"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110010, "没有开宴奖励"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110011, "开宴奖励领取失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(110012, "赴宴次数限制"));
        }
    }
}
