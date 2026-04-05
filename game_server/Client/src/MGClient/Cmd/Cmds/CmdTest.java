package MGClient.Cmd.Cmds;

import GC2GS.p003_FristTeamActivityOp.GC2GS_003_001_ReqGameLogicTest;
import GC2GS.p003_FristTeamActivityOp.GC2GS_003_002_ReqGameLogicRedirectTest;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "测试", name = "test")
public class CmdTest extends CmdBase
{
    @Command(comment = "测试GameLogic")
    public void ReqGameLogicTest(long _instanceId, int _param1)
    {
        GC2GS_003_001_ReqGameLogicTest msg = new GC2GS_003_001_ReqGameLogicTest();
        msg.setInstanceId(_instanceId);
        msg.setParam1(_param1);
        getOwner().sendGameMsg(msg);
    }

    @Command(comment = "测试GameLogic")
    public void ReqGameLogicRedirectTest(long _instanceId)
    {
        GC2GS_003_002_ReqGameLogicRedirectTest msg = new GC2GS_003_002_ReqGameLogicRedirectTest();
        msg.setInstanceId(_instanceId);
        getOwner().sendGameMsg(msg);
    }
}
