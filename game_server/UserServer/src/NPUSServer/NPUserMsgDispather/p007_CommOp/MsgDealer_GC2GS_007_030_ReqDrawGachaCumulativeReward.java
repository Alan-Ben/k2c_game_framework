package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_030_ReqDrawGachaCumulativeReward;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.AvatarGacha.RefGachaPool;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.GachaPoolInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_030_ReqDrawGachaCumulativeReward extends NPUserMsgDealer<GC2GS_007_030_ReqDrawGachaCumulativeReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_007_030_ReqDrawGachaCumulativeReward _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData ==null)
            return;

        RefGachaPool refGachaPool = RefGachaPool.getMgr().get(_msg.getPoolId());
        if (refGachaPool == null)
        {
            _committer.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        GachaPoolInfo poolInfo = userData.getGachaComponent().ensurePool(refGachaPool);
        if (poolInfo == null)
        {
            _committer.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RECRUIT);

        Result result = poolInfo.drawCumulativeReward(context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _committer.commitSucRes(US2GCWriter_007_CommOp.make_030_RetDrawGachaCumulativeReward());
    }
}
