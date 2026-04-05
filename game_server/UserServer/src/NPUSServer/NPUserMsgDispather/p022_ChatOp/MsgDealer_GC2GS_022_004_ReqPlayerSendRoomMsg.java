package NPUSServer.NPUserMsgDispather.p022_ChatOp;

import ChatSystem._AChatRoomInfo;
import GC2GS.p022_ChatOp.GC2GS_022_004_ReqPlayerSendRoomMsg;
import NPCommon.ErrMain.ChatErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPChatMsgType;
import NPUSServer.ChatSys.ChatUserInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_022_ChatOp;

/*************
 * 加入聊天房间
 *
 * =========================================
 *      新节点废弃本协议使用（新协议：022-006）
 *      兼容旧系统：只在本服的聊天房间里进行查找
 * =========================================
 */
public class MsgDealer_GC2GS_022_004_ReqPlayerSendRoomMsg extends NPUserMsgDealer<GC2GS_022_004_ReqPlayerSendRoomMsg>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_022_004_ReqPlayerSendRoomMsg _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        ENPChatMsgType msgType = ENPChatMsgType.ENPChatMsgType_FromInt(_msg.getMsgType());
        if(null == msgType)
        {
            _commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
            return;
        }

        //检查聊天用户是否注册
        final ChatUserInfo user = getUSServer().getChatUserMgr().lookupChatUser(userData.getCid());
        if (null == user)
        {
            _commiter.commitFailRes(ChatErr.CHAT_USER_NOT_FOUND.getCode());
            return;
        }

        _AChatRoomInfo room = userData.getUSServer().getChatRoomMgr().lookupRoomById(_msg.getRoomId());
        if(room == null)
        {
            _commiter.commitFailRes(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
            return;
        }

        //检查聊天用户是否禁言
        if(userData.getForbidChatComponent().isForbidChat(room.getRoomType().ordinal()))
        {
            _commiter.commitFailRes(ChatErr.CHAT_FORBID.getCode());
            return;
        }

        if(null == room.lookupChatUser(userData.getCid()))
        {
            _commiter.commitFailRes(ChatErr.CHAT_USER_JOIN_ROOM_FAIL.getCode());
            return;
        }

        room.SendRoomMsg(userData.getCid(), msgType, _msg.get_buffer_GameUser(), _msg.get_buffer_GameContent(), _result ->
        {
            if(!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_022_ChatOp.make_004_RetPlayerSendRoomMsg());
        });
    }
}
