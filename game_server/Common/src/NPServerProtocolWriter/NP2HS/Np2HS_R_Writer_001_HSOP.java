package NPServerProtocolWriter.NP2HS;


import NP2HS_R.p001_HSOp.*;

/**
 * @description:
 * @author: ricci
 * @date: 2023-03-24 10:00:03
 */
public class Np2HS_R_Writer_001_HSOP
{
    public static NP2HS_R_001_001_ReqServerInfoList make_001_001_ReqServerInfoList(long _serial)
    {
        NP2HS_R_001_001_ReqServerInfoList proto = new NP2HS_R_001_001_ReqServerInfoList();
        proto.setSerial(_serial);
        return proto;
    }

    public static NP2HS_R_001_002_ReqAllServerMail make_001_002_ReqAllServerMail(long _maxMailDbId, int _usTypeId)
    {
        NP2HS_R_001_002_ReqAllServerMail proto = new NP2HS_R_001_002_ReqAllServerMail();
        proto.setUsTypeId(_usTypeId);
        proto.setMaxMailDbId(_maxMailDbId);
        return proto;
    }

    public static NP2HS_R_001_004_ReqCheckIsInWhiteList make_001_004_ReqCheckIsInWhiteList(String _accName)
    {
        NP2HS_R_001_004_ReqCheckIsInWhiteList proto = new NP2HS_R_001_004_ReqCheckIsInWhiteList();
        proto.setAccName(_accName);
        return proto;
    }

    public static NP2HS_R_001_005_ReqPlatformInfo make_001_005_ReqPlatformInfo()
    {
        NP2HS_R_001_005_ReqPlatformInfo proto = new NP2HS_R_001_005_ReqPlatformInfo();
        return proto;
    }
    
    public static NP2HS_R_001_006_ReqPlatParamList make_001_006_ReqPlatParamList()
    {
    	NP2HS_R_001_006_ReqPlatParamList proto = new NP2HS_R_001_006_ReqPlatParamList();
        return proto;
    }
    
    public static NP2HS_R_001_010_ReqPushActivityScheduleResult make_010_ReqPushActivityScheduleResultSucc(long _phpOpSerial)
    {
    	NP2HS_R_001_010_ReqPushActivityScheduleResult proto = new NP2HS_R_001_010_ReqPushActivityScheduleResult();
    	proto.setPhpOpSerial(_phpOpSerial);
        return proto;
    }
    public static NP2HS_R_001_010_ReqPushActivityScheduleResult make_010_ReqPushActivityScheduleResultFail(long _phpOpSerial, int _errCode, String _errMsg)
    {
    	NP2HS_R_001_010_ReqPushActivityScheduleResult proto = new NP2HS_R_001_010_ReqPushActivityScheduleResult();
    	proto.setPhpOpSerial(_phpOpSerial);
    	proto.setErrCode(_errCode);
    	proto.setErrMsg(_errMsg);
        return proto;
    }
}
