package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_016_ReqSetTempRepairDone;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041016MarsExploreTeamSetRepairDoneBO;

public class MsgDealer_GC2GS_041_016_ReqSetTempRepairDone extends NPUserMsgDealer<GC2GS_041_016_ReqSetTempRepairDone>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_016_ReqSetTempRepairDone _msg)
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
        
        //队伍完成修复，进入IDLE状态
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_TEAM_SET_REPAIR_DONE);
        Result result = team.setRepairDone(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_016_RetSetTempRepairDone());

        //日志数据
        Opt041016MarsExploreTeamSetRepairDoneBO optBo = new Opt041016MarsExploreTeamSetRepairDoneBO();
        optBo.setTeamId(getUSServer().getBM(), _msg.getTeamId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
