package NPServerProtocolWriter.NP2PS.Request;

import NP2PS_R.p100_SysOp.NP2PS_R_100_001_GetAreaSerial;

public class NP2PS_R_Writer_100_SysOp
{
    public static NP2PS_R_100_001_GetAreaSerial make_001_GetAreaSerial(String _areaTag)
    {
        NP2PS_R_100_001_GetAreaSerial protocol = new NP2PS_R_100_001_GetAreaSerial();

        protocol.setAreaTag(_areaTag);

        return protocol;
    }
}
