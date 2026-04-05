package NPUSServer.NPGeneralListener.Writer;

import NPEnum.ENPBoxChatStatus;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_008_RetGetBoxInfo;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_009_RetAddBoxGainedCid;

import java.util.ArrayList;

public class NP2US_RB_Writer_003_CommonOp
{
    public static NP2US_RB_003_008_RetGetBoxInfo make_008_RetGetBoxInfo(long _boxId, ENPBoxChatStatus _boxStatus, ArrayList<Long> _gainedCidList)
    {
        NP2US_RB_003_008_RetGetBoxInfo protocol = new NP2US_RB_003_008_RetGetBoxInfo();
        protocol.setBoxId(_boxId);
        protocol.setBoxStatus(_boxStatus);
        protocol.getGainedCidList().addAll(_gainedCidList);

        return protocol;
    }

    public static NP2US_RB_003_009_RetAddBoxGainedCid make_009_RetAddBoxGainedCid(boolean _res
            , long _boxId, ENPBoxChatStatus _boxStatus, ArrayList<Long> _gainedCidList)
    {
        NP2US_RB_003_009_RetAddBoxGainedCid protocol = new NP2US_RB_003_009_RetAddBoxGainedCid();
        protocol.setRes(_res);
        protocol.setBoxId(_boxId);
        protocol.setBoxStatus(_boxStatus);
        protocol.getGainedCidList().addAll(_gainedCidList);

        return protocol;
    }
}
