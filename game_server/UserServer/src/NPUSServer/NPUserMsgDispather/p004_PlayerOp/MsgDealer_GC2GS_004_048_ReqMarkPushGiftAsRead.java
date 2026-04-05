package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_048_ReqMarkPushGiftAsRead;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_048_ReqMarkPushGiftAsRead extends NPUserMsgDealer<GC2GS_004_048_ReqMarkPushGiftAsRead>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_048_ReqMarkPushGiftAsRead _msg)
    {
        // 获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 调用组件方法标记已读
        Result result = userData.getPushGiftPackComponent().markAsRead(_msg.getGroupId(), _msg.getPushGiftId());
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        // 返回空响应
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_048_RetMarkPushGiftAsRead());
    }
}
