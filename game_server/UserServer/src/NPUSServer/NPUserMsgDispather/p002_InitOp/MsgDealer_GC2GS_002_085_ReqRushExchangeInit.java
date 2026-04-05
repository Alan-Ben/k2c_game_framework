package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_085_ReqRushExchangeInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

/**
 * 急速兑换初始化消息处理器
 */
public class MsgDealer_GC2GS_002_085_ReqRushExchangeInit extends NPUserMsgDealer<GC2GS_002_085_ReqRushExchangeInit> {

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_002_085_ReqRushExchangeInit _msg) {
        // 获取用户对象
        NPUSUserData userData = _committer.getUserData();
        if (null == userData) {
            return;
        }

        _committer.commitSucRes(
                US2GCWriter_002_InitOp.make_085_RetRushExchangeInit(userData.getRushExchangeComponent().toProto()));
    }
}
