package NPUSServer.ServerCallback;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_007_RetUSRegInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

public class NPUS_RBDealerRegUSServer implements _IWCGCallbackDealer
{
    private NPUserServer _m_server;

    public NPUS_RBDealerRegUSServer(NPUserServer _server)
    {
        _m_server = _server;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_007_RetUSRegInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        USLog.error(_m_server, "Reg User Server Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        NP2PS_RB_001_007_RetUSRegInfo protocol = (NP2PS_RB_001_007_RetUSRegInfo) _msg;
        if (null == protocol)
            return;

    }
}
