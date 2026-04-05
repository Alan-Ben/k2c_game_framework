package NPUSServer.NPUserMsgDispather.p037_GuildDungeonOp;

import GC2GS.p037_GuildDungeonOp.GC2GS_037_006_ReqRecoverHeroFight;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_037_GuildDungeonOp;

public class MsgDealer_GC2GS_037_006_ReqRecoverHeroFight extends NPUserMsgDealer<GC2GS_037_006_ReqRecoverHeroFight>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_037_006_ReqRecoverHeroFight _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查大臣
        HeroInfo hero = userData.getHeroComponent().lookupHero(_msg.getHeroId());
        if(null == hero)
        {
        	_commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_DUNGEON_RECOVER_FIGHT);
        
        Result result = userData.getGuildDungeonComponent().heroRecover(hero, context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_037_GuildDungeonOp.make_006_RetRecoverHeroFight());
    }
}
