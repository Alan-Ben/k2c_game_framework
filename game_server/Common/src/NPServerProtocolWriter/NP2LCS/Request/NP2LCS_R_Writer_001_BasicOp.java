package NPServerProtocolWriter.NP2LCS.Request;

import NP2LCS_R.p001_BasicOp.*;

public class NP2LCS_R_Writer_001_BasicOp
{
    public static NP2LCS_R_001_001_ReqAccInfo make_001_ReqAccInfo(String _accName, String _accPass)
    {
        NP2LCS_R_001_001_ReqAccInfo protocol = new NP2LCS_R_001_001_ReqAccInfo();

        protocol.setAccountName(_accName);
        protocol.setAccountPass(_accPass);

        return protocol;
    }

    public static NP2LCS_R_001_002_ReqAccInfoByChkKey make_002_ReqAccInfoByChkKey(String _accName, String _chkKey)
    {
        NP2LCS_R_001_002_ReqAccInfoByChkKey protocol = new NP2LCS_R_001_002_ReqAccInfoByChkKey();

        protocol.setAccountName(_accName);
        protocol.setChkKey(_chkKey);

        return protocol;
    }

    public static NP2LCS_R_001_003_ReqCheatUidInfo make_003_ReqCheatUidInfo(String _userName, String _userPass)
    {
        NP2LCS_R_001_003_ReqCheatUidInfo protocol = new NP2LCS_R_001_003_ReqCheatUidInfo();

        protocol.setUserName(_userName);
        protocol.setAccountPass(_userPass);

        return protocol;
    }

    public static NP2LCS_R_001_004_ReqSDKCheck make_004_ReqSDKCheck(String _accName, String _accPass, String _clientIp)
    {
        NP2LCS_R_001_004_ReqSDKCheck protocol = new NP2LCS_R_001_004_ReqSDKCheck();

        protocol.setAccName(_accName);
        protocol.setToken(_accPass);
        protocol.setClientIp(_clientIp);

        return protocol;
    }

    public static NP2LCS_R_001_098_ReqSetNeedCheckAcc make_098_ReqSetNeedCheckAcc(boolean _needCheck)
    {
        NP2LCS_R_001_098_ReqSetNeedCheckAcc protocol = new NP2LCS_R_001_098_ReqSetNeedCheckAcc();

        protocol.setNeedCheck(_needCheck);

        return protocol;
    }


    public static NP2LCS_R_001_099_ReqUpdateAcc make_099_ReqUpdateAcc(String _userName, String _userPass)
    {
        NP2LCS_R_001_099_ReqUpdateAcc protocol = new NP2LCS_R_001_099_ReqUpdateAcc();

        protocol.setUserName(_userName);
        protocol.setPass(_userPass);

        return protocol;
    }
}
