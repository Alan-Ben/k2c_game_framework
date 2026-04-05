package CrossTeamServer.RPCDispatcher.Chat;

import AllRpcData.All_Service.Chat.SendChatRoomMsg;
import ChatSystem._AChatRoomInfo;
import CrossTeamServer.CrossTeamServer;
import CrossTeamServer.GeneralListener.GeneralBasicServerListener;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPChatMsgType;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*******
 * 处理逻辑： 退出聊天房间
 */
public class SendChatRoomMsg_Handler extends RpcRequestHandler<SendChatRoomMsg> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, SendChatRoomMsg _rpc)
	{
        GeneralBasicServerListener listener = (GeneralBasicServerListener) _committer.getRequestDealer();
        CrossTeamServer server = (CrossTeamServer) listener.getBasicServer();

        ENPChatMsgType msgType = ENPChatMsgType.ENPChatMsgType_FromInt(_rpc.req().getMsgType());
        if(null == msgType)
        {
            _rpc.commitFail(CommErr.PARAM_ERROR.getCode());
            return;
        }

        _AChatRoomInfo room = server.getChatRoomMgr().lookupRoomById(_rpc.req().getRoomId());
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
