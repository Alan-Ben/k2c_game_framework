package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_027_ReqEquipSkillList;
import NPCommon.ErrMain.HeroErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.EquipComp.EquipInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;

public class MsgDealer_GC2GS_013_027_ReqEquipSkillList extends NPUserMsgDealer<GC2GS_013_027_ReqEquipSkillList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_013_027_ReqEquipSkillList _msg)
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

        _committer.commitSucRes(US2GCWriter_013_HeroOp.make_027_RetEquipSkillList(equipInfo));
    }
}