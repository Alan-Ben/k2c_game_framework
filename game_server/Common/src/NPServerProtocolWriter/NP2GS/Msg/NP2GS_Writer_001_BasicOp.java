package NPServerProtocolWriter.NP2GS.Msg;

import NP2GS.p001_BasicOp.*;
import WCGCommon.Enum.NPEnum.EWCGKickOutGateType;

import java.nio.ByteBuffer;

public class NP2GS_Writer_001_BasicOp
{
    public static NP2GS_001_001_SendbackUserMsg make_001_SendbackUserMsg(long _sessionId, int _dealerSerialize, ByteBuffer _msg)
    {
        NP2GS_001_001_SendbackUserMsg protocol = new NP2GS_001_001_SendbackUserMsg();

        protocol.setSessionId(_sessionId);
        protocol.setDealerSerialize(_dealerSerialize);
        protocol.setMsg(_msg);

        return protocol;
    }

    public static NP2GS_001_002_UserGateKicked make_002_UserGateKicked(long _uid, long _connectSessionId, EWCGKickOutGateType _kickType)
    {
        NP2GS_001_002_UserGateKicked protocol = new NP2GS_001_002_UserGateKicked();

        protocol.setUid(_uid);
        protocol.setSessionId(_connectSessionId);
        protocol.setKickType(_kickType.ordinal());

        return protocol;
    }

    public static NP2GS_001_003_OnUserDataLoaded make_003_OnUserDataLoaded(int _errCode, long _gsSessionId, long _infoSerialize, long _cid)
    {
        NP2GS_001_003_OnUserDataLoaded protocol = new NP2GS_001_003_OnUserDataLoaded();

        protocol.setErrCode(_errCode);
        protocol.setGsSessionId(_gsSessionId);
        protocol.setInfoSerialize(_infoSerialize);
        protocol.setCid(_cid);

        return protocol;
    }

    public static NP2GS_001_004_OnEnterUSButFreeze make_004_OnEnterUSButFreeze(long _freezeTime, long _cid, long _gsSessionId, long _infoSerial)
    {
        NP2GS_001_004_OnEnterUSButFreeze protocol = new NP2GS_001_004_OnEnterUSButFreeze();
        protocol.setCid(_cid);
        protocol.setFreezeTimeMs(_freezeTime);
        protocol.setGsSessionId(_gsSessionId);
        protocol.setInfoSerialize(_infoSerial);
        return protocol;
    }

    public static NP2GS_001_005_UpdateUsServerInfo make_005_UpdateUsServerVersion(int _usId, String _serverVersion, String _resVersion)
    {
        NP2GS_001_005_UpdateUsServerInfo protocol = new NP2GS_001_005_UpdateUsServerInfo();
        protocol.setUsId(_usId);
        protocol.setServerVersion(_serverVersion);
        protocol.setResVersion(_resVersion);
        return protocol;
    }

    public static NP2GS_001_010_SendbackUserClientRequest make_010_SendbackUserClientRequest(
            long _sessionId, int _dealerSerialize, long _clientRequestSerialize, boolean _res, int _errCode, ByteBuffer _retMsg)
    {
        NP2GS_001_010_SendbackUserClientRequest protocol = new NP2GS_001_010_SendbackUserClientRequest();

        protocol.setSessionId(_sessionId);
        protocol.setDealerSerialize(_dealerSerialize);

        protocol.setClientRequestSerialize(_clientRequestSerialize);
        protocol.setRes(_res);
        protocol.setErrCode(_errCode);
        protocol.setRetMsg(_retMsg);

        return protocol;
    }
}
