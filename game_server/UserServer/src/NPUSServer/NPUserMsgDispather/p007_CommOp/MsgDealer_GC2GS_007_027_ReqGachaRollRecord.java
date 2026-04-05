package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_027_ReqGachaRollRecord;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Record.GachaPoolRecord;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_027_ReqGachaRollRecord extends NPUserMsgDealer<GC2GS_007_027_ReqGachaRollRecord>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_007_027_ReqGachaRollRecord _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData ==null)
            return;

        GachaPoolRecord poolRecord = userData.getGachaComponent().lookupPoolRecord(_msg.getPoolId());
        if (poolRecord != null)
        {
            _committer.commitSucRes(US2GCWriter_007_CommOp.make_027_RetGachaRollRecord(poolRecord.makeProtoList()));
        }else
        {
            _committer.commitSucRes(US2GCWriter_007_CommOp.make_027_RetGachaRollRecord(null));
        }
    }
}
