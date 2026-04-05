package NPServerProtocolWriter.NP2CGS.Request;


import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_002_ReqCreateCrossGameInstance;
import NPEnum.ENPCrossGameCategoryEnum;

public class NP2CGS_R_Writer_001_BasicOp
{
    public static NP2CGS_R_001_002_ReqCreateCrossGameInstance make_002_ReqCreateCrossGameInstance(ENPCrossGameCategoryEnum _type, long _instanceId, byte[] _ext)
    {
        NP2CGS_R_001_002_ReqCreateCrossGameInstance protocol = new NP2CGS_R_001_002_ReqCreateCrossGameInstance();
        protocol.setCategory(_type);
        protocol.setInstanceId(_instanceId);
        protocol.setExtData(_ext);

        return protocol;
    }

}
