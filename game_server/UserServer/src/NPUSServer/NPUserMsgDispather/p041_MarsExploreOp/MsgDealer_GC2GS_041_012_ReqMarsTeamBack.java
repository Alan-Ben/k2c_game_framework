package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_012_ReqMarsTeamBack;
import NPCommon.ErrMain.MarsErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

public class MsgDealer_GC2GS_041_012_ReqMarsTeamBack extends NPUserMsgDealer<GC2GS_041_012_ReqMarsTeamBack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_012_ReqMarsTeamBack _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查队伍
        MarsExploreTeam team = userData.getMarsExploreComponent().getTeamMgr().lookup(_msg.getTeamId());
        if(null == team)
        {
        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_TEAM_NOT_FOUND.getCode());
        	return;
        }
        
        //处理队伍召回
        team.leaveMarsMine((err ->
        {
        	if(err > 0)
        	{
            	_commiter.commitFailRes(err);
            	return;
            }
        	
        	_commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_012_RetMarsTeamBack());
        }));
    }
}
