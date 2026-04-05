package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import GC2GS.p036_TreasureHuntOp.GC2GS_036_004_ReqTreasureHuntTreasureSkillActive;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure.TreasureHuntTreasureInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;

/**
 * 太空寻宝-奇物技能激活 协议处理类
 */
public class MsgDealer_GC2GS_036_004_ReqTreasureHuntTreasureSkillActive extends NPUserMsgDealer<GC2GS_036_004_ReqTreasureHuntTreasureSkillActive>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_004_ReqTreasureHuntTreasureSkillActive _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        TreasureHuntTreasureInfo treasureInfo = userData.getTreasureHuntComponent().getTreasureMgr().lookupTreasure(_msg.getTreasureId());
        if (treasureInfo == null)
        {
            _commiter.commitFailRes(TreasureHuntErr.TREASURE_HUNT_TREASURE_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_TREASURE_SKILL_ACTIVE);

        Result result = treasureInfo.activeSkill(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_004_RetTreasureHuntTreasureSkillActive());
    }
}
