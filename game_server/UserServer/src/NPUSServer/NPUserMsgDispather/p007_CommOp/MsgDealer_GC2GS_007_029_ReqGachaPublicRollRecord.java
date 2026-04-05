package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_029_ReqGachaPublicRollRecord;
import NPUSServer.GachaPublicRecord.GachaPublicPoolRecord;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_029_ReqGachaPublicRollRecord extends NPUserMsgDealer<GC2GS_007_029_ReqGachaPublicRollRecord>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_007_029_ReqGachaPublicRollRecord _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData ==null)
            return;

        GachaPublicPoolRecord gachaPublicPoolRecord = getUSServer().getGachaPublicRecordMgr().lookupPoolRecord(_msg.getPoolId());
        if (gachaPublicPoolRecord == null)
        {
            _committer.commitSucRes(US2GCWriter_007_CommOp.make_029_RetGachaPublicRollRecord(null));
        }else
        {
            _committer.commitSucRes(US2GCWriter_007_CommOp.make_029_RetGachaPublicRollRecord(gachaPublicPoolRecord.makeProtoList(_msg.getDbId())));
        }
    }
}
