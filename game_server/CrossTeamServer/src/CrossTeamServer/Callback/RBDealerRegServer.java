package CrossTeamServer.Callback;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_013_RegCrossDataServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

public class RBDealerRegServer implements _IWCGCallbackDealer
{
    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_013_RegCrossDataServer();
    }

    @Override
    public void dealFail(int _errCode)
    {
        ALServerLog.Error("Reg Cross Team Server Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
    }
}
