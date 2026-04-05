package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_062_ReqStageGoalInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;


public class MsgDealer_GC2GS_002_062_ReqStageGoalInit extends NPUserMsgDealer<GC2GS_002_062_ReqStageGoalInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_062_ReqStageGoalInit _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_062_RetStageGoalInit(userData));
    }
}
