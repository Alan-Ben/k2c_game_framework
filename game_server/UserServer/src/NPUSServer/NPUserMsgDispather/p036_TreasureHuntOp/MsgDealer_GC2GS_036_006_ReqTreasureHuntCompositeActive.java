package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import GC2GS.p036_TreasureHuntOp.GC2GS_036_006_ReqTreasureHuntCompositeActive;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.TreasureHuntCompositeInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

/**
 * 太空寻宝-合成激活 协议处理类
 */
public class MsgDealer_GC2GS_036_006_ReqTreasureHuntCompositeActive extends NPUserMsgDealer<GC2GS_036_006_ReqTreasureHuntCompositeActive>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_006_ReqTreasureHuntCompositeActive _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        TreasureHuntCompositeInfo compositeInfo = userData.getTreasureHuntComponent().getCompositeMgr().lookupComposite(_msg.getCompositeId());
        if (compositeInfo == null)
        {
            _commiter.commitFailRes(TreasureHuntErr.TREASURE_HUNT_COMPOSITE_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_COMPOSITE_SKILL_ACTIVE);

        Result result = compositeInfo.activeSkill(!_msg.getIsNormal(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_006_RetTreasureHuntCompositeActive());
    }
}
