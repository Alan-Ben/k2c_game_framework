package NPPlatServer.NPGeneralListener.Writer;

import Common.NpServerObj.NpServerObj_ServerInfo;
import NP2PS_RB.p001_BasicOp.*;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaInfo;
import NPPlatServer.NPPlatServerMgr.NPPlatCGSMgr;
import NPPlatServer.NPPlatServerMgr.NPPlatCRSMgr;
import NPPlatServer.NPPlatServerMgr.NPPlatGLSMgr;
import WCGBasicPlatServer.BasicServerListener._AWCGBasicServerListener;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.List;

public class NP2PS_RB_Writer_001_BasicOp
{
    public static NP2PS_RB_001_001_RetCSRegInfo make_001_RetRegInfo()
    {
        NP2PS_RB_001_001_RetCSRegInfo protocol = new NP2PS_RB_001_001_RetCSRegInfo();

        //所有CrossGame服务器列表
        NPPlatCGSMgr.getInstance().makeProtoList(protocol.getCrossGameServerTypeIdList());
        //所有CrossRank服务器列表
        NPPlatCRSMgr.getInstance().makeProtoList(protocol.getCrossRankServerTypeIdList());
        //所有GameLogic服务器列表
        NPPlatGLSMgr.getInstance().makeProtoList(protocol.getGameLogicServerTypeIdList());

        return protocol;
    }

    public static NP2PS_RB_001_002_RetGSRegInfo make_002_RetGSRegInfo(NPPlatAreaInfo areaInfo)
    {
        NP2PS_RB_001_002_RetGSRegInfo protocol = new NP2PS_RB_001_002_RetGSRegInfo();

        protocol.setAreaTagIdx(areaInfo.getAreaSerialize());

        return protocol;
    }

    public static NP2PS_RB_001_003_RetLCSRegInfo make_003_RetLCSRegInfo()
    {
        NP2PS_RB_001_003_RetLCSRegInfo protocol = new NP2PS_RB_001_003_RetLCSRegInfo();
        return protocol;
    }

    public static NP2PS_RB_001_004_RetLSRegInfo make_004_RetLSRegInfo(NPPlatAreaInfo areaInfo)
    {
        NP2PS_RB_001_004_RetLSRegInfo protocol = new NP2PS_RB_001_004_RetLSRegInfo();

        protocol.setAreaTagIdx(areaInfo.getAreaSerialize());

        return protocol;
    }

    public static NP2PS_RB_001_007_RetUSRegInfo make_007_RetUSRegInfo()
    {
        return new NP2PS_RB_001_007_RetUSRegInfo();
    }

    public static NP2PS_RB_001_009_RetCRSRegInfo make_009_RetCRSRegInfo()
    {
        return new NP2PS_RB_001_009_RetCRSRegInfo();
    }

    public static NP2PS_RB_001_010_RetCGSRegInfo make_010_RetCGSRegInfo(int _areaSerialize)
    {
        NP2PS_RB_001_010_RetCGSRegInfo protocol = new NP2PS_RB_001_010_RetCGSRegInfo();
        protocol.setAreaIndex(_areaSerialize);
        return protocol;
    }

    public static NP2PS_RB_001_011_RetOnlineServerList make_011_RetOnlineServerList(List<_AWCGBasicServerListener> _listenerList)
    {
        NP2PS_RB_001_011_RetOnlineServerList protocol = new NP2PS_RB_001_011_RetOnlineServerList();
        for (_AWCGBasicServerListener listener : _listenerList)
        {
            protocol.getServerTypeIdList().add(listener.getServerTypeId());
        }
        return protocol;
    }

    public static NP2PS_RB_001_012_RetOnlineRefServerList make_012_RetOnlineRefServerList(List<_AWCGBasicServerListener> _listenerList)
    {
        NP2PS_RB_001_012_RetOnlineRefServerList protocol = new NP2PS_RB_001_012_RetOnlineRefServerList();
        for (_AWCGBasicServerListener listener : _listenerList)
        {
            //跳过BUS MS
            if (listener.getServerType() == 9999 || listener.getServerType() == 10000)
                continue;
            //跳过LS
            if (listener.getServerType() == EServerType.LOGIN.ordinal())
                continue;
            //单服需要跳过LCS
            if (listener.getServerType() == EServerType.SINGLE.ordinal() && listener.getServerTypeId() == ENPSingleServerType.LOGIN_CHECK.ordinal())
                continue;
            //单服需要跳过RS
            if (listener.getServerType() == EServerType.SINGLE.ordinal() && listener.getServerTypeId() == ENPSingleServerType.RECORD.ordinal())
                continue;
            //单服需要跳过IS
            if (listener.getServerType() == EServerType.SINGLE.ordinal() && listener.getServerTypeId() == ENPSingleServerType.INTERFACE.ordinal())
                continue;
            protocol.getRefServerList().add(new NpServerObj_ServerInfo(listener.getServerType(), listener.getServerTypeId()));
        }
        return protocol;
    }

    public static NP2PS_RB_001_013_RegCrossDataServer make_013_RegCrossDataServer()
    {
        NP2PS_RB_001_013_RegCrossDataServer protocol = new NP2PS_RB_001_013_RegCrossDataServer();
        return protocol;
    }

    public static NP2PS_RB_001_014_RegGameLogicServer make_014_RegGameLogicServer()
    {
        NP2PS_RB_001_014_RegGameLogicServer protocol = new NP2PS_RB_001_014_RegGameLogicServer();
        return protocol;
    }
}
