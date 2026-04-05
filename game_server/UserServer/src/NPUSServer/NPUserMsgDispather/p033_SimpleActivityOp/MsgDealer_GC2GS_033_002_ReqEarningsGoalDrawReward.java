package NPUSServer.NPUserMsgDispather.p033_SimpleActivityOp;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_002_ReqEarningsGoalDrawReward;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EEarningsGoalType;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EarningsGoalActivity;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_033_SimpleActivityOp;

public class MsgDealer_GC2GS_033_002_ReqEarningsGoalDrawReward extends NPUserMsgDealer<GC2GS_033_002_ReqEarningsGoalDrawReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_033_002_ReqEarningsGoalDrawReward _msg)
    {
        EarningsGoalActivity activity = getUSServer().getCommActivityMgr().lookupActivity(_msg.getActivityInstanceId(), EarningsGoalActivity.class);
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        //检查活动状态
        if (!activity.isRunning())
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_EARNINGS_GOAL_REWARD);
        Result result = activity.drawReward(_committer.getUserData(), EEarningsGoalType.REWARD, _msg.getRefId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.getUserData().sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        _committer.commitSucRes(US2GCWriter_033_SimpleActivityOp.make_002_RetEarningsGoalDrawReward());
    }
}
