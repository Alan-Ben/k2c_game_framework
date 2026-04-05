using GS2GC.p002_MsgOp;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 当接收到了私聊消息时
    /// </summary>
    internal class ProtocolSubDealer_002_051_OnReceivePrivateMsg : _AProtocolBaseSubDealer<GS2GC_002_051_OnReceivePrivateMsg>
    {
        protected override void _dealProtocolByLog(ChatClient _dealer, GS2GC_002_051_OnReceivePrivateMsg _msg)
        {
            // 转到数据管理器中处理
            _dealer?.dataMgr.onReceivePrivateMsg(_msg);
        }
    }
}
