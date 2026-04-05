package USServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.SendChatRoomMsg;
import ChatSystem._AChatRoomInfo;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPChatMsgType;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑： 退出聊天房间
 */
public class SendChatRoomMsg_Handler extends _ATBasicUSRpc_Handler<SendChatRoomMsg> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, SendChatRoomMsg _rpc)
	{
        ENPChatMsgType msgType = ENPChatMsgType.ENPChatMsgType_FromInt(_rpc.req().getMsgType());
        if(null == msgType)
        {
            _rpc.commitFail(CommErr.PARAM_ERROR.getCode());
            return;
        }

        _AChatRoomInfo room = _usServer.getChatRoomMgr().lookupRoomById(_rpc.req().getRoomId());
        if(null == room)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        room.SendRoomMsg(_rpc.req().getCid(), msgType, _rpc.req().get_buffer_GameUser(), _rpc.req().get_buffer_GameContent(), _result ->
        {
            if(!_result.isSucc())
            {
                _rpc.commitFail(_result.getCode());
                return;
            }

            _rpc.commit();
        });
	}
}
