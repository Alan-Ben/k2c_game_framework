package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_020_ReqTakeOfflineReward;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardTakeResult;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import USLOGDB.OptBo.Opt007020OfflineRewardTakeBO;

public class MsgDealer_GC2GS_007_020_ReqTakeOfflineReward extends NPUserMsgDealer<GC2GS_007_020_ReqTakeOfflineReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_020_ReqTakeOfflineReward _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.OFFLINE_REWARD_TAKE);
        OfflineRewardTakeResult result = _commiter.getUserData().getOfflineRewardComponent().takeReward(_msg.getId(), context);
        if (!result.getResult().isSucc())
        {
            _commiter.commitFailRes(result.getResult().getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_007_CommOp.make_020_RetTakeOfflineReward(result.getExtData()));

        //操作日志
        Opt007020OfflineRewardTakeBO optBo = new Opt007020OfflineRewardTakeBO();
        optBo.setInstanceId(getUSServer().getBM(), _msg.getId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
