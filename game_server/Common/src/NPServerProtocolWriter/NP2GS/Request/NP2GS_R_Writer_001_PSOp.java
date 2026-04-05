package NPServerProtocolWriter.NP2GS.Request;

import NP2GS_R.p001_PSOp.NP2GS_R_001_001_ReqUserKey;

public class NP2GS_R_Writer_001_PSOp
{
    public static NP2GS_R_001_001_ReqUserKey make_001_ReqUserKey(String _uid)
    {
        NP2GS_R_001_001_ReqUserKey protocol = new NP2GS_R_001_001_ReqUserKey();

        protocol.setUid(_uid);

        return protocol;
    }
}
