package NPServerProtocolWriter.NP2US.Msg;


import NP2US_B.p001_BasicOp.*;

import java.util.ArrayList;

public class NP2US_B_Writer_001_BasicOp
{
    public static NP2US_B_001_001_CSOnline make_001_CSOnline()
    {
        return new NP2US_B_001_001_CSOnline();
    }

    public static NP2US_B_001_002_PlayerFreeze make_002_PlayerFreeze(String _uid)
    {

        return new NP2US_B_001_002_PlayerFreeze(_uid);
    }

    public static NP2US_B_001_003_AllServerMailUpdate make_003_AllServerMailUpdate(boolean _isAllServer, long _allServerMailId,
                                                                                   ArrayList<Integer> _serverIdList)
    {
        NP2US_B_001_003_AllServerMailUpdate proto = new NP2US_B_001_003_AllServerMailUpdate();
        proto.setIsAllServer(_isAllServer);
        proto.setAllServerMailId(_allServerMailId);
        proto.getServerList().addAll(_serverIdList);
        return proto;
    }

    public static NP2US_B_001_004_GSOnline make_004_GSOnline(int _gsId)
    {
        return new NP2US_B_001_004_GSOnline(_gsId);
    }

    public static NP2US_B_001_005_PayOnline make_005_PayOnline()
    {
        return new NP2US_B_001_005_PayOnline();
    }

    public static NP2US_B_001_006_USInfoListInit make_006_USInfoListInit()
    {
        return new NP2US_B_001_006_USInfoListInit();
    }
}
