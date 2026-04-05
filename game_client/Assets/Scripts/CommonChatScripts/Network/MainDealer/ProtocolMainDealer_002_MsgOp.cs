
using ALBasicProtocolPack;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 消息相关回包处理
    /// </summary>
    internal class ProtocolMainDealer_002_MsgOp : ALBasicProtocolMainOrderDealer
    {
        internal ProtocolMainDealer_002_MsgOp()
            : base(2, 60)
        {
            // 请求聊天室历史消息的回包
            regDealer(new ProtocolSubDealer_002_001_RetRoomHistory());
            // 请求所有未读会话的回包
            regDealer(new ProtocolSubDealer_002_004_RetUnreadPrivateMsgUserList());
            // 请求私聊的未读消息的回包
            regDealer(new ProtocolSubDealer_002_005_RetUnreadPrivateMsgList());
            // 收到了聊天室新消息的推送
            regDealer(new ProtocolSubDealer_002_050_OnReceiveRoomMsg());
            // 收到了私聊新消息的推送
            regDealer(new ProtocolSubDealer_002_051_OnReceivePrivateMsg());

        }
    }
}
