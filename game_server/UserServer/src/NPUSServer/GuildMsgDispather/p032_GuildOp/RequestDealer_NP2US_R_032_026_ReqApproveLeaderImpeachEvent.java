package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_026_ReqApproveLeaderImpeachEvent;
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
 * 强制取消弹劾事件
 */
public class RequestDealer_NP2US_R_032_026_ReqApproveLeaderImpeachEvent extends _ATRequestDealer_GuildOp<GC2GS_032_026_ReqApproveLeaderImpeachEvent>
{
    public RequestDealer_NP2US_R_032_026_ReqApproveLeaderImpeachEvent(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_026_ReqApproveLeaderImpeachEvent _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        //处理加入请求
        _AGuildEvent<?> event = guildInfo.getEventMgr().lookup(_msg.getEventDbId());
        if (!(event instanceof GuildEventDealer_ImpeachLeader))
        {
            _committer.commitFailRes(GuildErr.EVENT_NOT_FOUND.getCode());
            return;
        }

        //判断是否是本人
        if (((GuildEventDealer_ImpeachLeader) event).getData().getLeaderCid() != _committer.getCid())
        {
            _committer.commitFailRes(GuildErr.DONT_HAVE_PERMISSION.getCode());
            return;
        }

        Result result = guildInfo.getEventMgr().forceDoneEvent(_msg.getEventDbId());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_026_RetApproveLeaderImpeachEvent());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}
