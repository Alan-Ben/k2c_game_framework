package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_084_ReqRankGiftPackInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

public class MsgDealer_GC2GS_002_084_ReqRankGiftPackInit extends NPUserMsgDealer<GC2GS_002_084_ReqRankGiftPackInit> {

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_002_084_ReqRankGiftPackInit _msg) {
        // 获取用户对象
        NPUSUserData userData = _committer.getUserData();
        if (null == userData) {
            return;
        }

        _committer.commitSucRes(
                US2GCWriter_002_InitOp.make_084_RetRankGiftPackInit(userData.getRankGiftPackComponent().toProto()));
    }
}
