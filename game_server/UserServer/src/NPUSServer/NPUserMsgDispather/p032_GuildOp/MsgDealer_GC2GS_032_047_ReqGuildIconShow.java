package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_047_ReqGuildIconShow;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_032_047_ReqGuildIconShow extends NPUserMsgDealer<GC2GS_032_047_ReqGuildIconShow>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_047_ReqGuildIconShow _msg)
    {
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _msg.getGuildId(),
                _msg,
                null
        );
    }
}
