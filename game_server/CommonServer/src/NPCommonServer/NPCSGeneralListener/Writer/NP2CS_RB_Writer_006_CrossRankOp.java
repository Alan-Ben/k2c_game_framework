package NPCommonServer.NPCSGeneralListener.Writer;

import NP2CS_RB.np_p006_RankOp.NP2CS_RB_006_001_RetCrossInstance;
import NP2CS_RB.np_p006_RankOp.NP2CS_RB_006_002_RetDiscardCrossInstance;

public class NP2CS_RB_Writer_006_CrossRankOp
{
    public static NP2CS_RB_006_001_RetCrossInstance make_001_RetCrossInstance(long _instanceId)
    {
        NP2CS_RB_006_001_RetCrossInstance protocol = new NP2CS_RB_006_001_RetCrossInstance();
        protocol.setCrossInstanceId(_instanceId);
        return protocol;
    }

    public static NP2CS_RB_006_002_RetDiscardCrossInstance make_002_RetDiscardCrossInstance()
    {
        return new NP2CS_RB_006_002_RetDiscardCrossInstance();
    }
}
