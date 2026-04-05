package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_013_ReqProcessGuildJoinRequest;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 处理联盟加入请求
 */
public class RequestDealer_NP2US_R_032_013_ReqProcessGuildJoinRequest extends _ATRequestDealer_GuildOp<GC2GS_032_013_ReqProcessGuildJoinRequest>
{
    public RequestDealer_NP2US_R_032_013_ReqProcessGuildJoinRequest(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_013_ReqProcessGuildJoinRequest _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        //处理加入请求
        Result result = guildInfo.getGuildMgr().getJoinRequestMgr().dealJoinRequest(
                guildInfo, _msg.getRequestId(), _msg.getIsAccept());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_013_RetProcessGuildJoinRequest());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.PROCESS_JOIN_REQUEST;
    }
}