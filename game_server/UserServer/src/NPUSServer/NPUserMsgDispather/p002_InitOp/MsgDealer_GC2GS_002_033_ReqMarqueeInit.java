package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_033_ReqMarqueeInit;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;


public class MsgDealer_GC2GS_002_033_ReqMarqueeInit extends NPUserMsgDealer<GC2GS_002_033_ReqMarqueeInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_033_ReqMarqueeInit _msg)
    {
        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_033_RetMarqueeInit(_commiter.getUserData(), _msg.getMarqueeReadList(), _commiter.getUserData().getOnlineTimeMs()));
    }
}
