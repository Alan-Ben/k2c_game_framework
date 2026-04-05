package CrossTeamServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.ChatRoomErr;
import CrossTeamServer.CrossTeamServer;
import CrossTeamServer.GeneralListener.GeneralBasicServerListener;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 处理逻辑： 加入聊天房间
 */
public class ChatRoomErr_Handler extends RpcRequestHandler<ChatRoomErr> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, ChatRoomErr _rpc)
	{
        GeneralBasicServerListener listener = (GeneralBasicServerListener) _committer.getRequestDealer();
        CrossTeamServer server = (CrossTeamServer) listener.getBasicServer();

        //销毁聊天房间数据
        server.getChatRoomMgr().onRomErr(_rpc.req().getRoomId());

        _rpc.commit();
	}
}
