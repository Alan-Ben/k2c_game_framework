package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_022_ReqEquipSkillRebuild;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.EquipComp.EquipInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;

public class MsgDealer_GC2GS_013_022_ReqEquipSkillRebuild extends NPUserMsgDealer<GC2GS_013_022_ReqEquipSkillRebuild>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_013_022_ReqEquipSkillRebuild _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData == null)
            return;

        EquipInfo equipInfo = userData.getEquipComponent().lookupEquipByDbId(_msg.getDbId());
        if (equipInfo == null)
        {
            _committer.commitFailRes(HeroErr.EQUIP_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.EQUIP_SKILL_RESHAPE);

        Result result = equipInfo.reshapeSkill(_msg.getIndex(), _msg.getIsAdvance(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_013_HeroOp.make_022_RetEquipSkillRebuild());
    }
}