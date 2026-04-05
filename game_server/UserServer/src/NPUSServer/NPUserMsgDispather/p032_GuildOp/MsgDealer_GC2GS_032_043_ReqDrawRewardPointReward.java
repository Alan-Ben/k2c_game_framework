package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_043_ReqDrawRewardPointReward;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetLongInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Guild.RefGuildCooperateArea;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

/**
 * 领取奖励据点个人奖励处理器
 */
public class MsgDealer_GC2GS_032_043_ReqDrawRewardPointReward extends NPUserMsgDealer<GC2GS_032_043_ReqDrawRewardPointReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_043_ReqDrawRewardPointReward _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_COOPERATE_DRAW_REWARD_POINT_REWARD);

        //记录领取情况，这里直接记录避免短时间内连续发送导致联盟奖励重复获取
        Result result = _committer.getUserData().getGuildCooperateComponent().recordRewardDraw(_msg.getPos().getAreaId(), _msg.getPos().getIndex(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_RetLongInfo>(_committer) {
                    @Override
                    protected GuildOp_RetLongInfo _createNewTmpObj() {
                        return new GuildOp_RetLongInfo();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_RetLongInfo _msg)
                    {
                        //获取静态配置信息
                        long areaId = _msg.getNum();
                        RefGuildCooperateArea areaRef = RefGuildCooperateArea.getMgr().get(areaId);
                        if(null == areaRef)
                        {
                            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
                            return ;
                        }

                        // 发放奖励
                        _commiter.getUserData().gainItemList(areaRef.reward_point_reward_list,context);
                        _commiter.getUserData().gainItemList(areaRef.reward_point_random_reward_list,context);

                        _commiter.getUserData().sendMsgToGC(context.getCollector().toProto());

                        _commiter.commitSucRes(US2GCWriter_032_GuildOp.make_043_RetDrawRewardPointReward());
                    }
                },
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}