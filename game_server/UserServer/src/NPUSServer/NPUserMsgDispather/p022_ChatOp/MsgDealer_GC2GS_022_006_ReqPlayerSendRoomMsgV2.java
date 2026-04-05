package NPUSServer.NPUserMsgDispather.p022_ChatOp;

import GC2GS.p022_ChatOp.GC2GS_022_006_ReqPlayerSendRoomMsgV2;
import NPCommon.ErrMain.ChatErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_022_ChatOp;

/*************
 * 加入聊天房间
 */
public class MsgDealer_GC2GS_022_006_ReqPlayerSendRoomMsgV2 extends NPUserMsgDealer<GC2GS_022_006_ReqPlayerSendRoomMsgV2>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_022_006_ReqPlayerSendRoomMsgV2 _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //检查聊天用户是否禁言
        if(userData.getForbidChatComponent().isForbidChat(_msg.getRoomType()))
        {
            _commiter.commitFailRes(ChatErr.CHAT_FORBID.getCode());
            return;
        }

        //加入聊天房间
        userData.getPlayerChatRoomDealer().sendRoomMsg(_msg.getRoomType(), _msg.getExtId(),
                _msg.getMsgType(), _msg.get_buffer_GameUser(), _msg.get_buffer_GameContent(), _result ->
        {
            if(!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_022_ChatOp.make_006_RetPlayerSendRoomMsgV2());
        });
    }
}
