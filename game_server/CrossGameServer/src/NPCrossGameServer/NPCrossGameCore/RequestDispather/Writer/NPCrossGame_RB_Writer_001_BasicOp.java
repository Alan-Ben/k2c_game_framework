package NPCrossGameServer.NPCrossGameCore.RequestDispather.Writer;

import ALBasicProtocolPack._IALProtocolStructure;
import CrossGame_RB.p001_BasicOp.CrossGame_RB_001_001_RetGCForwardMsg;

public class NPCrossGame_RB_Writer_001_BasicOp
{
    public static CrossGame_RB_001_001_RetGCForwardMsg make_001_RetGCForwardMsg(_IALProtocolStructure _proto)
    {
        CrossGame_RB_001_001_RetGCForwardMsg proto = new CrossGame_RB_001_001_RetGCForwardMsg();
        proto.setMsg(_proto.makeFullPackage());

        return proto;
    }
}
