package NPServerProtocolWriter.NP2CGS.RequestBack;


import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_002_RetCreateCrossGameInstance;
import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_003_RetDiscardCrossGameInstance;
import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_004_RetGM;

import java.nio.ByteBuffer;

public class NP2CGS_RB_Writer_001_BasicOp
{
    public static NP2CGS_RB_001_002_RetCreateCrossGameInstance make_002_RetCreateCrossGameInstance(ByteBuffer _ext)
    {
        NP2CGS_RB_001_002_RetCreateCrossGameInstance protocol = new NP2CGS_RB_001_002_RetCreateCrossGameInstance();
        protocol.setRetMsg(_ext);

        return protocol;
    }

    public static NP2CGS_RB_001_003_RetDiscardCrossGameInstance make_003_RetDiscardCrossGameInstance()
    {
        NP2CGS_RB_001_003_RetDiscardCrossGameInstance protocol = new NP2CGS_RB_001_003_RetDiscardCrossGameInstance();

        return protocol;
    }

    public static NP2CGS_RB_001_004_RetGM make_004_RetGM(boolean _res, String _retMsg)
    {
        NP2CGS_RB_001_004_RetGM protocol = new NP2CGS_RB_001_004_RetGM();
        protocol.setRes(_res);
        protocol.setReturnMsg(_retMsg);

        return protocol;
    }
}
