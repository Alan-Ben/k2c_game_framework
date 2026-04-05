package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_007_ReqHeroTalentSkillUpgrade;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;
import USLOGDB.OptBo.Opt013007HeroUpgradeTalentSkillUseItemBO;

public class MsgDealer_GC2GS_013_007_ReqHeroTalentSkillUpgrade extends NPUserMsgDealer<GC2GS_013_007_ReqHeroTalentSkillUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_013_007_ReqHeroTalentSkillUpgrade _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (userData == null)
            return;

        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if (heroInfo == null)
        {
            _commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_TALENT_SKILL_UPGRADE);
        //升级逻辑
        Result upgradeResult = heroInfo.getTalentSkillMgr().upgrade(_msg.getTalentSkillId(), _msg.getIsTen(), context);
        if (!upgradeResult.isSucc())
        {
            _commiter.commitFailRes(upgradeResult.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_013_HeroOp.make_007_RetHeroTalentSkillUpgrade());
        
        //数据日志
        BM bmObj = userData.getUSServer().getBM();
        Opt013007HeroUpgradeTalentSkillUseItemBO optBo = new Opt013007HeroUpgradeTalentSkillUseItemBO();
        optBo.setHeroId(bmObj, _msg.getHeroId());
        optBo.setTalentSkillId(bmObj, _msg.getTalentSkillId());
        userData.logEvent(optBo, context);
    }
}
