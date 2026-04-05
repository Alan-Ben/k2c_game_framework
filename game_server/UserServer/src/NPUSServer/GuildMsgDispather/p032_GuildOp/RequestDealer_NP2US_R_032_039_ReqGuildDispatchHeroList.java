package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_039_ReqGuildDispatchHeroList;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 申请派遣英雄列表信息
 */
public class RequestDealer_NP2US_R_032_039_ReqGuildDispatchHeroList extends _ATRequestDealer_GuildOp<GC2GS_032_039_ReqGuildDispatchHeroList>
{
    public RequestDealer_NP2US_R_032_039_ReqGuildDispatchHeroList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_039_ReqGuildDispatchHeroList _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_039_RetGuildDispatchHeroList(guildInfo.getHeroDispatchMgr().makeAllDetailInfo()));
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}