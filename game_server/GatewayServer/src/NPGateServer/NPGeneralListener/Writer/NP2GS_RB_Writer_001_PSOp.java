package NPGateServer.NPGeneralListener.Writer;

import NP2GS_RB.p001_PSOp.NP2GS_RB_001_001_RetUserKey;

public class NP2GS_RB_Writer_001_PSOp
{
    public static NP2GS_RB_001_001_RetUserKey make_001_RetUserKeySuc(String _uid, String _checkCode)
    {
        NP2GS_RB_001_001_RetUserKey protocol = new NP2GS_RB_001_001_RetUserKey();

        protocol.setRes(true);
        protocol.setUid(_uid);
        protocol.setCheckCode(_checkCode);

        return protocol;
    }
}
