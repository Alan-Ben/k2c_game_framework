package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_025_ReqCancelLeaderImpeachEvent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_032_025_ReqCancelLeaderImpeachEvent extends NPUserMsgDealer<GC2GS_032_025_ReqCancelLeaderImpeachEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_025_ReqCancelLeaderImpeachEvent _msg)
    {
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
