package USServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.ChatRoomErr;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑： 加入聊天房间
 */
public class ChatRoomErr_Handler extends _ATBasicUSRpc_Handler<ChatRoomErr> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, ChatRoomErr _rpc)
	{
        //销毁聊天房间数据
        _usServer.getChatRoomMgr().onRomErr(_rpc.req().getRoomId());

        _rpc.commit();
	}
}
