package NPLoginServer.NPGCListener.Writer;

import NPLS2GC.p001_BasicOp.*;
import WCGCommon.Enum.NPEnum.EWCGCountryType;

public class NPLS2GCWriter_001_BasicOp
{
    public static NPLS2GC_001_001_RetBasicInfo make_001_RetBasicInfo(String _uid, EWCGCountryType _countryType)
    {
        NPLS2GC_001_001_RetBasicInfo protocol = new NPLS2GC_001_001_RetBasicInfo();

        protocol.setUid(_uid);
        protocol.setCountryIdx(_countryType.ordinal());

        return protocol;
    }

    public static NPLS2GC_001_002_EnterGameRes make_002_EnterGameSuc(String _uid, String _checkCode, String _connectIp, int _connectPort, boolean _inWhiteList)
    {
        NPLS2GC_001_002_EnterGameRes protocol = new NPLS2GC_001_002_EnterGameRes();
        protocol.setRes(true);
        protocol.setGateServerIp(_connectIp);
        protocol.setGateServerPort(_connectPort);
        protocol.setUid(_uid);
        protocol.setCheckCode(_checkCode);
        protocol.setIsWhite(_inWhiteList);
        return protocol;
    }

    public static NPLS2GC_001_002_EnterGameRes make_002_EnterGameFail()
    {
        NPLS2GC_001_002_EnterGameRes protocol = new NPLS2GC_001_002_EnterGameRes();

        protocol.setRes(false);

        return protocol;
    }

    public static NPLS2GC_001_003_CancelQueueRes make_003_CancelQueueSuc()
    {
        NPLS2GC_001_003_CancelQueueRes protocol = new NPLS2GC_001_003_CancelQueueRes();

        protocol.setRes(true);

        return protocol;
    }

    public static NPLS2GC_001_003_CancelQueueRes make_003_CancelQueueFail()
    {
        NPLS2GC_001_003_CancelQueueRes protocol = new NPLS2GC_001_003_CancelQueueRes();

        protocol.setRes(false);

        return protocol;
    }

    public static NPLS2GC_001_004_EnterQueue make_004_EnterQueue(long _queueIdx)
    {
        NPLS2GC_001_004_EnterQueue protocol = new NPLS2GC_001_004_EnterQueue();

        protocol.setQueueIdx(_queueIdx);

        return protocol;
    }

    public static NPLS2GC_001_005_RetQueueHeadIdx make_005_RetQueueHeadIdx(long _headQueueIdx)
    {
        NPLS2GC_001_005_RetQueueHeadIdx protocol = new NPLS2GC_001_005_RetQueueHeadIdx();

        protocol.setHeadQueueIdx(_headQueueIdx);

        return protocol;
    }
}
