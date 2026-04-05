package NPServerProtocolWriter.NP2LS.Msg;

import NP2LS.p001_BasicOp.NP2LS_001_002_SetForbidenLogin;

public class NP2LS_Writer_001_BasicOp
{
    public static NP2LS_001_002_SetForbidenLogin make_002_SetForbidenLogin(boolean _isForbidenLogin)
    {
        NP2LS_001_002_SetForbidenLogin protocol = new NP2LS_001_002_SetForbidenLogin();
        protocol.setIsForbidenLogin(_isForbidenLogin);
        return protocol;
    }
}
