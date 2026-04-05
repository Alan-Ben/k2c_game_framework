package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_019_ReqChapterInit;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

public class MsgDealer_GC2GS_002_019_ReqChapterInit extends NPUserMsgDealer<GC2GS_002_019_ReqChapterInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_002_019_ReqChapterInit _msg)
    {
        _committer.commitSucRes(US2GCWriter_002_InitOp.make_019_RetChapterInit(_committer.getUserData()));
    }
}
