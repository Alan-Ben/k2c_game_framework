package NPPlatServer.NPGeneralListener.Writer;

import NP2PS_RB.p100_SysOp.NP2PS_RB_100_001_RetAreaSerial;

public class NP2PS_RB_Writer_100_SysOp
{
    public static NP2PS_RB_100_001_RetAreaSerial make_001_RetAreaSerialSuc(long _areaSerial)
    {
        NP2PS_RB_100_001_RetAreaSerial protocol = new NP2PS_RB_100_001_RetAreaSerial();

        protocol.setIsFound(true);
        protocol.setAreaSerial(_areaSerial);

        return protocol;
    }

    public static NP2PS_RB_100_001_RetAreaSerial make_001_RetAreaSerialFail()
    {
        NP2PS_RB_100_001_RetAreaSerial protocol = new NP2PS_RB_100_001_RetAreaSerial();

        protocol.setIsFound(false);
        protocol.setAreaSerial(-1);

        return protocol;
    }
}
