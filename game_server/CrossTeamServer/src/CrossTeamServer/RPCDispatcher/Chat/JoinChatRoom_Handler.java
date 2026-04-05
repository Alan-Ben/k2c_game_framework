package CrossTeamServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.JoinChatRoom;
import ChatSystem._AChatRoomInfo;
import CrossTeamServer.CrossTeamServer;
import CrossTeamServer.GeneralListener.GeneralBasicServerListener;
import NPCommon.ErrMain.ChatErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 处理逻辑： 加入聊天房间
 */
public class JoinChatRoom_Handler extends RpcRequestHandler<JoinChatRoom> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, JoinChatRoom _rpc)
	{
        GeneralBasicServerListener listener = (GeneralBasicServerListener) _committer.getRequestDealer();
        CrossTeamServer server = (CrossTeamServer) listener.getBasicServer();

        _AChatRoomInfo room = server.getChatRoomMgr().lookupRoomById(_rpc.req().getRoomId());
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
