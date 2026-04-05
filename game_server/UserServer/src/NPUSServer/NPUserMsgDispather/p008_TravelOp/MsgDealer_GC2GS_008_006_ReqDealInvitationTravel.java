package  NPUSServer.NPUserMsgDispather.p008_TravelOp;

import Common.TravelEnum.ETravelEventType;
import GC2GS.p008_TravelOp.GC2GS_008_006_ReqDealInvitationTravel;
import NPCommon.ErrMain.TravelErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_TRAVEL_EVENT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer.TravelEventDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer.TravelEventDealerResult;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_008_TravelOp;
public class  MsgDealer_GC2GS_008_006_ReqDealInvitationTravel extends NPUserMsgDealer<GC2GS_008_006_ReqDealInvitationTravel>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_008_006_ReqDealInvitationTravel _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TRAVEL_DEAL);
        
        //获取并移除对应的游历事件
        TravelCanDealEventInfo info = userData.getTravelComponent().doneCanDealEvent(_msg.getInstanceId(), context);
        if(null == info)
        {
        	_commiter.commitFailRes(TravelErr.TRAVEL_EVENT_NOT_FOUND.getCode());
        	return;
        }
        
        //处理游历事件数据
        TravelEventDealerResult result = TravelEventDealerMgr.getInstance().dealEvent(ETravelEventType.INVITATION, info, _msg, context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getErrCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_008_TravelOp.make_006_RetDealInvitationTravel(result.getEventResult()));

        //触发事件
        Event_P_TRAVEL_EVENT event = new Event_P_TRAVEL_EVENT(context);
        userData.onLogicEvent(event);
    }
}
