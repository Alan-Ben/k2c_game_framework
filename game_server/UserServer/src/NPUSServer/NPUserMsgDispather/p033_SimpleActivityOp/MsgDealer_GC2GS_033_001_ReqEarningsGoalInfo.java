package NPUSServer.NPUserMsgDispather.p033_SimpleActivityOp;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_001_ReqEarningsGoalInfo;
import NPCommon.ErrMain.ActivityErr;
import NPUSServer.CommonActivityMgr.Activities.EarningsGoal.EarningsGoalActivity;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_033_SimpleActivityOp;

public class MsgDealer_GC2GS_033_001_ReqEarningsGoalInfo extends NPUserMsgDealer<GC2GS_033_001_ReqEarningsGoalInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_033_001_ReqEarningsGoalInfo _msg)
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

        _committer.commitSucRes(US2GCWriter_033_SimpleActivityOp.make_001_RetEarningsGoalInfo(activity, _committer.getUserData().getCid()));
    }
}
