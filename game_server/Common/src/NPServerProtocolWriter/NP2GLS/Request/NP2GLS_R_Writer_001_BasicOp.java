package NPServerProtocolWriter.NP2GLS.Request;


import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_005_RegGroupInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_006_DiscardInstance;

import java.nio.ByteBuffer;

public class NP2GLS_R_Writer_001_BasicOp
{
    public static NP2GLS_R_001_005_RegGroupInstance make_005_RegGroupInstance(long _groupId, long _activityId, ByteBuffer _addInfo)
    {
        NP2GLS_R_001_005_RegGroupInstance proto = new NP2GLS_R_001_005_RegGroupInstance();
        proto.setGroupId(_groupId);
        proto.setActivityId(_activityId);

        if(null != _addInfo)
            proto.setAddInfo(_addInfo);

        return proto;
    }

    public static NP2GLS_R_001_006_DiscardInstance make_006_DiscardInstance(long _instanceId)
    {
        NP2GLS_R_001_006_DiscardInstance proto = new NP2GLS_R_001_006_DiscardInstance();
        proto.setInstanceId(_instanceId);

        return proto;
    }
}
