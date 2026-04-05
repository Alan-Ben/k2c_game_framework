package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_001_ReqHeroUpgrade;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;
import USLOGDB.OptBo.Opt013001HeroUpgradeLvlBO;

public class MsgDealer_GC2GS_013_001_ReqHeroUpgrade extends NPUserMsgDealer<GC2GS_013_001_ReqHeroUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_013_001_ReqHeroUpgrade _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (userData == null)
            return;

        //判断是否解锁一键升级十级
        if (_msg.getIsTen() && !NPPlayerConditionDealerMgr.IsEnable(RefGeneral.Ref().hero_level_up_ten_times_condition, userData, null))
        {
            _commiter.commitFailRes(HeroErr.HERO_A_KEY_UPGRADE_FUNC_NOT_UNLOCK.getCode());
            return;
        }

        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if (heroInfo == null)
        {
            _commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_UPGRADE);
        //升级逻辑
        Result upgradeResult = heroInfo.upgrade(_msg.getIsTen(), context);
        if (!upgradeResult.isSucc())
        {
            _commiter.commitFailRes(upgradeResult.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_013_HeroOp.make_001_RetHeroUpgrade());

        //日志数据
        BM bmObj = userData.getUSServer().getBM();
        Opt013001HeroUpgradeLvlBO optBo = new Opt013001HeroUpgradeLvlBO();
        optBo.setHeroId(bmObj, _msg.getHeroId());
        optBo.setIsTen(bmObj, _msg.getIsTen());
        optBo.setCurLvl(bmObj, heroInfo.getLevel());
        userData.logEvent(optBo, context);
    }
}
