package NPUSServer.NPUserMsgDispather.p022_ChatOp;

import GC2GS.p022_ChatOp.GC2GS_022_005_ReqPlayerSendPrivateMsg;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.PlayerInfo_IconShow;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_022_ChatOp;

/*************
 * 加入聊天房间
 */
public class MsgDealer_GC2GS_022_005_ReqPlayerSendPrivateMsg extends NPUserMsgDealer<GC2GS_022_005_ReqPlayerSendPrivateMsg>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_022_005_ReqPlayerSendPrivateMsg _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //检查聊天用户是否注册
        final ChatUserInfo user = getUSServer().getChatUserMgr().lookupChatUser(userData.getCid());
        if(null == user)
        {
            _commiter.commitFailRes(ChatErr.CHAT_USER_NOT_FOUND.getCode());
            return;
        }
        
        getUSServer().getPlayerCacheGetter().getInfo(PlayerInfo_IconShow.class, _msg.getReceiverCid(), 
        		new HandlerTwo<Boolean, PlayerInfo_IconShow>()
		        {
		            @Override
		            public void handle(Boolean _isSuc, PlayerInfo_IconShow _iconShowInfo)
		            {
		                if (!_isSuc)
		                {
		                	_commiter.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
		                	return;
		                }

                        userData.getPlayerChatRoomDealer().sendPrivateMsg(_msg.getReceiverCid(), _msg.getMsgType(), _msg.get_buffer_GameUser(), _msg.get_buffer_GameContent(),
                                (_result, msgId) ->
                                {
                                    if(!_result.isSucc())
                                    {
                                        _commiter.commitFailRes(_result.getCode());
                                        return;
                                    }

                                    _commiter.commitSucRes(US2GCWriter_022_ChatOp.make_005_RetPlayerSendPrivateMsg(msgId));
                                });
                    }
                });
    }
}
