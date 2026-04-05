package USServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.QuitChatRoom;
import ChatSystem._AChatRoomInfo;
import NPCommon.ErrMain.ChatErr;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;
/*******
 * 处理逻辑： 退出聊天房间
 */
public class QuitChatRoom_Handler extends _ATBasicUSRpc_Handler<QuitChatRoom> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, QuitChatRoom _rpc)
	{
        _AChatRoomInfo room = _usServer.getChatRoomMgr().lookupRoomById(_rpc.req().getRoomId());
        if(null == room)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        room.QuitRoom(_rpc.req().getCid());

		_rpc.commit();
	}
}
