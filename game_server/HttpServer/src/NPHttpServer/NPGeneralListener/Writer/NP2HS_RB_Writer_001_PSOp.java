package NPHttpServer.NPGeneralListener.Writer;


import Common.NpServerObj.NpServerObj_PlatFormMail;
import Common.ServerObj.ServerObj_PHPParamList;
import Common.ServerObj.ServerObj_WhiteAccList;
import NP2HS_RB.p001_HSOp.*;
import NPCommon.NP_SYS_ServerItem;

import java.util.ArrayList;
import java.util.Collection;

public class NP2HS_RB_Writer_001_PSOp
{
    public static NP2HS_RB_001_001_RetServerInfoList make_001_RetServerInfoList(long _serial, ArrayList<NP_SYS_ServerItem> _systemInfoList)
    {
        NP2HS_RB_001_001_RetServerInfoList proto = new NP2HS_RB_001_001_RetServerInfoList();
        proto.setSerial(_serial);
        proto.getServerInfoList().addAll(_systemInfoList);
        return proto;
    }

    public static NP2HS_RB_001_002_RetAllServerMail make_002_RetAllServerMail(long maxMailDbId, Collection<? extends NpServerObj_PlatFormMail> _mailList)
    {
        NP2HS_RB_001_002_RetAllServerMail proto = new NP2HS_RB_001_002_RetAllServerMail();
        proto.getPlatformMailList().addAll(_mailList);
        proto.setMailMaxId(maxMailDbId);
        return proto;
    }

    public static NP2HS_RB_001_004_RetCheckIsInWhiteList make_004_RetCheckIsInWhiteList(boolean _isInList)
    {
        NP2HS_RB_001_004_RetCheckIsInWhiteList proto = new NP2HS_RB_001_004_RetCheckIsInWhiteList();
        proto.setIsInWhiteList(_isInList);
        return proto;
    }

    public static NP2HS_RB_001_005_RetPlatformInfo make_005_RetPlatformInfo(int _platformId, int _platAreaId)
    {
        NP2HS_RB_001_005_RetPlatformInfo proto = new NP2HS_RB_001_005_RetPlatformInfo();
        proto.setPlatformId(_platformId);
        proto.setPlatAreaId(_platAreaId);
        return proto;
    }
    
    public static NP2HS_RB_001_006_RetPlatParamList make_006_RetPlatParamList(long _dataSerial, String _phpSerial, ServerObj_PHPParamList _paramListObj)
    {
    	NP2HS_RB_001_006_RetPlatParamList proto = new NP2HS_RB_001_006_RetPlatParamList();
        proto.setDataSerial(_dataSerial);
        proto.setPhpSerial(_phpSerial);
        proto.setPListObj(_paramListObj);
        return proto;
    }

    public static NP2HS_RB_001_010_RetPushActivityScheduleResult make_010_RetPushActivityScheduleResult()
    {
    	NP2HS_RB_001_010_RetPushActivityScheduleResult proto = new NP2HS_RB_001_010_RetPushActivityScheduleResult();
        return proto;
    }

    /**
     * 构造返回白名单列表协议
     *
     * @param _whiteAccList 白名单列表对象
     * @return 返回白名单列表协议对象
     */
    public static NP2HS_RB_001_007_RetWhiteAccList make_007_RetWhiteAccList(ServerObj_WhiteAccList _whiteAccList)
    {
        NP2HS_RB_001_007_RetWhiteAccList proto = new NP2HS_RB_001_007_RetWhiteAccList();
        proto.setWhiteAccList(_whiteAccList);
        return proto;
    }
}
