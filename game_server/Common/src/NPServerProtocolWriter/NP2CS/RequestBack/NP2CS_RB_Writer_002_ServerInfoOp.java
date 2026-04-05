package NPServerProtocolWriter.NP2CS.RequestBack;


import Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo;
import Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo;
import Common.NpServerObj.NpServerObj_SYS_ServerItem;
import NP2CS_RB.np_p002_serverInfoOp.*;
import NP2CS_RB.p002_ServerInfoOP.NP2CS_RB_002_015_RetUpdateWhiteAccList;
import NPCommon.NP_SYS_ServerItem;

import java.util.Collection;

public class NP2CS_RB_Writer_002_ServerInfoOp
{
    public static NP2CS_RB_002_001_RetUSInfoListInit make_001_ReqUSInfoListInit(Collection<? extends NpServerObj_SYS_ServerIndexInfo> _serverList)
    {
        NP2CS_RB_002_001_RetUSInfoListInit protocol = new NP2CS_RB_002_001_RetUSInfoListInit();
        protocol.getServerIndexList().addAll(_serverList);
        return protocol;
    }

    public static NP2CS_RB_002_002_RetUSInfoList make_002_ReqUSInfoList(Collection<? extends NP_SYS_ServerItem> _itemList)
    {
        NP2CS_RB_002_002_RetUSInfoList protocol = new NP2CS_RB_002_002_RetUSInfoList();
        protocol.getServerItemList().addAll(_itemList);
        return protocol;
    }

    public static NP2CS_RB_002_004_RetUpdateUSHoldInfo make_004_RetUpdateUSHoldInfo()
    {
        NP2CS_RB_002_004_RetUpdateUSHoldInfo protocol = new NP2CS_RB_002_004_RetUpdateUSHoldInfo();
        return protocol;
    }

    public static NP2CS_RB_002_005_RetUSHoldInfo make_005_RetUSHoldInfo(Collection<? extends NpServerObj_SYS_ServerHoldInfo> _holdInfoList)
    {
        NP2CS_RB_002_005_RetUSHoldInfo protocol = new NP2CS_RB_002_005_RetUSHoldInfo();
        protocol.getHoldInfoList().addAll(_holdInfoList);
        return protocol;
    }

    public static NP2CS_RB_002_006_RetUSFreezeInfo make_006_RetUSFreezeInfo(boolean _isFreeze, long _freezeTimeMs, boolean _inWhiteList)
    {
        NP2CS_RB_002_006_RetUSFreezeInfo protocol = new NP2CS_RB_002_006_RetUSFreezeInfo();
        protocol.setIsFreeze(_isFreeze);
        protocol.setFreezeTimeMs(_freezeTimeMs);
        protocol.setInWhitelist(_inWhiteList);
        return protocol;
    }

    public static NP2CS_RB_002_007_RetUSInfoListByTypeId make_007_RetUSInfoListByTypeId(NpServerObj_SYS_ServerItem _serverItem)
    {
        NP2CS_RB_002_007_RetUSInfoListByTypeId protocol = new NP2CS_RB_002_007_RetUSInfoListByTypeId();
        protocol.setServerItem(_serverItem);
        return protocol;
    }

    public static NP2CS_RB_002_008_RetMostRecommendedUSInfo make_008_RetMostRecommendedUSInfo(NP_SYS_ServerItem _serverItem)
    {
        NP2CS_RB_002_008_RetMostRecommendedUSInfo protocol = new NP2CS_RB_002_008_RetMostRecommendedUSInfo();
        protocol.setServerItem(_serverItem);
        return protocol;
    }

    public static NP2CS_RB_002_009_RetTryCrossGameServer make_009_RetTryCrossGameServer(int _serverTypeId, long _instanceId)
    {
        NP2CS_RB_002_009_RetTryCrossGameServer protocol = new NP2CS_RB_002_009_RetTryCrossGameServer();
        protocol.setServerTypeId(_serverTypeId);
        protocol.setInstanceId(_instanceId);

        return protocol;
    }

    public static NP2CS_RB_002_010_RetSetCrossGameWeight make_010_RetSetCrossGameWeight()
    {
        NP2CS_RB_002_010_RetSetCrossGameWeight protocol = new NP2CS_RB_002_010_RetSetCrossGameWeight();

        return protocol;
    }

    public static NP2CS_RB_002_011_RetUpdateUSInfoList make_011_RetUpdateUSInfoList()
    {
        NP2CS_RB_002_011_RetUpdateUSInfoList protocol = new NP2CS_RB_002_011_RetUpdateUSInfoList();

        return protocol;
    }

    public static NP2CS_RB_002_012_RetBanUid make_012_RetBanUid()
    {
        NP2CS_RB_002_012_RetBanUid protocol = new NP2CS_RB_002_012_RetBanUid();

        return protocol;
    }

    public static NP2CS_RB_002_013_RetUnBanUid make_013_RetUnBanUid()
    {
        NP2CS_RB_002_013_RetUnBanUid protocol = new NP2CS_RB_002_013_RetUnBanUid();

        return protocol;
    }

    public static NP2CS_RB_002_014_RetUpdatePHPParamList make_014_ReqPHPParamList()
    {
    	NP2CS_RB_002_014_RetUpdatePHPParamList protocol = new NP2CS_RB_002_014_RetUpdatePHPParamList();

        return protocol;
    }

    /**
     * 构造返回更新白名单列表协议
     *
     * @return 返回更新白名单列表协议对象
     */
    public static NP2CS_RB_002_015_RetUpdateWhiteAccList make_015_RetUpdateWhiteAccList()
    {
        return new NP2CS_RB_002_015_RetUpdateWhiteAccList();
    }
}
