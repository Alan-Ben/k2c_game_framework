package NPUSServer.NPUserMsgDispather.p007_CommOp;

import Common.StageGoalObj.StageGoal_BigStepFirstReachInfo;
import GC2GS.p007_CommOp.GC2GS_007_016_ReqStageGoalFirstReachDetailInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

import java.util.List;

public class MsgDealer_GC2GS_007_016_ReqStageGoalFirstReachDetailInfo extends NPUserMsgDealer<GC2GS_007_016_ReqStageGoalFirstReachDetailInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_016_ReqStageGoalFirstReachDetailInfo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();


        List<StageGoal_BigStepFirstReachInfo> list = getUSServer().getStageGoalFirstReachMgr().makeDetailInfo(_msg.getBigStepId());

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_016_RetStageGoalFirstReachDetailInfo(list));
    }
}