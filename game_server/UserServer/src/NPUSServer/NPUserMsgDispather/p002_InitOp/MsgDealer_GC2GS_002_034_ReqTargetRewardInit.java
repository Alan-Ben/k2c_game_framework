package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_034_ReqTargetRewardInit;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;


public class MsgDealer_GC2GS_002_034_ReqTargetRewardInit extends NPUserMsgDealer<GC2GS_002_034_ReqTargetRewardInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_034_ReqTargetRewardInit _msg)
    {
        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_034_RetTargetRewardInit(_commiter.getUserData()));
    }
}
