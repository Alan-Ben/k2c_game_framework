package NPUSServer.NPUserMsgDispather.p007_CommOp;

import Common.StageGoalObj.StageGoal_BigStepFirstReachInfo;
import Common.StageGoalObj.StageGoal_TopPlayerInfo;
import GC2GS.p007_CommOp.GC2GS_007_017_ReqStageGoalFirstReachBaseInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

import java.util.List;

public class MsgDealer_GC2GS_007_017_ReqStageGoalFirstReachBaseInfo extends NPUserMsgDealer<GC2GS_007_017_ReqStageGoalFirstReachBaseInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_017_ReqStageGoalFirstReachBaseInfo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();


        StageGoal_TopPlayerInfo topInfo = getUSServer().getStageGoalFirstReachMgr().makeTop1BaseInfo();
        List<StageGoal_BigStepFirstReachInfo> list = getUSServer().getStageGoalFirstReachMgr().makeBaseList();

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_017_RetStageGoalFirstReachBaseInfo(topInfo,list));
    }
} 