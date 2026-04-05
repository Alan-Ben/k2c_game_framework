package NPServerProtocolWriter.NP2PS.Msg;

import NP2PS.p002_GSOp.NP2PS_002_001_ReduceHandleUser;
import NP2PS.p002_GSOp.NP2PS_002_002_AddHandleUser;

public class NP2PS_Writer_002_GSOp
{
    public static NP2PS_002_001_ReduceHandleUser make_001_ReduceHandleUser(String _uid)
    {
        NP2PS_002_001_ReduceHandleUser protocol = new NP2PS_002_001_ReduceHandleUser();

        protocol.setUid(_uid);

        return protocol;
    }

    public static NP2PS_002_002_AddHandleUser make_002_AddHandleUser(String _uid)
    {
        NP2PS_002_002_AddHandleUser protocol = new NP2PS_002_002_AddHandleUser();

        protocol.setUid(_uid);

        return protocol;
    }
}
