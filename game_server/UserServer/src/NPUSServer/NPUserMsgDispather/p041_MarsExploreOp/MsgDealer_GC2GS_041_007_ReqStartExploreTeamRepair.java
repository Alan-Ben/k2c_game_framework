package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_007_ReqStartExploreTeamRepair;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041007MarsExploreTeamStartRepairBO;

public class MsgDealer_GC2GS_041_007_ReqStartExploreTeamRepair extends NPUserMsgDealer<GC2GS_041_007_ReqStartExploreTeamRepair>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_007_ReqStartExploreTeamRepair _msg)
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
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_START_REPAIR);
        //转换维修状态
        Result result = team.startRepair(_msg.getRepairNum(), context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_007_RetStartExploreTeamRepair());

        //日志数据
        Opt041007MarsExploreTeamStartRepairBO optBo = new Opt041007MarsExploreTeamStartRepairBO();
        optBo.setTeamId(getUSServer().getBM(), _msg.getTeamId());
        optBo.setRepairNum(getUSServer().getBM(), _msg.getRepairNum());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
