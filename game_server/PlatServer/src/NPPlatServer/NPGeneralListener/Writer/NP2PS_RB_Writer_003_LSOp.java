package NPPlatServer.NPGeneralListener.Writer;

import NP2PS_RB.p003_LSOp.NP2PS_RB_003_001_RetGateKey;

public class NP2PS_RB_Writer_003_LSOp
{

    public static NP2PS_RB_003_001_RetGateKey make_002_RetGateKey(String _uid, String _gateServerIp, int _gateServerPort, String _checkCode)
    {
        NP2PS_RB_003_001_RetGateKey protocol = new NP2PS_RB_003_001_RetGateKey();

        protocol.setUid(_uid);
        protocol.setGateServerIp(_gateServerIp);
        protocol.setGateServerPort(_gateServerPort);
        protocol.setCheckCode(_checkCode);

        return protocol;
    }
}
