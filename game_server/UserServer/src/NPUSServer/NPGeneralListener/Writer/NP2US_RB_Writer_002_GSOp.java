package NPUSServer.NPGeneralListener.Writer;

import NP2US_RB.p002_GSOp.*;
import NPUSServer.NPUserServer;

public class NP2US_RB_Writer_002_GSOp
{
    public static NP2US_RB_002_001_RegUserGate make_001_RegUserGateSuc(long _newSerialize)
    {
        NP2US_RB_002_001_RegUserGate protocol = new NP2US_RB_002_001_RegUserGate();

        protocol.setRes(true);
        protocol.setNewSerialize(_newSerialize);

        return protocol;
    }

    public static NP2US_RB_002_001_RegUserGate make_001_RegUserGateFail()
    {
        NP2US_RB_002_001_RegUserGate protocol = new NP2US_RB_002_001_RegUserGate();

        protocol.setRes(false);

        return protocol;
    }

    public static NP2US_RB_002_002_UnregUserGate make_002_UnregUserGateSuc()
    {
        NP2US_RB_002_002_UnregUserGate protocol = new NP2US_RB_002_002_UnregUserGate();

        protocol.setRes(true);

        return protocol;
    }

    public static NP2US_RB_002_002_UnregUserGate make_002_UnregUserGateFail()
    {
        NP2US_RB_002_002_UnregUserGate protocol = new NP2US_RB_002_002_UnregUserGate();

        protocol.setRes(false);

        return protocol;
    }

    public static NP2US_RB_002_004_RetResumeUSInfo make_004_RetResumeUSInfo(long _cid)
    {
        NP2US_RB_002_004_RetResumeUSInfo protocol = new NP2US_RB_002_004_RetResumeUSInfo();
        protocol.setCid(_cid);
        return protocol;
    }

    public static NP2US_RB_002_005_RetEnterUSInfo make_005_RetEnterUSInfo(long _queueIndex)
    {
        NP2US_RB_002_005_RetEnterUSInfo protocol = new NP2US_RB_002_005_RetEnterUSInfo();
        protocol.setQueueIndex(_queueIndex);

        return protocol;
    }

    public static NP2US_RB_002_006_RetQueueInfo make_006_RetQueueInfo(NPUserServer _usServer)
    {
        NP2US_RB_002_006_RetQueueInfo protocol = new NP2US_RB_002_006_RetQueueInfo();

        protocol.setCurQueueIndex(_usServer.getQueueMgr().getFirstIndex());

        return protocol;
    }

    public static NP2US_RB_002_007_RetQuitQueueRes make_007_RetQuitQueueRes(int _err)
    {
        NP2US_RB_002_007_RetQuitQueueRes protocol = new NP2US_RB_002_007_RetQuitQueueRes();
        protocol.setErrCode(_err);
        return protocol;
    }
}
