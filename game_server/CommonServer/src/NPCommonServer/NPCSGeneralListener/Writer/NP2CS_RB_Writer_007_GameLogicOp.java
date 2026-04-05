package NPCommonServer.NPCSGeneralListener.Writer;

import NP2CS_RB.np_p007_GameLogicOp.NP2CS_RB_007_001_RetGameLogicInstance;
import NP2CS_RB.np_p007_GameLogicOp.NP2CS_RB_007_002_RetDiscardGameLogicInstance;

public class NP2CS_RB_Writer_007_GameLogicOp
{
    public static NP2CS_RB_007_001_RetGameLogicInstance make_001_RetGameLogicInstance(long _instanceId)
    {
        NP2CS_RB_007_001_RetGameLogicInstance proto = new NP2CS_RB_007_001_RetGameLogicInstance();
        proto.setInstanceId(_instanceId);

        return proto;
    }

    public static NP2CS_RB_007_002_RetDiscardGameLogicInstance make_002_RetDiscardGameLogicInstance()
    {
        NP2CS_RB_007_002_RetDiscardGameLogicInstance proto = new NP2CS_RB_007_002_RetDiscardGameLogicInstance();

        return proto;
    }
}
