package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.MarsObj.MarsBattleV2_MemberInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_061_ReqJoinRally;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;

/**
 * 加入集结请求（Guild侧处理入口）
 */
public class RequestDealer_NP2US_R_042_061_ReqJoinRally extends _ATRequestDealer_GuildOp<GC2GS_042_061_ReqJoinRally>
{
    public RequestDealer_NP2US_R_042_061_ReqJoinRally(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_061_ReqJoinRally _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if (guildInfo == null)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //获取队伍截面数据
        MarsBattleV2_MemberInfo teamInfo = new MarsBattleV2_MemberInfo();
        teamInfo.readPackage(_committer.getAddInfo());

        Result result = guildInfo.getRallyMgr().requestJoinRally(_committer.getCid(), _msg.getRallyId(), _msg.getTeamId(), teamInfo);
        if (!result.isSucc()) {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_061_RetJoinRally(_msg.getRallyId()));
    }

    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}


