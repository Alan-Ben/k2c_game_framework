package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_006_ReqHeroStarUpgrade;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;

public class MsgDealer_GC2GS_013_006_ReqHeroStarUpgrade extends NPUserMsgDealer<GC2GS_013_006_ReqHeroStarUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_013_006_ReqHeroStarUpgrade _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_STAR_UP);

        //升级逻辑
        Result result = heroInfo.improveStar(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_013_HeroOp.make_006_RetHeroStarUpgrade());
    }
}
