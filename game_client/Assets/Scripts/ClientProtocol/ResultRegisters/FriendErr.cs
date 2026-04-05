using GOE;
namespace Common
{
/****
 好友错误
 ****/
    class FriendErr
    {
        static FriendErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120001, "好友系统错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120002, "已经是好友"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120003, "好友数量上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120004, "对方好友数量上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120005, "好友申请不存在错误"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120006, "找不到指定好友"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120008, "对方好友申请数量上限"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120009, "找不到好友分组"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(120010, "好友分组数量达到上限"));
        }
    }
}
