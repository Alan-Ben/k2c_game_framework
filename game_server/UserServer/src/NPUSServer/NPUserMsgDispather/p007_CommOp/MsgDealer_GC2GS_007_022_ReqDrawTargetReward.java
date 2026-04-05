package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_022_ReqDrawTargetReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_022_ReqDrawTargetReward extends NPUserMsgDealer<GC2GS_007_022_ReqDrawTargetReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_022_ReqDrawTargetReward _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.OFFLINE_REWARD_LIST_TAKE);

        Result result = _commiter.getUserData().getTargetRewardComponent().drawReward(_msg.getId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.getUserData().sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_022_RetDrawTargetReward());
    }
}
