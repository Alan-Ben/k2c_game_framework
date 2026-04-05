package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_035_ReqCommonRefreshInit;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;


public class MsgDealer_GC2GS_002_035_ReqCommonRefreshInit extends NPUserMsgDealer<GC2GS_002_035_ReqCommonRefreshInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_035_ReqCommonRefreshInit _msg)
    {
        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_035_RetCommonRefreshInit(_commiter.getUserData()));
    }
}
