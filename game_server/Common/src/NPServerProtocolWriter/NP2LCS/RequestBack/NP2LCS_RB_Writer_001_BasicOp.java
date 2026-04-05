package NPServerProtocolWriter.NP2LCS.RequestBack;

import NP2LCS_RB.p001_BasicOp.NP2LCS_RB_001_001_RetAccInfo;

public class NP2LCS_RB_Writer_001_BasicOp
{
    public static NP2LCS_RB_001_001_RetAccInfo make_001_RetAccInfoSuc(String _uid, String _chkKey)
    {
        NP2LCS_RB_001_001_RetAccInfo protocol = new NP2LCS_RB_001_001_RetAccInfo();

        protocol.setRes(true);
        protocol.setUid(_uid);
        protocol.setChkKey(_chkKey);

        return protocol;
    }

    public static NP2LCS_RB_001_001_RetAccInfo make_001_RetAccInfoFail()
    {
        NP2LCS_RB_001_001_RetAccInfo protocol = new NP2LCS_RB_001_001_RetAccInfo();

        protocol.setRes(false);

        return protocol;
    }
}
