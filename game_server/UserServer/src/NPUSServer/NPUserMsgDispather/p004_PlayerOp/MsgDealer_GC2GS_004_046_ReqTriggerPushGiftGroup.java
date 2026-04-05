package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_046_ReqTriggerPushGiftGroup;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_046_ReqTriggerPushGiftGroup extends NPUserMsgDealer<GC2GS_004_046_ReqTriggerPushGiftGroup>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_046_ReqTriggerPushGiftGroup _msg)
    {
        // 获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.NONE);

        // 调用组件方法触发礼包组
        Result result = userData.getPushGiftPackComponent().triggerGroup(_msg.getGroupId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        // 返回空响应
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_046_RetTriggerPushGiftGroup());
    }
}
