package CrossTeamServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.GetChatRoom;
import ChatSystem._AChatRoomInfo;
import CrossTeamServer.CrossTeamServer;
import CrossTeamServer.GeneralListener.GeneralBasicServerListener;
import NPCommon.ErrMain.ChatErr;
import NPEnum.ENPChatRoomType;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 处理逻辑： 加入聊天房间
 */
public class GetChatRoom_Handler extends RpcRequestHandler<GetChatRoom> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, GetChatRoom _rpc)
	{
        GeneralBasicServerListener listener = (GeneralBasicServerListener) _committer.getRequestDealer();
        CrossTeamServer server = (CrossTeamServer) listener.getBasicServer();

        ENPChatRoomType roomType = ENPChatRoomType.ENPChatRoomType_FromInt(_rpc.req().getRoomType());
        if(null == roomType)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        _AChatRoomInfo room = server.getChatRoomMgr().lookupRoom(roomType, _rpc.req().getRoomTypeId());
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
