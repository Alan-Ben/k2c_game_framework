package NPServerProtocolWriter.NP2US.Request;

import NP2US_R.p002_GSOp.*;

public class NP2US_R_Writer_002_GSOp
{
    public static NP2US_R_002_001_RegUserGate make_001_RegUserGate(long _cid, long _sessionId, int _msgDealerSerialize, String _remoteIP, String _customData, String _areaTag)
    {
        NP2US_R_002_001_RegUserGate protocol = new NP2US_R_002_001_RegUserGate();

        protocol.setCid(_cid);
        protocol.setSessionId(_sessionId);
        protocol.setDealerSerialize(_msgDealerSerialize);
        protocol.setClientIp(_remoteIP);
        protocol.setCustomData(_customData);
        protocol.setAreaTag(_areaTag);

        return protocol;
    }

    public static NP2US_R_002_002_UnregUserGate make_002_UnregUserGate(long _sessionId, long _cid, long _serialize)
    {
        NP2US_R_002_002_UnregUserGate protocol = new NP2US_R_002_002_UnregUserGate();
        protocol.setSessionId(_sessionId);
        protocol.setCid(_cid);
        protocol.setSerialize(_serialize);

        return protocol;
    }

    public static NP2US_R_002_004_ReqResumeUSInfo make_004_ReqResumeUSInfo(String _uid, long _gsSessionId, long _gsInfoSerialize)
    {
        NP2US_R_002_004_ReqResumeUSInfo protocol = new NP2US_R_002_004_ReqResumeUSInfo();

        protocol.setUid(_uid);
        protocol.setGsSessionId(_gsSessionId);
        protocol.setGsInfoSerialize(_gsInfoSerialize);

        return protocol;
    }

    public static NP2US_R_002_005_ReqEnterUS make_005_ReqEnterUS(String _uid, long _gsSessionId, long _gsInfoSerialize, String _clientCustomData)
    {
        NP2US_R_002_005_ReqEnterUS protocol = new NP2US_R_002_005_ReqEnterUS();

        protocol.setUid(_uid);
        protocol.setGsSessionId(_gsSessionId);
        protocol.setGsInfoSerialize(_gsInfoSerialize);
        protocol.setCustomData(_clientCustomData);
        return protocol;
    }

    public static NP2US_R_002_006_ReqQueueInfo make_006_ReqQueueInfo()
    {
        NP2US_R_002_006_ReqQueueInfo protocol = new NP2US_R_002_006_ReqQueueInfo();

        return protocol;
    }

    public static NP2US_R_002_007_ReqQuitQueue make_007_ReqQuitQueue(long _infoSerialize, long _curQueueIndex)
    {
        NP2US_R_002_007_ReqQuitQueue protocol = new NP2US_R_002_007_ReqQuitQueue();

        protocol.setInfoSerialize(_infoSerialize);
        protocol.setCurQueueIndex(_curQueueIndex);

        return protocol;
    }
}
