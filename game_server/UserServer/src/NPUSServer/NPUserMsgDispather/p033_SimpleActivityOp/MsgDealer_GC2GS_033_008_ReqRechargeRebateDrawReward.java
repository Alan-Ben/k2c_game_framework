package NPUSServer.NPUserMsgDispather.p033_SimpleActivityOp;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_008_ReqRechargeRebateDrawReward;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Activities.RechargeRebate.RechargeRebateActivity;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_033_SimpleActivityOp;

/**
 * 请求领取充值返利奖励处理器
 * <p>
 * 处理客户端领取返利奖励的请求
 */
public class MsgDealer_GC2GS_033_008_ReqRechargeRebateDrawReward extends NPUserMsgDealer<GC2GS_033_008_ReqRechargeRebateDrawReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_033_008_ReqRechargeRebateDrawReward _msg)
    {
        // 查找活动实例
        RechargeRebateActivity activity = getUSServer().getCommActivityMgr().lookupActivity(
                _msg.getActivityInstanceId(), RechargeRebateActivity.class);

        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 检查活动状态
        if (!activity.isRunning())
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_RECHARGE_REBATE_REWARD);

        // 领取奖励
        Result result = activity.drawReward(_committer.getUserData(), _msg.getGroupId(), _msg.getStepId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.getUserData().sendMsgToGC(context.getCollector().toProto());

        // 返回成功响应（空响应）
        _committer.commitSucRes(US2GCWriter_033_SimpleActivityOp.make_008_RetRechargeRebateDrawReward());
    }
}
