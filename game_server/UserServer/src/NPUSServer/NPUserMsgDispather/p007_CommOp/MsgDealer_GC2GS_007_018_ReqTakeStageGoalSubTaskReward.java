package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_018_ReqTakeStageGoalSubTaskReward;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.StageGoalErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.StageGlobalComp.StageGoalTaskInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_018_ReqTakeStageGoalSubTaskReward extends NPUserMsgDealer<GC2GS_007_018_ReqTakeStageGoalSubTaskReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_018_ReqTakeStageGoalSubTaskReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        StageGoalTaskInfo taskInfo = userData.getStageGoalComponent().lookupTask(_msg.getTaskId());
        if (taskInfo == null)
        {
            _commiter.commitFailRes(StageGoalErr.STAGE_GOAL_TASK_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TAKE_STAGE_GOAL_TASK_REWARD);

        Result result = taskInfo.drawReward(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_018_RetTakeStageGoalSubTaskReward(context));
    }
} 