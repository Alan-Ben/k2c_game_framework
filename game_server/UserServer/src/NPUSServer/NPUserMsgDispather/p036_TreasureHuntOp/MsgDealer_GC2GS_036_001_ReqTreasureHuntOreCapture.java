package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import GC2GS.p036_TreasureHuntOp.GC2GS_036_001_ReqTreasureHuntOreCapture;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntCaptureResult;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

public class MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture extends NPUserMsgDealer<GC2GS_036_001_ReqTreasureHuntOreCapture>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_001_ReqTreasureHuntOreCapture _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_CAPTURE);

        ResultOne<TreasureHuntCaptureResult> captureResult = userData.getTreasureHuntComponent()
                .capture(_msg.getType(), _msg.getIsAdvance(), _msg.getDistance(), _msg.getAreaId(), context, null);
        if (!captureResult.isSucc())
        {
            _commiter.commitFailRes(captureResult.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_001_RetTreasureHuntOreCapture(captureResult.getData().exp, captureResult.getData().captureResultList));
    }
}
