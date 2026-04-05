package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

import GC2GS.p036_TreasureHuntOp.GC2GS_036_003_ReqTreasureHuntOreSkillUpgrade;
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
 * 太空寻宝-矿石技能升级 协议处理类
 */
public class MsgDealer_GC2GS_036_003_ReqTreasureHuntOreSkillUpgrade extends NPUserMsgDealer<GC2GS_036_003_ReqTreasureHuntOreSkillUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_036_003_ReqTreasureHuntOreSkillUpgrade _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_ORE_SKILL_UPGRADE);

        Result result = oreInfo.upgradeSkill(!_msg.getIsNormal(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp.make_003_RetTreasureHuntOreSkillUpgrade());
    }
}
