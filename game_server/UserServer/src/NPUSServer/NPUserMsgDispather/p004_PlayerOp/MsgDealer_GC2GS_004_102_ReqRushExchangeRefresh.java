package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_102_ReqRushExchangeRefresh;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/**
 * 请求刷新急速兑换礼包消息处理器
 */
public class MsgDealer_GC2GS_004_102_ReqRushExchangeRefresh extends NPUserMsgDealer<GC2GS_004_102_ReqRushExchangeRefresh> {

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_004_102_ReqRushExchangeRefresh _msg) {
        // 获取用户对象
        NPUSUserData userData = _committer.getUserData();
        if (null == userData) {
            return;
        }

        // 调用组件方法
        Result result = userData.getRushExchangeComponent().tryRefresh();

        // 处理结果
        if (!result.isSucc()) {
            _committer.commitFailRes(result.getCode());
            return;
        }

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_004_PlayerOp.make_102_RetRushExchangeRefresh());
    }
}
