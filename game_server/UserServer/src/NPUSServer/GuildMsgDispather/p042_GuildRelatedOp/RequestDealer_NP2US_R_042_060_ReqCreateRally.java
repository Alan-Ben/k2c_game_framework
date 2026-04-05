package NPUSServer.GuildMsgDispather.p042_GuildRelatedOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.MarsObj.MarsBattleV2_MemberInfo;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_060_ReqCreateRally;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_042_GuildRelatedOp;
import NPUSServer.NPUserServer;

/**
 * 创建集结请求（Guild侧处理入口）
 */
public class RequestDealer_NP2US_R_042_060_ReqCreateRally extends _ATRequestDealer_GuildOp<GC2GS_042_060_ReqCreateRally>
{
    public RequestDealer_NP2US_R_042_060_ReqCreateRally(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_042_060_ReqCreateRally _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if (guildInfo == null)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        //获取队伍截面数据
        MarsBattleV2_MemberInfo createTeamInfo = new MarsBattleV2_MemberInfo();
        createTeamInfo.readPackage(_committer.getAddInfo());

        //尝试创建集结
        ResultOne<Long> result = guildInfo.getRallyMgr().requestCreateRally(_committer.getCid(), _msg.getTeamId(), createTeamInfo);
        if (!result.isSucc()) {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_042_GuildRelatedOp.make_060_RetCreateRally(result.getData()));
    }

    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}


