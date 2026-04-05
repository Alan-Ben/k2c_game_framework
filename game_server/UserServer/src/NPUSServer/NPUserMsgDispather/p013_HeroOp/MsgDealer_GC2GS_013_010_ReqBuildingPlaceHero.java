package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_010_ReqBuildingPlaceHero;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;

public class MsgDealer_GC2GS_013_010_ReqBuildingPlaceHero extends NPUserMsgDealer<GC2GS_013_010_ReqBuildingPlaceHero>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_013_010_ReqBuildingPlaceHero _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.HERO_BUILDING_PLACE);
        //逻辑
        Result result = heroInfo.placeToBuilding(_msg.getBuildingId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_013_HeroOp.make_010_RetBuildingPlaceHero());
    }
}
