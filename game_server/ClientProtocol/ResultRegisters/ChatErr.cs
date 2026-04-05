using GOE;
namespace Common
{
/****
 聊天错误
 ****/
    class ChatErr
    {
        static ChatErr()
        {
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(210001, "聊天房间不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(210002, "聊天房间未初始化完成"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(210003, "聊天用户不存在"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(210006, "聊天用户加入房间失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(210007, "聊天用户退出房间失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(210008, "发送消息失败"));
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(210009, "禁言中"));
        }
    }
}
