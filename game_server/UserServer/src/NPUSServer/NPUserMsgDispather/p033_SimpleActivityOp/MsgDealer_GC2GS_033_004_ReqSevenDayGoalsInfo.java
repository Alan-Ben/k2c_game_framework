package NPUSServer.NPUserMsgDispather.p033_SimpleActivityOp;

import GC2GS.p033_SimpleActivityOp.GC2GS_033_004_ReqSevenDayGoalsInfo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_033_SimpleActivityOp;

public class MsgDealer_GC2GS_033_004_ReqSevenDayGoalsInfo extends NPUserMsgDealer<GC2GS_033_004_ReqSevenDayGoalsInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_033_004_ReqSevenDayGoalsInfo _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        _committer.commitSucRes(US2GCWriter_033_SimpleActivityOp.make_004_RetSevenDayGoalsInfo(userData));
    }
}
