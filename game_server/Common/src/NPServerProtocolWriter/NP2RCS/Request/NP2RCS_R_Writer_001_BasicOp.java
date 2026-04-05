package NPServerProtocolWriter.NP2RCS.Request;

import NP2LCS_R.p001_BasicOp.NP2RCS_R_001_001_ReqUpdateLoginRecord;
import NP2LCS_R.p001_BasicOp.NP2RCS_R_001_002_ReqLookupLoginRecord;

/**
 * @description:
 * @author: ricci
 * @date: 2022-06-27 19:29:58
 */
public class NP2RCS_R_Writer_001_BasicOp
{
    public static NP2RCS_R_001_001_ReqUpdateLoginRecord make_001_001_ReqUpdateLoginRecord(long _cid, String _accountId, int _serverLogicId, int _serverTypeId)
    {
        NP2RCS_R_001_001_ReqUpdateLoginRecord proto = new NP2RCS_R_001_001_ReqUpdateLoginRecord();
        proto.setCid(_cid);
        proto.setAccountId(_accountId);
        proto.setServerLogicId(_serverLogicId);
        proto.setServerTypeId(_serverTypeId);
        return proto;
    }

    public static NP2RCS_R_001_002_ReqLookupLoginRecord make_001_002_ReqLookupLoginRecord(String _accountId)
    {
        NP2RCS_R_001_002_ReqLookupLoginRecord proto = new NP2RCS_R_001_002_ReqLookupLoginRecord();
        proto.setAccountId(_accountId);
        return proto;
    }
}
