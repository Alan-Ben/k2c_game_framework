package GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.p003_FristTeamActivityOp;

import GC2GS.p003_FristTeamActivityOp.GC2GS_003_001_ReqGameLogicTest;
import GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.ActivityMsgCommiter;
import GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.ActivityMsgDealer;
import GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.Write.Activity2GCWriter_003_FristTeamActivityOp;

public class ActivityDealer_GC2GS_003_001_ReqGameLogicTest extends ActivityMsgDealer<GC2GS_003_001_ReqGameLogicTest>
{
    @Override
    protected void _dealMessage(ActivityMsgCommiter _committer, GC2GS_003_001_ReqGameLogicTest _msg)
    {
        _committer.commitSucRes(Activity2GCWriter_003_FristTeamActivityOp.make_001_RetGameLogicTest(_msg.getParam1()));
    }
}
