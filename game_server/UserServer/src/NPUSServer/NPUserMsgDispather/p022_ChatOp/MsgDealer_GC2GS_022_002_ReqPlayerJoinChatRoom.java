package NPUSServer.NPUserMsgDispather.p022_ChatOp;

import GC2GS.p022_ChatOp.GC2GS_022_002_ReqPlayerJoinChatRoom;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_022_ChatOp;

/*************
 * 加入聊天房间
 */
public class MsgDealer_GC2GS_022_002_ReqPlayerJoinChatRoom extends NPUserMsgDealer<GC2GS_022_002_ReqPlayerJoinChatRoom>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_022_002_ReqPlayerJoinChatRoom _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getPlayerChatRoomDealer().joinRoom(_msg.getRoomType(), _msg.getExtId(), (_result, _roomId)->
        {
            if(!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            //通知聊天服务器，玩家加入了房间
            userData.sendMsgToGC(US2GCWriter_022_ChatOp.make_050_OnChatRoomJoin(_msg.getRoomType(), _roomId, _msg.getExtId()));

            //返回结果给客户端
            _commiter.commitSucRes(US2GCWriter_022_ChatOp.make_002_RetPlayerJoinChatRoom());
        });
    }
}
