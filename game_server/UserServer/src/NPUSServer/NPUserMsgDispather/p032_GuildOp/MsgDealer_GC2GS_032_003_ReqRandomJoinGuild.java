package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_003_ReqRandomJoinGuild;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

public class MsgDealer_GC2GS_032_003_ReqRandomJoinGuild extends NPUserMsgDealer<GC2GS_032_003_ReqRandomJoinGuild>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_003_ReqRandomJoinGuild _msg)
    {
        //检查玩家是否已经加入联盟
        if (_committer.getUserData().getGuildComponent().getGuildId() > 0)
        {
            _committer.commitFailRes(GuildErr.PLAYER_ALREADY_IN_GUILD.getCode());
            return;
        }

        //检查加入联盟CD
        if (!_committer.getUserData().getGuildComponent().checkJoinGuildCd())
        {
            _committer.commitFailRes(GuildErr.JOIN_GUILD_CD.getCode());
            return;
        }

        //加入联盟
        Result result = getUSServer().getGuildMgr().randomJoinGuild(_committer.getUserData());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_003_RetRandomJoinGuild());
    }
}
