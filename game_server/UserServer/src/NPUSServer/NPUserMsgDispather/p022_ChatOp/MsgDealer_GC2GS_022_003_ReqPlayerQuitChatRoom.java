package NPUSServer.NPUserMsgDispather.p022_ChatOp;

import GC2GS.p022_ChatOp.GC2GS_022_003_ReqPlayerQuitChatRoom;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_022_ChatOp;

/*************
 * 加入聊天房间
 */
public class MsgDealer_GC2GS_022_003_ReqPlayerQuitChatRoom extends NPUserMsgDealer<GC2GS_022_003_ReqPlayerQuitChatRoom>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_022_003_ReqPlayerQuitChatRoom _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getPlayerChatRoomDealer().quitRoom(_msg.getRoomType(), _msg.getExtId(), (_result)->
        {
            if(!_result.isSucc())
            {
                _commiter.commitFailRes(_result.getCode());
                return;
            }

            _commiter.commitSucRes(US2GCWriter_022_ChatOp.make_003_RetPlayerQuitChatRoom());
        });
    }
}
