package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_081_ReqPushGiftPackList;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

/**
 * 请求推送礼包列表消息处理器（002_081协议）
 *
 * 功能：
 * 玩家登录时请求推送礼包列表，返回所有礼包组的状态
 */
public class MsgDealer_GC2GS_002_081_ReqPushGiftPackList extends NPUserMsgDealer<GC2GS_002_081_ReqPushGiftPackList> {
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_081_ReqPushGiftPackList _msg) {
        // 获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_081_RetPushGiftPackList(userData));
    }
}
