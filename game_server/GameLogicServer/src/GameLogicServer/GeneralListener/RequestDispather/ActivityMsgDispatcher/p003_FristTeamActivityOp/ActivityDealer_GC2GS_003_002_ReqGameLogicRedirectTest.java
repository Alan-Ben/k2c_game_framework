package GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.p003_FristTeamActivityOp;

import Common.ServerObj.ServerObj_GameLogicTestAdd;
import GC2GS.p003_FristTeamActivityOp.GC2GS_003_002_ReqGameLogicRedirectTest;
import GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.ActivityMsgCommiter;
import GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.ActivityMsgDealer;
import GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher.Write.Activity2GCWriter_003_FristTeamActivityOp;

public class ActivityDealer_GC2GS_003_002_ReqGameLogicRedirectTest extends ActivityMsgDealer<GC2GS_003_002_ReqGameLogicRedirectTest>
{
    @Override
    protected void _dealMessage(ActivityMsgCommiter _committer, GC2GS_003_002_ReqGameLogicRedirectTest _msg)
    {
        ServerObj_GameLogicTestAdd addInfo = new ServerObj_GameLogicTestAdd();
        addInfo.readPackage(_committer.getAddInfo());

        _committer.commitSucRes(Activity2GCWriter_003_FristTeamActivityOp.make_002_RetGameLogicRedirectTest(addInfo.getParam1()));
    }
}
