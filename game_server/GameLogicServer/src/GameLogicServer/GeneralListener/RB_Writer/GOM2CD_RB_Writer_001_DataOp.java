package GameLogicServer.GeneralListener.RB_Writer;

import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_005_RegGroupInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_006_DiscardInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_007_QuitInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_008_RegUs;

public class GOM2CD_RB_Writer_001_DataOp
{
    public static NP2GLS_RB_001_005_RegGroupInstance make_005_RegGroupInstance(long _instanceId)
    {
        NP2GLS_RB_001_005_RegGroupInstance proto = new NP2GLS_RB_001_005_RegGroupInstance();
        proto.setInstanceId(_instanceId);

        return proto;
    }

    public static NP2GLS_RB_001_006_DiscardInstance make_006_DiscardInstance()
    {
        NP2GLS_RB_001_006_DiscardInstance proto = new NP2GLS_RB_001_006_DiscardInstance();

        return proto;
    }

    public static NP2GLS_RB_001_007_QuitInstance make_007_QuitInstance()
    {
        NP2GLS_RB_001_007_QuitInstance proto = new NP2GLS_RB_001_007_QuitInstance();

        return proto;
    }

    public static NP2GLS_RB_001_008_RegUs make_008_RegUs()
    {
        NP2GLS_RB_001_008_RegUs proto = new NP2GLS_RB_001_008_RegUs();

        return proto;
    }
}

