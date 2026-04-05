package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_018_ReqGuildImpeachLeader;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.Event.GuildEventDealer_ImpeachLeader;
import NPUSServer.Guild.Event._AGuildEvent;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 弹劾投票
 */
public class RequestDealer_NP2US_R_032_018_ReqGuildImpeachLeader extends _ATRequestDealer_GuildOp<GC2GS_032_018_ReqGuildImpeachLeader>
{
    public RequestDealer_NP2US_R_032_018_ReqGuildImpeachLeader(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_018_ReqGuildImpeachLeader _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        //处理加入请求
        _AGuildEvent<?> event = guildInfo.getEventMgr().lookup(_msg.getEventDbId());
        if (!(event instanceof GuildEventDealer_ImpeachLeader))
        {
            _committer.commitFailRes(GuildErr.EVENT_NOT_FOUND.getCode());
            return;
        }

        //投票
        Result voteResult = ((GuildEventDealer_ImpeachLeader) event).vote(_committer.getCid());
        if (!voteResult.isSucc())
        {
            _committer.commitFailRes(voteResult.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_018_RetGuildImpeachLeader());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}