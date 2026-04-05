package NPServerProtocolWriter.NP2CS.Msg;

import NP2CS_R.np_p002_serverInfoOp.NP2CS_R_002_012_ReqBanUid;
import NP2CS_R.np_p002_serverInfoOp.NP2CS_R_002_013_ReqUnBanUid;

public class NP2CS_Writer_002_BasicOp
{

    public static NP2CS_R_002_012_ReqBanUid make_012_ReqBanUid(String _uid, long _freezeTimeMs)
    {
        NP2CS_R_002_012_ReqBanUid protocol = new NP2CS_R_002_012_ReqBanUid();
        protocol.setUid(_uid);
        protocol.setFreezeTimeMs(_freezeTimeMs);

        return protocol;
    }

    public static NP2CS_R_002_013_ReqUnBanUid make_013_ReqUnBanUid(String _uid)
    {
        NP2CS_R_002_013_ReqUnBanUid protocol = new NP2CS_R_002_013_ReqUnBanUid();
        protocol.setUid(_uid);
        return protocol;
    }
}
