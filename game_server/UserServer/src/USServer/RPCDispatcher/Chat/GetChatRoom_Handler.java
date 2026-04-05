package USServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.GetChatRoom;
import ChatSystem._AChatRoomInfo;
import NPCommon.ErrMain.ChatErr;
import NPEnum.ENPChatRoomType;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑： 加入聊天房间
 */
public class GetChatRoom_Handler extends _ATBasicUSRpc_Handler<GetChatRoom> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GetChatRoom _rpc)
	{
        ENPChatRoomType roomType = ENPChatRoomType.ENPChatRoomType_FromInt(_rpc.req().getRoomType());
        if(null == roomType)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        _AChatRoomInfo room = _usServer.getChatRoomMgr().lookupRoom(roomType, _rpc.req().getRoomTypeId());
        if(null == room)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        if(room.getRoomSdkId() <= 0)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_INITED.getCode());
            return;
        }

        _rpc.retObj().setRoomId(room.getRoomSdkId());
        _rpc.commit();
	}
}
