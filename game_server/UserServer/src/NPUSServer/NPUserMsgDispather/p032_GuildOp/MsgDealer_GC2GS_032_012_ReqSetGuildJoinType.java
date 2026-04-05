package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_012_ReqSetGuildJoinType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_032_012_ReqSetGuildJoinType extends NPUserMsgDealer<GC2GS_032_012_ReqSetGuildJoinType>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_012_ReqSetGuildJoinType _msg)
    {
        GuildOp_PlayerCname addInfo = new GuildOp_PlayerCname();
        addInfo.setCname(_committer.getUserData().getPlayerComponent().getName());

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo
        );
    }
}
