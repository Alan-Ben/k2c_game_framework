package NPServerProtocolWriter.NP2PS.Request;

import NP2PS_R.p001_BasicOp.*;

public class NP2PS_R_Writer_001_BasicOp
{
    public static NP2PS_R_001_001_RegCS make_001_RegServer()
    {
        NP2PS_R_001_001_RegCS protocol = new NP2PS_R_001_001_RegCS();

        return protocol;
    }

    public static NP2PS_R_001_002_RegGS make_002_RegGS(String _areaTag, int _userMaxCount, int _userHandleWeight, String _connectIp, int _port)
    {
        NP2PS_R_001_002_RegGS protocol = new NP2PS_R_001_002_RegGS();
        protocol.setAreaTag(_areaTag);
        protocol.setUserMaxCount(_userMaxCount);
        protocol.setUserHandleWeight(_userHandleWeight);
        protocol.setConnectIp(_connectIp);
        protocol.setConnectPort(_port);

        return protocol;
    }

    public static NP2PS_R_001_003_RegLCS make_003_RegLCS()
    {
        NP2PS_R_001_003_RegLCS protocol = new NP2PS_R_001_003_RegLCS();

        return protocol;
    }

    public static NP2PS_R_001_004_RegLSServer make_004_RegLSServer(String _areaTag)
    {
        NP2PS_R_001_004_RegLSServer protocol = new NP2PS_R_001_004_RegLSServer();
        protocol.setAreaTag(_areaTag);

        return protocol;
    }

    public static NP2PS_R_001_007_RegUS make_007_RegUS()
    {
        return new NP2PS_R_001_007_RegUS();
    }

    public static NP2PS_R_001_009_RegCRS make_009_RegCRS()
    {
        return new NP2PS_R_001_009_RegCRS();
    }

    public static NP2PS_R_001_010_RegCGS make_010_RegCGS(String _areaTag)
    {
        NP2PS_R_001_010_RegCGS protocol = new NP2PS_R_001_010_RegCGS();
        protocol.setAreaTag(_areaTag);

        return protocol;
    }
    
    public static NP2PS_R_001_013_RegCrossDataServer make_013_RegCrossDataServer()
    {
        return new NP2PS_R_001_013_RegCrossDataServer();
    }

    public static NP2PS_R_001_014_RegGameLogicServer make_014_RegGameLogicServer()
    {
        return new NP2PS_R_001_014_RegGameLogicServer();
    }
}
