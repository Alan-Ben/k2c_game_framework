package NPServerProtocolWriter.NP2CS.Request;


import Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo;
import Common.ServerObj.ServerObj_PHPParamList;
import NP2CS_R.np_p002_serverInfoOp.*;
import NP2CS_R.p002_ServerInfoOP.NP2CS_R_002_015_ReqUpdateWhiteAccList;
import NPCommon.NP_SYS_ServerItem;
import NPEnum.EWhiteAccOpType;

import java.util.ArrayList;
import java.util.Collection;

public class NP2CS_R_Writer_002_ServerInfoOp
{
    public static NP2CS_R_002_001_ReqUSInfoListInit make_001_ReqUSInfoListInit()
    {
        NP2CS_R_002_001_ReqUSInfoListInit protocol = new NP2CS_R_002_001_ReqUSInfoListInit();
        return protocol;
    }

    public static NP2CS_R_002_002_ReqUSInfoList make_002_ReqUSInfoList(Collection<Integer> _usLogicIdList)
    {
        NP2CS_R_002_002_ReqUSInfoList protocol = new NP2CS_R_002_002_ReqUSInfoList();
        protocol.getUsLogicIdList().addAll(_usLogicIdList);
        return protocol;
    }

    public static NP2CS_R_002_004_ReqUpdateUSHoldInfo make_004_ReqUpdateUSHoldInfo(NpServerObj_SYS_ServerHoldInfo _holdInfo)
    {
        NP2CS_R_002_004_ReqUpdateUSHoldInfo protocol = new NP2CS_R_002_004_ReqUpdateUSHoldInfo();
        protocol.setHoldInfo(_holdInfo);
        return protocol;
    }

    public static NP2CS_R_002_005_ReqUSHoldInfo make_005_ReqUSHoldInfo(Collection<Integer> _serverTypeIdList)
    {
        NP2CS_R_002_005_ReqUSHoldInfo protocol = new NP2CS_R_002_005_ReqUSHoldInfo();
        protocol.getServerTypeIdList().addAll(_serverTypeIdList);
        return protocol;
    }

    public static NP2CS_R_002_006_ReqUSFreezeInfo make_006_ReqUSFreezeInfo(String _uid)
    {
        NP2CS_R_002_006_ReqUSFreezeInfo protocol = new NP2CS_R_002_006_ReqUSFreezeInfo();
        protocol.setUid(_uid);
        return protocol;
    }

    public static NP2CS_R_002_007_ReqUSInfoListByTypeId make_007_ReqUSInfoListByTypeId(int _serverTypeId)
    {
        NP2CS_R_002_007_ReqUSInfoListByTypeId protocol = new NP2CS_R_002_007_ReqUSInfoListByTypeId();
        protocol.setUsTypeId(_serverTypeId);
        return protocol;
    }

    public static NP2CS_R_002_008_ReqMostRecommendedUSInfo make_008_ReqMostRecommendedUSInfo()
    {
        NP2CS_R_002_008_ReqMostRecommendedUSInfo protocol = new NP2CS_R_002_008_ReqMostRecommendedUSInfo();
        return protocol;
    }

    public static NP2CS_R_002_011_ReqUpdateUSInfoList make_011_ReqUpdateUSInfoList(ArrayList<NP_SYS_ServerItem> _serverList)
    {
        NP2CS_R_002_011_ReqUpdateUSInfoList protocol = new NP2CS_R_002_011_ReqUpdateUSInfoList();
        protocol.getServerItemList().addAll(_serverList);
        return protocol;
    }

    public static NP2CS_R_002_014_ReqUpdatePHPParamList make_014_ReqUpdatePHPParamList(long _dataSerial, String _phpSerial, ServerObj_PHPParamList _paramListObj)
    {
    	NP2CS_R_002_014_ReqUpdatePHPParamList protocol = new NP2CS_R_002_014_ReqUpdatePHPParamList();
    	protocol.setDataSerial(_dataSerial);
    	protocol.setPhpSerial(_phpSerial);
        protocol.setPListObj(_paramListObj);
        return protocol;
    }

    /**
     * 构造请求增量更新白名单协议
     *
     * @param _opType 操作类型（ADD/REMOVE）
     * @param _acc 账号
     * @return 请求更新白名单列表协议对象
     */
    public static NP2CS_R_002_015_ReqUpdateWhiteAccList make_015_ReqUpdateWhiteAccList(EWhiteAccOpType _opType, String _acc)
    {
        NP2CS_R_002_015_ReqUpdateWhiteAccList protocol = new NP2CS_R_002_015_ReqUpdateWhiteAccList();
        protocol.setOpType(_opType);
        protocol.setAcc(_acc);
        return protocol;
    }
}
