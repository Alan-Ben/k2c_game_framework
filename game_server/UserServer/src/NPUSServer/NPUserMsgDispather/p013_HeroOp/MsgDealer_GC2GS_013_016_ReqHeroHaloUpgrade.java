package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_016_ReqHeroHaloUpgrade;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroHaloInfo;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;

public class MsgDealer_GC2GS_013_016_ReqHeroHaloUpgrade extends NPUserMsgDealer<GC2GS_013_016_ReqHeroHaloUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_013_016_ReqHeroHaloUpgrade _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData == null)
            return;

        HeroInfo heroInfo = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if (heroInfo == null)
        {
            _committer.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_HALO_UPGRADE);

        HeroHaloInfo haloInfo = heroInfo.getHaloInfo();
        if (haloInfo == null)
        {
            _committer.commitFailRes(HeroErr.HERO_DONT_HAVE_HALO.getCode());
            return;
        }

        //升级逻辑
        Result result = haloInfo.upgrade(context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_013_HeroOp.make_016_RetHeroHaloUpgrade());
    }
}