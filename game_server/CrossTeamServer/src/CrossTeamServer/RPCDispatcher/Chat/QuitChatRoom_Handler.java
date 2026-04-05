package CrossTeamServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.QuitChatRoom;
import ChatSystem._AChatRoomInfo;
import CrossTeamServer.CrossTeamServer;
import CrossTeamServer.GeneralListener.GeneralBasicServerListener;
import NPCommon.ErrMain.ChatErr;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 处理逻辑： 退出聊天房间
 */
public class QuitChatRoom_Handler extends RpcRequestHandler<QuitChatRoom> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, QuitChatRoom _rpc)
	{
        GeneralBasicServerListener listener = (GeneralBasicServerListener) _committer.getRequestDealer();
        CrossTeamServer server = (CrossTeamServer) listener.getBasicServer();

        _AChatRoomInfo room = server.getChatRoomMgr().lookupRoomById(_rpc.req().getRoomId());
        if(null == room)
        {
            _rpc.commitFail(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        room.QuitRoom(_rpc.req().getCid());

		_rpc.commit();
	}
}
