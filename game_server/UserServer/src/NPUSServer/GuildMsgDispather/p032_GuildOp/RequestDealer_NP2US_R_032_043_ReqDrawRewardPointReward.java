package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_043_ReqDrawRewardPointReward;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetLongInfo;
import NPCommon.ErrMain.GuildCooperateErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.Cooperate.GuildCooperatePointInfo;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 领取奖励据点个人奖励消息处理类
 *
 * 功能：
 * 1. 领取指定奖励据点的个人奖励
 * 2. 验证玩家是否有领取资格
 * 3. 发放相应的奖励道具
 */
public class RequestDealer_NP2US_R_032_043_ReqDrawRewardPointReward extends _ATRequestDealer_GuildOp<GC2GS_032_043_ReqDrawRewardPointReward>
{
    public RequestDealer_NP2US_R_032_043_ReqDrawRewardPointReward(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_043_ReqDrawRewardPointReward _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_COOPERATE_DRAW_REWARD_POINT_REWARD);

        // 检查是否已经解锁
        GuildCooperatePointInfo pointInfo = guildInfo.getCooperateInfo().lookupPoint(_msg.getPos().getAreaId(), _msg.getPos().getIndex());
        if (pointInfo == null)
        {
            _committer.commitFailRes(GuildCooperateErr.REWARD_POINT_NOT_FOUND.getCode());
            return;
        }

        if (!pointInfo.isDefeat())
        {
            _committer.commitFailRes(GuildCooperateErr.REWARD_POINT_NOT_DEFEATED.getCode());
            return;
        }

        // 尝试领取公会财富
        pointInfo.drawGuildWealthReward(_committer.getCid(), context);

        //使用Tip方式下发奖励
        getUSServer().sendMsgToGC(_committer.getCid(), context.getCollector().toProto(ENpRewardShowType.TIP));

        GuildOp_RetLongInfo retInfo = new GuildOp_RetLongInfo(pointInfo.getAreaId());
        //返回请求
        _committer.commitSucRes(retInfo);
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}