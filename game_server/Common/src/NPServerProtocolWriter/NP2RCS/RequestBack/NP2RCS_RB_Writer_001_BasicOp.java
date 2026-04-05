package NPServerProtocolWriter.NP2RCS.RequestBack;


import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import NP2LCS_RB.p001_BasicOp.NP2RCS_RB_001_001_RetUpdateLoginRecord;
import NP2LCS_RB.p001_BasicOp.NP2RCS_RB_001_002_RetLookupLoginRecord;

import java.util.List;

public class NP2RCS_RB_Writer_001_BasicOp
{
    public static NP2RCS_RB_001_001_RetUpdateLoginRecord make_001_001_RetUpdateLoginRecord()
    {
        return new NP2RCS_RB_001_001_RetUpdateLoginRecord();
    }

    public static NP2RCS_RB_001_002_RetLookupLoginRecord make_001_002_ReqLookupLoginRecord(List<NP_SYS_PlayerJoinedUSInfo> _joinedUSList)
    {
        NP2RCS_RB_001_002_RetLookupLoginRecord proto = new NP2RCS_RB_001_002_RetLookupLoginRecord();
        proto.getJoinedUSList().addAll(_joinedUSList);
        return proto;
    }
}
