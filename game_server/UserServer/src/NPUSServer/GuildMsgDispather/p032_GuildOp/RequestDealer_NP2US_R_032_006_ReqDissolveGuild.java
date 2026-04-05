package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_006_ReqDissolveGuild;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 申请解散联盟
 */
public class RequestDealer_NP2US_R_032_006_ReqDissolveGuild extends _ATRequestDealer_GuildOp<GC2GS_032_006_ReqDissolveGuild>
{
    public RequestDealer_NP2US_R_032_006_ReqDissolveGuild(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_006_ReqDissolveGuild _msg)
    {
        Result result = _committer.getGuildInfo().dissolve(_committer.getCid());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_006_RetDissolveGuild());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.DISSOLVE_GUILD;
    }
}