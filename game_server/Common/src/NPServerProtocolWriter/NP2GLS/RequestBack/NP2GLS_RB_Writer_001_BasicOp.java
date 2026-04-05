package NPServerProtocolWriter.NP2GLS.RequestBack;


import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_005_RegGroupInstance;

public class NP2GLS_RB_Writer_001_BasicOp
{
    public static NP2GLS_RB_001_005_RegGroupInstance make_005_RegGroupInstance(long _instanceId)
    {
        NP2GLS_RB_001_005_RegGroupInstance proto = new NP2GLS_RB_001_005_RegGroupInstance();
        proto.setInstanceId(_instanceId);

        return proto;
    }
}
