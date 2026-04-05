package GameLogicServer.Callback;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_014_RegGameLogicServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

public class RBDealerRegServer implements _IWCGCallbackDealer
{
    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_014_RegGameLogicServer();
    }

    @Override
    public void dealFail(int _errCode)
    {
        ALServerLog.Error("Reg Game Logic Server Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
    }
}
