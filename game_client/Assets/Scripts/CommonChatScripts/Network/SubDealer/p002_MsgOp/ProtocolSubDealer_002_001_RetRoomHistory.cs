

using GS2GC.p002_MsgOp;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 请求聊天室历史消息的回包
    /// </summary>
    internal class ProtocolSubDealer_002_001_RetRoomHistory : _AProtocolBaseSubDealer<GS2GC_002_001_RetRoomHistory>
    {
        protected override void _dealProtocolByLog(ChatClient _dealer, GS2GC_002_001_RetRoomHistory _msg)
        {
            if (_msg == null)
                return;

            // 处理异常码
            if (_msg.getErrCode() != 0)
            {
                Chat.onNetError?.Invoke(_msg.getErrCode());
                return;
            }

            // 转到数据管理器中处理
            _dealer?.dataMgr.retRoomChatHistory(_msg);
        }
    }
}
