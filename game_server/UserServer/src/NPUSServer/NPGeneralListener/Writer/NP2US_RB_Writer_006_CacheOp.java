package NPUSServer.NPGeneralListener.Writer;

import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import Common.NpPlayerInfoObj.PlayerInfo_CommonShow;
import NP2US_RB.p006_CacheOp.NP2US_RB_006_001_GetPlayerShowInfo;
import NP2US_RB.p006_CacheOp.NP2US_RB_006_002_GetPlayerIconShowInfo;
import NP2US_RB.p006_CacheOp.NP2US_RB_006_004_GetPlayerJoinUSInfo;
import NPCommon.PlayerInfo_IconShow;

public class NP2US_RB_Writer_006_CacheOp
{
    public static NP2US_RB_006_001_GetPlayerShowInfo make_001_GetPlayerShowInfo(PlayerInfo_CommonShow _showInfo)
    {
        NP2US_RB_006_001_GetPlayerShowInfo proto = new NP2US_RB_006_001_GetPlayerShowInfo();
        proto.setShowInfo(_showInfo);
        return proto;
    }

    public static NP2US_RB_006_002_GetPlayerIconShowInfo make_002_GetPlayerIconShowInfo(PlayerInfo_IconShow _showInfo)
    {
        NP2US_RB_006_002_GetPlayerIconShowInfo proto = new NP2US_RB_006_002_GetPlayerIconShowInfo();
        proto.setShowInfo(_showInfo);
        return proto;
    }

    public static NP2US_RB_006_004_GetPlayerJoinUSInfo make_004_GetPlayerJoinUSInfo(NP_SYS_PlayerJoinedUSInfo _joinUSInfo)
    {
        NP2US_RB_006_004_GetPlayerJoinUSInfo proto = new NP2US_RB_006_004_GetPlayerJoinUSInfo();
        proto.setJoinInfo(_joinUSInfo);
        return proto;
    }
}
