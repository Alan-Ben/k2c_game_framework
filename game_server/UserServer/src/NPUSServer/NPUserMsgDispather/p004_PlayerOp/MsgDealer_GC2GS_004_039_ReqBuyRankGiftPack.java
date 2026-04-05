package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_039_ReqBuyRankGiftPack;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/**
 * 购买冲榜礼包协议处理器
 */
public class MsgDealer_GC2GS_004_039_ReqBuyRankGiftPack extends NPUserMsgDealer<GC2GS_004_039_ReqBuyRankGiftPack> {

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_004_039_ReqBuyRankGiftPack _msg) {
        NPUSUserData userData = _committer.getUserData();
        if (null == userData)
            return;

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RANK_GIFT_PACK_BUY);

        // 调用组件购买方法
        Result result = userData.getRankGiftPackComponent().buyGiftPack(_msg.getDbId(), context);
        if (!result.isSucc()) {
            _committer.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_004_PlayerOp.make_039_RetBuyRankGiftPack());
    }
}
