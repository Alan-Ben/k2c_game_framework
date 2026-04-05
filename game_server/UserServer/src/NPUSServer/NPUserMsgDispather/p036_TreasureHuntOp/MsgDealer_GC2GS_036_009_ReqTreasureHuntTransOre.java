package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import Common.TreasureHuntObj.TreasureHunt_TransOreResult;
import GC2GS.p036_TreasureHuntOp.GC2GS_036_009_ReqTreasureHuntTransOre;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

import java.util.List;

/**
 * 太空寻宝-转化矿石 协议处理类
 */
public class MsgDealer_GC2GS_036_009_ReqTreasureHuntTransOre extends NPUserMsgDealer<GC2GS_036_009_ReqTreasureHuntTransOre>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_009_ReqTreasureHuntTransOre _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_TRANS_ORE);

        ResultOne<List<TreasureHunt_TransOreResult>> result = userData.getTreasureHuntComponent().getOreMgr().transPendingOre(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_009_RetTreasureHuntTransOre(result.getData()));
    }
}
