package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_005_ReqTransferGuild;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 申请调整联盟盟主
 */
public class RequestDealer_NP2US_R_032_005_ReqTransferGuild extends _ATRequestDealer_GuildOp<GC2GS_032_005_ReqTransferGuild>
{
    public RequestDealer_NP2US_R_032_005_ReqTransferGuild(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_005_ReqTransferGuild _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_LEADER_TRANSFER_MANUAL);

        Result result = _committer.getGuildInfo().transLeader(_msg.getMemberId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_005_RetTransferGuild());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.TRANSFER_LEADER;
    }
}