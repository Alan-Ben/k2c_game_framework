package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_088_ReqLoverCollectInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

/**
 * 情人收集初始化消息处理器
 */
public class MsgDealer_GC2GS_002_088_ReqLoverCollectInit extends NPUserMsgDealer<GC2GS_002_088_ReqLoverCollectInit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_002_088_ReqLoverCollectInit _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (null == userData)
            return;

        _committer.commitSucRes(
                US2GCWriter_002_InitOp.make_088_RetLoverCollectInit(
                        userData.getLoverCollectComponent().getTargetLoverId(),
                        userData.getLoverCollectComponent().isIsClaimed()));
    }
}
