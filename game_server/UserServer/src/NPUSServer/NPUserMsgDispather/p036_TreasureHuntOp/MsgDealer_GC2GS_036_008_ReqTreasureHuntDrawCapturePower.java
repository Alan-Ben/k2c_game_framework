package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import GC2GS.p036_TreasureHuntOp.GC2GS_036_008_ReqTreasureHuntDrawCapturePower;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

/**
 * 太空寻宝-领取捕捉能力 协议处理类
 */
public class MsgDealer_GC2GS_036_008_ReqTreasureHuntDrawCapturePower extends NPUserMsgDealer<GC2GS_036_008_ReqTreasureHuntDrawCapturePower>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_008_ReqTreasureHuntDrawCapturePower _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_DRAW_CAPTURE_POWER);
        Result result = userData.getTreasureHuntComponent().drawNormalCaptureItem(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_008_RetTreasureHuntDrawCapturePower());
    }
}
