package NPUSServer.ChatSys;

import ALBasicProtocolPack._IALProtocolStructure;
import AllRpcData.All_Service.Chat.SendChatRoomMsg;
import NP2IS_RB.p001_ISOp.NP2IS_RB_001_007_RetSendPrivateMsg;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPServerProtocolWriter.NP2IS.Request.Np2IS_R_Writer_001_ISOp;
import NPUSServer.NPUserServer;
import RPC.RpcSender;
import RPC._ARpcCallBack;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

import java.nio.ByteBuffer;

public class ChatRoomApi
{
    /**
     * 发送聊天房间系统消息接口（US房间），仅限本服US服务器内使用
     * @param _server
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public static void sendUsRoomSysMsg(NPUserServer _server,
                                        ENPChatMsgType _msgType, ByteBuffer _userContent, ByteBuffer _msgContent, _ICallBackResult _callback)
    {
        senRoomSysMsgByType(_server, ENPChatRoomType.US_SERVER.ordinal(), _server.getServerTypeId(), 0L, _msgType.ordinal(), _userContent, _msgContent, _callback);
    }

    /**
     * 发送聊天房间系统消息接口（公会房间），支持跨服
     * @param _server
     * @param _guildId
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public static void sendGuildRoomSysMsg(NPUserServer _server, long _guildId,
                                           ENPChatMsgType _msgType, ByteBuffer _userContent, ByteBuffer _msgContent, _ICallBackResult _callback)
    {
        if(_guildId <= 0)
        {
            if(null != _callback)
                _callback.onRunOver(GuildErr.GUILD_NOT_EXIST);
            return;
        }

        senRoomSysMsgByType(_server, ENPChatRoomType.GUILD.ordinal(), _guildId, 0L, _msgType.ordinal(), _userContent, _msgContent, _callback);
    }

    /**
     * 发送聊天房间系统消息接口（活动组队房间），支持跨服
     * @param _server
     * @param _instanceId 活动实例ID
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public static void sendActivityTeamRoomSysMsg(NPUserServer _server, long _instanceId,
                                                  ENPChatMsgType _msgType, ByteBuffer _userContent, ByteBuffer _msgContent, _ICallBackResult _callback)
    {
        if(_instanceId <= 0)
        {
            if(null != _callback)
                _callback.onRunOver(CommErr.PARAM_ERROR);
            return;
        }

        senRoomSysMsgByType(_server, ENPChatRoomType.ACTIVITY_TEAM.ordinal(), 0, _instanceId, _msgType.ordinal(), _userContent, _msgContent, _callback);
    }

    /**
     * 发送聊天房间系统消息接口（房间类型 + 房间类型ID + 额外参数ID），支持跨服
     * @param _server
     * @param _roomType
     * @param _roomTypeId
     * @param _extId
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public static void senRoomSysMsgByType(NPUserServer _server, int _roomType, long _roomTypeId, long _extId,
                                           int _msgType, ByteBuffer _userContent, ByteBuffer _msgContent, _ICallBackResult _callback)
    {
        _IChatRoomDealer dealer = ChatRoomDealerMgr.getInstance().getDealer(_roomType);
        if(null == dealer)
        {
            if(null != _callback)
                _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND);
            return;
        }

        dealer.getRoom(_server, _roomTypeId, _extId, (_result, _roomId, _chatServerType, _chatServerTypeId) ->
        {
            if(!_result.isSucc())
            {
                if(null != _callback)
                    _callback.onRunOver(_result);
                return;
            }

            sendRoomSysMsg(_server, _roomId, _chatServerType, _chatServerTypeId,
                    _msgType, _userContent, _msgContent, _callback);
        });
    }

    /**
     * 发送聊天房间系统消息接口
     * @param _server
     * @param _roomId
     * @param _chatServerType
     * @param _chatServerTypeId
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public static void sendRoomSysMsg(NPUserServer _server, long _roomId, int _chatServerType, int _chatServerTypeId,
                                      int _msgType, ByteBuffer _userContent, ByteBuffer _msgContent,
                                      _ICallBackResult _callback)
    {
        sendRoomMsg(_server, 0L, _roomId, _chatServerType, _chatServerTypeId, _msgType, _userContent, _msgContent, _callback);
    }

    /**
     * 发送聊天房间消息接口
     * @param _server
     * @param _cid
     * @param _roomId
     * @param _chatServerType
     * @param _chatServerTypeId
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public static void sendRoomMsg(NPUserServer _server, long _cid, long _roomId, int _chatServerType, int _chatServerTypeId,
                                   int _msgType, ByteBuffer _userContent, ByteBuffer _msgContent,
                                   _ICallBackResult _callback)
    {
        SendChatRoomMsg rpc = new SendChatRoomMsg();
        rpc.req().setRoomId(_roomId);
        rpc.req().setCid(_cid);
        rpc.req().setMsgType(_msgType);
        rpc.req().setGameUser(_userContent);
        rpc.req().setGameContent(_msgContent);

        if(NPEnum.EServerType.SINGLE.ordinal() == _chatServerType) //单服务器类型
        {
            RpcSender sender = null;
            if(NPEnum.ENPSingleServerType.CROSS_TEAM.ordinal() == _chatServerTypeId) //CrossTeam服务器上的聊天房间
            {
                sender = _server.rpc2crossTeam();
            }

            if(null == sender)
            {
                if(null != _callback)
                    _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND);
                return;
            }

            sender.request(rpc, new _ARpcCallBack<SendChatRoomMsg>()
            {
                @Override
                public void call_back(int _errCode, SendChatRoomMsg _rpc)
                {
                    if(_errCode > 0)
                    {
                        if(null != _callback)
                            _callback.onRunOver(Result.failed(_errCode));
                        return;
                    }

                    if(null != _callback)
                        _callback.onRunOver(Result.SUCC);
                }
            });
        }
        else //多服务器类型
        {
            RpcSender sender = null;
            if(NPEnum.EServerType.USER.ordinal() == _chatServerType) //US服务器上的聊天房间
            {
                sender = _server.rpc2us();
            }

            if(null == sender)
            {
                if(null != _callback)
                    _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND);
                return;
            }

            sender.requestTo(_chatServerTypeId, rpc, new _ARpcCallBack<SendChatRoomMsg>()
            {
                @Override
                public void call_back(int _errCode, SendChatRoomMsg _rpc)
                {
                    if(_errCode > 0)
                    {
                        if(null != _callback)
                            _callback.onRunOver(Result.failed(_errCode));
                        return;
                    }

                    if(null != _callback)
                        _callback.onRunOver(Result.SUCC);
                }
            });
        }
    }

    /**
     * 发送私聊消息接口
     * @param _server
     * @param _senderChatUid
     * @param _receiverCid
     * @param _msgType
     * @param _gameUser
     * @param _gameContent
     * @param _callback
     */
	public static void sendPrivateMsg(NPUserServer _server, String _senderChatUid, long _receiverCid,
                                      int _msgType, ByteBuffer _gameUser, ByteBuffer _gameContent,
                                      _ICallBackResultT<Long> _callback)
	{
		//发起私聊请求
        _server.sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.INTERFACE.ordinal(),
				Np2IS_R_Writer_001_ISOp.make_007_ReqSendPrivateMsg(_senderChatUid, _receiverCid, _msgType, _gameUser, _gameContent),
				new _IWCGCallbackDealer() 
				{
					@Override
					public void dealSuc(_IALProtocolStructure _ret) 
					{
						NP2IS_RB_001_007_RetSendPrivateMsg proto = (NP2IS_RB_001_007_RetSendPrivateMsg) _ret;
						
						if(null != _callback)
							_callback.onRunOver(Result.SUCC, proto.getMsgId());
					}
					
					@Override
					public void dealFail(int _err) 
					{
						if(null != _callback)
							_callback.onRunOver(Result.failed(_err), 0L);
					}
					
					@Override
					public _IALProtocolStructure createProtocolObj() 
					{
						return new NP2IS_RB_001_007_RetSendPrivateMsg();
					}
				});
	}
}
