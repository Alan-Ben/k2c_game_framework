package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_003_ReqSetExploreTeamHero;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

import java.util.ArrayList;

public class MsgDealer_GC2GS_041_003_ReqSetExploreTeamHero extends NPUserMsgDealer<GC2GS_041_003_ReqSetExploreTeamHero>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_003_ReqSetExploreTeamHero _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查大臣数量上限
        if(_msg.getHeroIdList().size() > RefGeneral.Ref().mars_explore_team_hero_max_num)
        {
        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_TEAM_HERO_LIMIT.getCode());
        	return;
        }
        
        //检查队伍
        MarsExploreTeam team = userData.getMarsExploreComponent().getTeamMgr().lookup(_msg.getTeamId());
        if(null == team)
        {
        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_TEAM_NOT_FOUND.getCode());
        	return;
        }
        
         //检查大臣
        ArrayList<HeroInfo> heroList = new ArrayList<>();
        for(int i = 0; i < _msg.getHeroIdList().size(); i++)
        {
        	HeroInfo hero = userData.getHeroComponent().lookupHero(_msg.getHeroIdList().get(i));
        	if(null == hero)
        	{
            	_commiter.commitFailRes(HeroErr.HERO_NOT_FOUND.getCode());
            	return;
        	}
        	
        	heroList.add(hero);
        }
        
        //处理数据
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_SET_HERO);
        Result result = team.setHeroList(heroList, context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_003_RetSetExploreTeamHero());
    }
}
