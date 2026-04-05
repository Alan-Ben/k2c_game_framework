package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_002_ReqHeroSetSkin;
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
import USLOGDB.OptBo.Opt013002HeroSetSkinBO;

public class MsgDealer_GC2GS_013_002_ReqHeroSetSkin extends NPUserMsgDealer<GC2GS_013_002_ReqHeroSetSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_013_002_ReqHeroSetSkin _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (userData == null)
            return;

        long skinId = _msg.getSkinId();
        RefHeroSkin refHeroSkin = RefHeroSkin.getMgr().get(skinId);
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_SET_SKIN);
        //升级逻辑
        Result unlockResult = heroInfo.chgSkin(_msg.getSkinId(), context);
        if (!unlockResult.isSucc())
        {
            _commiter.commitFailRes(unlockResult.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_013_HeroOp.make_002_RetHeroSetSkin());
        
        //日志数据
        BM bmObj = userData.getUSServer().getBM();
        Opt013002HeroSetSkinBO optBo = new Opt013002HeroSetSkinBO();
        optBo.setHeroId(bmObj, heroInfo.getHeroId());
        optBo.setSkinId(bmObj, _msg.getSkinId());
        userData.logEvent(optBo, context);
    }
}
