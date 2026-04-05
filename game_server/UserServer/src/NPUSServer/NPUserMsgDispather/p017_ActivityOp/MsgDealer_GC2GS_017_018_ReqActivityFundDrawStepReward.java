package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_018_ReqActivityFundDrawStepReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;

/**
 * 活动基金一键领取奖励请求处理器
 *
 * 功能：处理客户端请求一键领取所有可领取的阶段奖励
 */
public class MsgDealer_GC2GS_017_018_ReqActivityFundDrawStepReward extends NPUserMsgDealer<GC2GS_017_018_ReqActivityFundDrawStepReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_018_ReqActivityFundDrawStepReward _msg)
    {
        // 获取用户对象
        NPUSUserData userData = _committer.getUserData();

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_FUND_DRAW_REWARD);

        // 执行一键领取
        Result result = userData.getActivityFundComponent().drawAllAvailableRewards(_msg.getFundId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        // 推送奖励弹窗
        userData.sendMsgToGC(context.getCollector().toProto());

        // 返回成功
        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_018_RetActivityFundDrawStepReward());
    }
}
