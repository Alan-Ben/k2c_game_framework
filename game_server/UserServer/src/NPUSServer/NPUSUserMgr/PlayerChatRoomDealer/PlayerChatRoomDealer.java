package NPUSServer.NPUSUserMgr.PlayerChatRoomDealer;

import NPCommon.ErrMain.ChatErr;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CallBack._IGetChatRoomCallBackResult;
import NPUSServer.ChatSys.ChatRoomApi;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.nio.ByteBuffer;

/**
 * 玩家发起的聊天数据处理
 */
public class PlayerChatRoomDealer
{
    //玩家数据
    private NPUSUserData _m_udUserData;

    public PlayerChatRoomDealer(NPUSUserData _userData)
    {
        _m_udUserData = _userData;
    }

    public NPUSUserData getUserData() {return _m_udUserData;}

    /**
     * 根据用户数据获取对应的聊天房间ID
     * @param _roomType
     * @param _extId
     * @param _callback
     */
    public void getRoom(int _roomType, long _extId, _IGetChatRoomCallBackResult _callback)
    {
        _IPlayerChatRoomDealer dealer = PlayerChatRoomDealerMgr.getInstance().getDealer(_roomType);
        if(null == dealer)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND, 0L, 0, 0);
            return;
        }

        dealer.getRoom(_m_udUserData, _extId, _callback);
    }

    /**
     * 玩家加入指定聊天房间
     * @param _roomType
     * @param _extId
     * @param _callback
     */
    public void joinRoom(int _roomType, long _extId, _ICallBackResultT<Long> _callback)
    {
        _IPlayerChatRoomDealer dealer = PlayerChatRoomDealerMgr.getInstance().getDealer(_roomType);
        if(null == dealer)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND, 0L);
            return;
        }

        dealer.joinRoom(_m_udUserData, _extId, _callback);
    }

    /**
     * 玩家退出指定聊天房间
     * @param _roomType
     * @param _extId
     * @param _callback
     */
    public void quitRoom(int _roomType, long _extId, _ICallBackResult _callback)
    {
        _IPlayerChatRoomDealer dealer = PlayerChatRoomDealerMgr.getInstance().getDealer(_roomType);
        if(null == dealer)
        {
            _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND);
            return;
        }

        dealer.quitRoom(_m_udUserData, _extId, _callback);
    }

    /**
     * 玩家发送聊天消息接口
     * @param _roomType
     * @param _extId
     * @param _msgType
     * @param _userContent
     * @param _msgContent
     * @param _callback
     */
    public void sendRoomMsg(int _roomType, long _extId, int _msgType, ByteBuffer _userContent, ByteBuffer _msgContent, _ICallBackResult _callback)
    {
        _IPlayerChatRoomDealer dealer = PlayerChatRoomDealerMgr.getInstance().getDealer(_roomType);
        if(null == dealer)
        {
            if(null != _callback)
                _callback.onRunOver(ChatErr.CHAT_ROOM_NOT_FOUND);
            return;
        }

        dealer.getRoom(_m_udUserData, _extId, (_result, _roomId, _chatServerType, _chatServerTypeId) ->
        {
            if(!_result.isSucc())
            {
                if(null != _callback)
                    _callback.onRunOver(_result);
                return;
            }

            ChatRoomApi.sendRoomMsg(_m_udUserData.getUSServer(), _m_udUserData.getCid(), _roomId, _chatServerType, _chatServerTypeId,
                    _msgType, _userContent, _msgContent, _callback);
        });
    }

    /**
     * 玩家发送私聊消息接口
     * @param _receiverCid
     * @param _msgType
     * @param _gameUser
     * @param _gameContent
     * @param _callback
     */
    public void sendPrivateMsg(long _receiverCid, int _msgType
            , ByteBuffer _gameUser, ByteBuffer _gameContent, _ICallBackResultT<Long> _callback)
    {
        //检查玩家是否注册聊天用户
        ChatUserInfo chatUser = _m_udUserData.getUSServer().getChatUserMgr().lookupChatUser(_m_udUserData.getCid());
        if(null == chatUser)
        {
            _callback.onRunOver(ChatErr.CHAT_USER_NOT_FOUND, 0L);
            return;
        }

        ChatRoomApi.sendPrivateMsg(_m_udUserData.getUSServer(), chatUser.getChatUid(), _receiverCid, _msgType, _gameUser, _gameContent, _callback);
    }
}
