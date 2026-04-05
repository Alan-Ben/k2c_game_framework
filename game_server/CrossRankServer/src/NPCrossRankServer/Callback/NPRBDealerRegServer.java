package NPCrossRankServer.Callback;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_009_RetCRSRegInfo;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

public class NPRBDealerRegServer implements _IWCGCallbackDealer
{

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_009_RetCRSRegInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        ALServerLog.Error("Reg Cross Rank Server Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        NP2PS_RB_001_009_RetCRSRegInfo protocol = (NP2PS_RB_001_009_RetCRSRegInfo) _msg;
        if (null == protocol)
            return;
    }

}
