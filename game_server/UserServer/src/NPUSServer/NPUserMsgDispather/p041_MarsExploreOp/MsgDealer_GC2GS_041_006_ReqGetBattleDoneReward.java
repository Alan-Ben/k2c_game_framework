package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_006_ReqGetBattleDoneReward;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent.MarsExploreEvent_BATTLE;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreEvent._AMarsExploreEventInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041006MarsExploreBattleDoneRewardBO;

public class MsgDealer_GC2GS_041_006_ReqGetBattleDoneReward extends NPUserMsgDealer<GC2GS_041_006_ReqGetBattleDoneReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_006_ReqGetBattleDoneReward _msg)
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
        if(!(info instanceof MarsExploreEvent_BATTLE))
        {
        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_EVENT_TYPE_ERROR.getCode());
        	return;
        }
        
        //处理过程
        MarsExploreEvent_BATTLE battleEvent = (MarsExploreEvent_BATTLE) info;
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_EXPLORE_EVENT_GET_DONE_REWARD);
        Result result = battleEvent.getDoneReward(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_006_RetGetBattleDoneReward(context));

        //日志数据
        Opt041006MarsExploreBattleDoneRewardBO optBo = new Opt041006MarsExploreBattleDoneRewardBO();
        optBo.setExploreLvl(userData.getUSServer().getBM(), info.getExploreLvl());
        optBo.setEventInstanceId(userData.getUSServer().getBM(), _msg.getId());
        optBo.setEventRefId(userData.getUSServer().getBM(), info.getEventId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
