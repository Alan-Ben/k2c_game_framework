using GS2GC.p002_MsgOp;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 请求私聊未读消息的回包
    /// </summary>
    internal class ProtocolSubDealer_002_004_RetUnreadPrivateMsgUserList : _AProtocolBaseSubDealer<GS2GC_002_004_RetUnreadPrivateMsgUserList>
    {
        protected override void _dealProtocolByLog(ChatClient _dealer, GS2GC_002_004_RetUnreadPrivateMsgUserList _msg)
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
            _dealer?.dataMgr.retPrivateUnreadMsgBrief(_msg);
        }
    }
}
