package NPGateServer.NPGCListener.Writer;

import ALBasicCommon.ALBasicCommonFun;
import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import NPCommon.NPVersion;
import NPCommon.NP_SYS_ServerItem;
import NPCommon.Util.CommonFunc;
import NPGS2GC.p001_BasicOp.*;
import NPGameRes.ServerVersionInfo;
import NPGateServer.USRefVersionMgr.USVersionInfo;
import NPGateServer.USRefVersionMgr.USVersionMgr;

import java.util.Collection;

public class NPGS2GCWriter_001_BasicOp
{
    public static NPGS2GC_001_001_RetBasicInfo make_001_RetBasicInfo(int _usServerId)
    {
        NPGS2GC_001_001_RetBasicInfo protocol = new NPGS2GC_001_001_RetBasicInfo();

        //获取数据对象
        USVersionInfo versionInfo = USVersionMgr.getInstance().getUSRefVersionInfo(_usServerId);
        if (null != versionInfo)
        {
            protocol.setVersion(versionInfo.getServerVersion());
            protocol.setResVersion(versionInfo.getResVersion());
        }else
        {
            protocol.setVersion(NPVersion.getString());
            protocol.setResVersion(ServerVersionInfo.getInstance().getVersion());
        }

        protocol.setTimezone(CommonFunc.getOffsetTimeZone());
        protocol.setTimeTag(CommonFunc.getNowTimeMS());
        protocol.setDstOffset(CommonFunc.getDstOffset());

        return protocol;
    }

    public static NPGS2GC_001_002_HeartPack make_002_HeartPack(long _clientHeartSerialize, long _clientTimeTag)
    {
        NPGS2GC_001_002_HeartPack proto = new NPGS2GC_001_002_HeartPack();

        proto.setClientTimeTag(_clientTimeTag);
        proto.setServerTimeTag(ALBasicCommonFun.getNowTimeMS());
        proto.setClientHeartSerialize(_clientHeartSerialize);

        return proto;
    }

    public static NPGS2GC_001_003_RetReloginKey make_003_RetReloginKey(String _reloginKey)
    {
        NPGS2GC_001_003_RetReloginKey proto = new NPGS2GC_001_003_RetReloginKey();
        proto.setReloginKey(_reloginKey);
        return proto;
    }

    public static NPGS2GC_001_004_OnUSEnterDone make_004_OnUSEnterDone(int _usId, long _cid, long _clientInitSerialize)
    {
        NPGS2GC_001_004_OnUSEnterDone proto = new NPGS2GC_001_004_OnUSEnterDone();

        proto.setErrCode(0);
        proto.setUsId(_usId);
        proto.setClientInitSerialize(_clientInitSerialize);
        proto.setServerTimeTag(ALBasicCommonFun.getNowTimeMS());
        proto.setCid(_cid);

        return proto;
    }

    public static NPGS2GC_001_004_OnUSEnterDone make_004_OnUSEnterDone(int _res, int _usId, long _clientInitSerialize)
    {
        NPGS2GC_001_004_OnUSEnterDone proto = new NPGS2GC_001_004_OnUSEnterDone();

        proto.setErrCode(_res);
        proto.setUsId(_usId);
        proto.setClientInitSerialize(_clientInitSerialize);
        proto.setServerTimeTag(ALBasicCommonFun.getNowTimeMS());

        return proto;
    }

    public static NPGS2GC_001_005_EnterUSRes make_005_EnterUSRes(int _res, long _clientInitSerialize, long _idx)
    {
        NPGS2GC_001_005_EnterUSRes proto = new NPGS2GC_001_005_EnterUSRes();

        proto.setError(_res);
        proto.setClientSerialize(_clientInitSerialize);
        proto.setQueueIndex(_idx);

        return proto;
    }

    public static NPGS2GC_001_005_EnterUSRes make_005_EnterUSRes(int _errCode, long _clientInitSerialize)
    {
        NPGS2GC_001_005_EnterUSRes proto = new NPGS2GC_001_005_EnterUSRes();

        proto.setError(_errCode);
        proto.setClientSerialize(_clientInitSerialize);

        return proto;
    }

    public static NPGS2GC_001_006_QuitUSRes make_006_QuitUSRes(int _errCode, long _clientInitSerialize)
    {
        NPGS2GC_001_006_QuitUSRes proto = new NPGS2GC_001_006_QuitUSRes();

        proto.setError(_errCode);
        proto.setClientSerialize(_clientInitSerialize);

        return proto;
    }

