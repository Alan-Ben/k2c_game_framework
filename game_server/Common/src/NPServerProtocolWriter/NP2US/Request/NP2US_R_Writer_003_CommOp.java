package NPServerProtocolWriter.NP2US.Request;

import WCGCS2US_R.p003_CommOp.NP2US_R_003_012_ReqBanCid;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_013_ReqUnBanCid;

public class NP2US_R_Writer_003_CommOp
{
    public static NP2US_R_003_012_ReqBanCid make_012_ReqBanCid(long _cid, long _freezeTime)
    {
        NP2US_R_003_012_ReqBanCid protocol = new NP2US_R_003_012_ReqBanCid();

        protocol.setCid(_cid);
        protocol.setFreezeTimeMs(_freezeTime);

        return protocol;
    }

    public static NP2US_R_003_013_ReqUnBanCid make_013_ReqUnBanCid(long _cid)
    {
        NP2US_R_003_013_ReqUnBanCid protocol = new NP2US_R_003_013_ReqUnBanCid();

        protocol.setCid(_cid);
        return protocol;
    }
}
