package NPServerProtocolWriter.NP2PS.Request;

import NP2PS_R.p003_LSOp.NP2PS_R_003_001_ReqGateServer;

public class NP2PS_R_Writer_003_LSOp
{
    public static NP2PS_R_003_001_ReqGateServer make_001_ReqGateServer(String _uid)
    {
        NP2PS_R_003_001_ReqGateServer protocol = new NP2PS_R_003_001_ReqGateServer();

        protocol.setUid(_uid);

        return protocol;
    }
}
