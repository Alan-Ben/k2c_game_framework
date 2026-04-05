package NPCrossGameServer.CallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_010_RetCGSRegInfo;
import NPCrossGameServer.NPCrossGameServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

public class NPRBDealerRegServer implements _IWCGCallbackDealer
{

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_010_RetCGSRegInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        ALServerLog.Error("Reg Cross-Game Server Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        NP2PS_RB_001_010_RetCGSRegInfo regInfo = (NP2PS_RB_001_010_RetCGSRegInfo) _msg;
        ALServerLog.Info("Reg Cross-Game Server Suc!");
        NPCrossGameServer.getInstance().setAreaIndex(regInfo.getAreaIndex());
    }
}
