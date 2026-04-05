package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_019_ReqTakeStageGoalBigStepReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_019_ReqTakeStageGoalBigStepReward extends NPUserMsgDealer<GC2GS_007_019_ReqTakeStageGoalBigStepReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_019_ReqTakeStageGoalBigStepReward _msg)
    {
    	NPUSUserData userData = _commiter.getUserData();
    	
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TAKE_STAGE_GOAL_BIG_STEP_REWARD);
        Result result = userData.getStageGoalComponent().doneBigStep(_msg.getBigStepId(), context);
        if(!result.isSucc())
		{
        	_commiter.commitFailRes(result.getCode());
        	return;
		}
        
        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_019_RetTakeStageGoalBigStepReward(context));
    }
}
