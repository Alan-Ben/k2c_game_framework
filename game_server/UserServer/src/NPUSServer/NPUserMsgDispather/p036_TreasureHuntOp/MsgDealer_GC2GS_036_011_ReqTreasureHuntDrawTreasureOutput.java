package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import GC2GS.p036_TreasureHuntOp.GC2GS_036_011_ReqTreasureHuntDrawTreasureOutput;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_TREASURE_HUNT_DRAW_DAILY_GEM;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure.TreasureHuntTreasureOutputInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

/**
 * 太空寻宝-领取奇物产出 协议处理类
 */
public class MsgDealer_GC2GS_036_011_ReqTreasureHuntDrawTreasureOutput extends NPUserMsgDealer<GC2GS_036_011_ReqTreasureHuntDrawTreasureOutput>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_011_ReqTreasureHuntDrawTreasureOutput _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_DRAW_DAILY_GEM);

        TreasureHuntTreasureOutputInfo outputInfo = userData.getTreasureHuntComponent().getTreasureMgr().lookupTreasureOutput(_msg.getTreasureId());
        if (outputInfo == null)
        {
            _commiter.commitFailRes(TreasureHuntErr.TREASURE_HUNT_TREASURE_NO_OUTPUT.getCode());
            return;
        }

        Result result = outputInfo.draw(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_011_RetTreasureHuntDrawTreasureOutput());

        userData.onLogicEvent(new Event_P_TREASURE_HUNT_DRAW_DAILY_GEM(context));
    }
}
