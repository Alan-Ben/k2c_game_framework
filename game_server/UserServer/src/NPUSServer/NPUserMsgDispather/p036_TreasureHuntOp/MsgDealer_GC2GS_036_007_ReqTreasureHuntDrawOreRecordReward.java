package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import GC2GS.p036_TreasureHuntOp.GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Ore.TreasureHuntOreInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

/**
 * 太空寻宝-领取矿石记录奖励 协议处理类
 */
public class MsgDealer_GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward extends NPUserMsgDealer<GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_007_ReqTreasureHuntDrawOreRecordReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        TreasureHuntOreInfo oreInfo = userData.getTreasureHuntComponent().getOreMgr().lookupOre(_msg.getOreId());
        if (oreInfo == null)
        {
            _commiter.commitFailRes(TreasureHuntErr.TREASURE_HUNT_ORE_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_DRAW_ORE_RECORD_REWARD);

        Result result = oreInfo.drawRecordReward(_msg.getRecordIndex(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_007_RetTreasureHuntDrawOreRecordReward());
    }
}
