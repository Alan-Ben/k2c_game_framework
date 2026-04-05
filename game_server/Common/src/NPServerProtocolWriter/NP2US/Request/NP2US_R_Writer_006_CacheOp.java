package NPServerProtocolWriter.NP2US.Request;

import NP2US_R.p006_CacheOp.NP2US_R_006_001_GetPlayerShowInfo;
import NP2US_R.p006_CacheOp.NP2US_R_006_002_GetPlayerIconShowInfo;
import NP2US_R.p006_CacheOp.NP2US_R_006_004_GetPlayerJoinUSInfo;

public class NP2US_R_Writer_006_CacheOp
{
    public static NP2US_R_006_001_GetPlayerShowInfo make_001_GetPlayerShowInfo(long _cid)
    {
        NP2US_R_006_001_GetPlayerShowInfo protocol = new NP2US_R_006_001_GetPlayerShowInfo();
        protocol.setCid(_cid);
        return protocol;
    }

    public static NP2US_R_006_002_GetPlayerIconShowInfo make_002_GetPlayerIconShowInfo(long _cid)
    {
        NP2US_R_006_002_GetPlayerIconShowInfo protocol = new NP2US_R_006_002_GetPlayerIconShowInfo();
        protocol.setCid(_cid);
        return protocol;
    }

    public static NP2US_R_006_004_GetPlayerJoinUSInfo make_004_GetPlayerJoinUSInfo(long _cid)
    {
        NP2US_R_006_004_GetPlayerJoinUSInfo protocol = new NP2US_R_006_004_GetPlayerJoinUSInfo();
        protocol.setCid(_cid);
        return protocol;
    }


}
