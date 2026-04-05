package NPUSServer.NPUserMsgDispather.Write;

import GS2GC.p003_FristTeamActivityOp.GS2GC_003_001_RetGameLogicTest;
import GS2GC.p003_FristTeamActivityOp.GS2GC_003_002_RetGameLogicRedirectTest;

public class US2GCWriter_003_FristTeamActivityOp
{
    public static GS2GC_003_001_RetGameLogicTest make_001_RetGameLogicTest(int _param1)
    {
        GS2GC_003_001_RetGameLogicTest proto = new GS2GC_003_001_RetGameLogicTest();
        proto.setParam1(_param1);

        return proto;
    }

    public static GS2GC_003_002_RetGameLogicRedirectTest make_002_RetGameLogicRedirectTest(long _param1)
    {
        GS2GC_003_002_RetGameLogicRedirectTest proto = new GS2GC_003_002_RetGameLogicRedirectTest();
        proto.setParam1(_param1);

        return proto;
    }
}

