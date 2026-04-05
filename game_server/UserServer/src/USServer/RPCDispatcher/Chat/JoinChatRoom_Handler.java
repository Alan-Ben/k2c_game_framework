package USServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.JoinChatRoom;
import ChatSystem._AChatRoomInfo;
import NPCommon.ErrMain.ChatErr;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 加入聊天房间
 */
public class JoinChatRoom_Handler extends _ATBasicUSRpc_Handler<JoinChatRoom> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, JoinChatRoom _rpc)
	{
        _AChatRoomInfo room = _usServer.getChatRoomMgr().lookupRoomById(_rpc.req().getRoomId());
        if(null == room)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        room.JoinRoom(_rpc.req().getChatUser(), (_result, _roomId) ->
        {
            _rpc.retObj().setRoomId(_roomId);

            _rpc.commit();
        });
	}
}
