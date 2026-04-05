package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_105_ReqClaimLover;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/**
 * 情人收集-领取情人消息处理器
 */
public class MsgDealer_GC2GS_004_105_ReqClaimLover extends NPUserMsgDealer<GC2GS_004_105_ReqClaimLover>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_004_105_ReqClaimLover _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.LOVER_COLLECT_CLAIM);

        Result result = userData.getLoverCollectComponent().claimLover(context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_004_PlayerOp.make_105_RetClaimLover());
    }
}
