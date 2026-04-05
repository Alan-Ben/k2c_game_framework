package NPServerProtocolWriter.NP2PS.Msg;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2PS.p001_BasicOp.NP2PS_001_002_BroadcastLS;

import java.nio.ByteBuffer;

public class NP2PS_Writer_001_BasicOp
{
    public static NP2PS_001_002_BroadcastLS make_002_BroadcastLS(_IALProtocolStructure _protocol)
    {
        NP2PS_001_002_BroadcastLS protocol = new NP2PS_001_002_BroadcastLS();

        protocol.setMsg(_protocol.makeFullPackage());

        return protocol;
    }

    public static NP2PS_001_002_BroadcastLS make_002_BroadcastLS(ByteBuffer _msg)
    {
        NP2PS_001_002_BroadcastLS protocol = new NP2PS_001_002_BroadcastLS();

        protocol.setMsg(_msg);

        return protocol;
    }
}
