package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_023_ReqGuildJoinRequestAKeyDeal;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 一键处理联盟加入请求
 */
public class RequestDealer_NP2US_R_032_023_ReqGuildJoinRequestAKeyDeal extends _ATRequestDealer_GuildOp<GC2GS_032_023_ReqGuildJoinRequestAKeyDeal>
{
    public RequestDealer_NP2US_R_032_023_ReqGuildJoinRequestAKeyDeal(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_023_ReqGuildJoinRequestAKeyDeal _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        //处理加入请求
        Result result = guildInfo.getGuildMgr().getJoinRequestMgr().aKeyDealJoinRequest(guildInfo, _msg.getIsAgree());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_023_RetGuildJoinRequestAKeyDeal());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.PROCESS_JOIN_REQUEST;
    }
}
