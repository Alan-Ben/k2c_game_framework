package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_005_ReqStartDealExploreEvent;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent._AMarsExploreEventInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041005MarsExploreStartBO;

public class MsgDealer_GC2GS_041_005_ReqStartDealExploreEvent extends NPUserMsgDealer<GC2GS_041_005_ReqStartDealExploreEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_005_ReqStartDealExploreEvent _msg)
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
        
        //检查事件
        _AMarsExploreEventInfo info = userData.getMarsExploreComponent().getEventMgr().lookup(_msg.getId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_EVENT_NOT_FOUND.getCode());
        	return;
        }
        
        //更新队伍状态
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_START_DEAL);
        Result result = info.dealStart(team, context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_005_RetStartDealExploreEvent());

        //日志数据
        Opt041005MarsExploreStartBO optBo = new Opt041005MarsExploreStartBO();
        optBo.setTeamId(userData.getUSServer().getBM(), _msg.getTeamId());
        optBo.setExploreLvl(userData.getUSServer().getBM(), info.getExploreLvl());
        optBo.setEventInstanceId(userData.getUSServer().getBM(), _msg.getId());
        optBo.setEventType(userData.getUSServer().getBM(), info.getEventType().ordinal());
        optBo.setEventRefId(userData.getUSServer().getBM(), info.getEventId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
