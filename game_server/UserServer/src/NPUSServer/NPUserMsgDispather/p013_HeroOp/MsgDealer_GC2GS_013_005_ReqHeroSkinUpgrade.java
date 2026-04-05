package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_005_ReqHeroSkinUpgrade;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Hero.RefHeroSkin;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;
import USLOGDB.OptBo.Opt013005HeroUpgradeSkinBO;

public class MsgDealer_GC2GS_013_005_ReqHeroSkinUpgrade extends NPUserMsgDealer<GC2GS_013_005_ReqHeroSkinUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_013_005_ReqHeroSkinUpgrade _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (userData == null)
            return;

        RefHeroSkin refHeroSkin = RefHeroSkin.getMgr().get(_msg.getSkinId());
        if (refHeroSkin == null)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(refHeroSkin.hero_id);
        if (heroInfo == null)
        {
            _commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_SKIN_UPGRADE);
        //升级逻辑
        Result upgradeResult = heroInfo.getSkinMgr().upgrade(_msg.getSkinId(), context);
        if (!upgradeResult.isSucc())
        {
            _commiter.commitFailRes(upgradeResult.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_013_HeroOp.make_005_RetHeroSkinUpgrade());
        
        //数据日志
        BM bmObj = userData.getUSServer().getBM();
        Opt013005HeroUpgradeSkinBO optBo = new Opt013005HeroUpgradeSkinBO();
        optBo.setHeroId(bmObj, heroInfo.getHeroId());
        optBo.setSkinId(bmObj, _msg.getSkinId());
        userData.logEvent(optBo, context);
    }
}
