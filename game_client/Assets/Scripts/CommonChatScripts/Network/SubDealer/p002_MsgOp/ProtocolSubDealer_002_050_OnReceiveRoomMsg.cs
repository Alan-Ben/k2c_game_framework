using GS2GC.p002_MsgOp;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 当接收到了聊天室消息时
    /// </summary>
    internal class ProtocolSubDealer_002_050_OnReceiveRoomMsg : _AProtocolBaseSubDealer<GS2GC_002_050_OnReceiveRoomMsg>
    {
        protected override void _dealProtocolByLog(ChatClient _dealer, GS2GC_002_050_OnReceiveRoomMsg _msg)
        {
            // 转到数据管理器中处理
            _dealer?.dataMgr.onReceiveRoomMsg(_msg);
        }
    }
}
