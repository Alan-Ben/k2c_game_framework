package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_009_ReqGetBossDoneReward;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent.MarsExploreEvent_BOSS;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent._AMarsExploreEventInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

public class MsgDealer_GC2GS_041_009_ReqGetBossDoneReward extends NPUserMsgDealer<GC2GS_041_009_ReqGetBossDoneReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_009_ReqGetBossDoneReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //检查事件
        _AMarsExploreEventInfo info = userData.getMarsExploreComponent().getEventMgr().lookup(_msg.getId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_EVENT_NOT_FOUND.getCode());
        	return;
        }
        
        //检查事件类型
        if(!(info instanceof MarsExploreEvent_BOSS))
        {
        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_EVENT_TYPE_ERROR.getCode());
        	return;
        }
        
        //处理过程
        MarsExploreEvent_BOSS battleEvent = (MarsExploreEvent_BOSS) info;
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_EVENT_GET_DONE_REWARD);
        Result result = battleEvent.getDoneReward(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_009_RetGetBossDoneReward(context));
    }
}