    public static NPGS2GC_001_007_RetQueueInfo make_007_RetQueueInfo(long _clientSerialize, long _curQueueIndex)
    {
        NPGS2GC_001_007_RetQueueInfo proto = new NPGS2GC_001_007_RetQueueInfo();

        proto.setClientSerialize(_clientSerialize);
        proto.setCurEnterIndex(_curQueueIndex);

        return proto;
    }

    public static NPGS2GC_001_008_QuitQueue make_008_QuitQueue(long _clientSerialize, int _errCode)
    {
        NPGS2GC_001_008_QuitQueue proto = new NPGS2GC_001_008_QuitQueue();

        proto.setClientSerialize(_clientSerialize);
        proto.setErrCode(_errCode);

        return proto;
    }

    public static NPGS2GC_001_009_ResumeUSConnectionRes make_009_ResumeUSConnectionRes(long _clientSerialize, int _errCode)
    {
        NPGS2GC_001_009_ResumeUSConnectionRes proto = new NPGS2GC_001_009_ResumeUSConnectionRes();

        proto.setError(_errCode);
        proto.setClientSerialize(_clientSerialize);
        proto.setServerTimeTag(ALBasicCommonFun.getNowTimeMS());

        return proto;
    }

    public static NPGS2GC_001_010_OnMsgInvalid make_010_OnMsgInvalid(long _sessionId)
    {
        NPGS2GC_001_010_OnMsgInvalid proto = new NPGS2GC_001_010_OnMsgInvalid();
        proto.setSessionId(_sessionId);
        return proto;
    }

    public static NPGS2GC_001_011_RetPlayerJoinedUSList make_011_RetPlayerJoinedUSList(Collection<? extends NP_SYS_PlayerJoinedUSInfo> _joinedUSList,
                                                                                       int _lastJoinUSLogicId)
    {
        NPGS2GC_001_011_RetPlayerJoinedUSList proto = new NPGS2GC_001_011_RetPlayerJoinedUSList();
        proto.getJoinedUSList().addAll(_joinedUSList);
        proto.setLastJoinUSLogicId(_lastJoinUSLogicId);
        return proto;
    }

    public static NPGS2GC_001_012_RetMostRecommendedUSInfo make_012_RetMostRecommendedUSInfo(NP_SYS_ServerItem _serverItem)
    {
        NPGS2GC_001_012_RetMostRecommendedUSInfo proto = new NPGS2GC_001_012_RetMostRecommendedUSInfo();
        proto.setServerItem(_serverItem);
        return proto;
    }

    public static NPGS2GC_001_013_EnterUSButFreeze make_013_EnterUSButFreeze(long _clientSerial, long _freezeTimeMs)
    {
        NPGS2GC_001_013_EnterUSButFreeze proto = new NPGS2GC_001_013_EnterUSButFreeze();
        proto.setClientSerialize(_clientSerial);
        proto.setFreezeTimeMs(_freezeTimeMs);
        return proto;
    }

    public static NPGS2GC_001_020_RetReconnectInfo make_020_RetReconnectInfo(int _receivedMesCount)
    {
        NPGS2GC_001_020_RetReconnectInfo proto = new NPGS2GC_001_020_RetReconnectInfo();
        proto.setReceivedMesCount(_receivedMesCount);
        return proto;
    }

    public static NPGS2GC_001_021_ReceivedMsg make_021_ReceivedMsg(int _receivedMesCount)
    {
        NPGS2GC_001_021_ReceivedMsg proto = new NPGS2GC_001_021_ReceivedMsg();
        proto.setReceivedMesCount(_receivedMesCount);
        return proto;
    }

    public static NPGS2GC_001_022_SendSerializeMsg make_022_SendSerializeMsg(int _serialize, byte[] _msg)
    {
        NPGS2GC_001_022_SendSerializeMsg proto = new NPGS2GC_001_022_SendSerializeMsg();
        proto.setMsgSerialize(_serialize);
        proto.setMsg(_msg);
        return proto;
    }

    public static NPGS2GC_001_023_RetClientRequest make_023_RetClientRequest(
            int _serialize, long _clientRequestSerialize, boolean _res, int _errCode, byte[] _msg)
    {
        NPGS2GC_001_023_RetClientRequest proto = new NPGS2GC_001_023_RetClientRequest();

        proto.setMsgSerialize(_serialize);

        proto.setClientRequestSerialize(_clientRequestSerialize);

        proto.setRes(_res);
        proto.setErrCode(_errCode);

        proto.setMsgBuffer(_msg);

        return proto;
    }

    public static NPGS2GC_001_030_BeDeviceKicked make_030()
    {
        NPGS2GC_001_030_BeDeviceKicked proto = new NPGS2GC_001_030_BeDeviceKicked();

        return proto;
    }
}
