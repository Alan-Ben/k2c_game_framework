package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_042_ReqSetRecommendRewardPoint;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 设置推荐攻击据点
 */
public class RequestDealer_NP2US_R_032_042_ReqSetRecommendRewardPoint extends _ATRequestDealer_GuildOp<GC2GS_032_042_ReqSetRecommendRewardPoint>
{
    public RequestDealer_NP2US_R_032_042_ReqSetRecommendRewardPoint(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_042_ReqSetRecommendRewardPoint _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        // 设置推荐奖励据点
        Result result = guildInfo.getCooperateInfo().setRecommendPoint(_msg.getPos());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_042_RetSetRecommendRewardPoint());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.SET_GUILD_COOPERATE_RECOMMEND_REWARD_POINT;
    }

}