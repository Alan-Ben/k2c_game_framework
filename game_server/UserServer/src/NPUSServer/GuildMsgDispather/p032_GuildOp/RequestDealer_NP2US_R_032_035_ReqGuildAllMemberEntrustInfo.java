package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_035_ReqGuildAllMemberEntrustInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 获取全成员贡献信息
 */
public class RequestDealer_NP2US_R_032_035_ReqGuildAllMemberEntrustInfo extends _ATRequestDealer_GuildOp<GC2GS_032_035_ReqGuildAllMemberEntrustInfo>
{
    public RequestDealer_NP2US_R_032_035_ReqGuildAllMemberEntrustInfo(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_035_ReqGuildAllMemberEntrustInfo _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_035_RetGuildAllMemberEntrustInfo(guildInfo.getMemberMgr().makeEntrustList()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}