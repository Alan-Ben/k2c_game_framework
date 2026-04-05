package  NPUSServer.NPUserMsgDispather.p008_TravelOp;

import Common.TravelObj.Travel_EventResult;
import GC2GS.p008_TravelOp.GC2GS_008_002_ReqAkeyTravel;
import MJLog.MJEventLog;
import NPCommon.ErrMain.TravelErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_008_TravelOp;

import java.util.ArrayList;
public class  MsgDealer_GC2GS_008_002_ReqAkeyTravel extends NPUserMsgDealer<GC2GS_008_002_ReqAkeyTravel>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_008_002_ReqAkeyTravel _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //获取玩家当前可以游历的次数
        long cdCount = userData.getLazyCDComponent().getItemCount(RefGeneral.Ref().travel_cost_lazycd_id);
        int realCount = (int) Math.min(cdCount, RefGeneral.Ref().travel_akey_event_limit);
        if(realCount <= 0)
        {
        	_commiter.commitFailRes(TravelErr.TRAVEL_COST_NOT_ENOUGH.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TRAVEL_AKEY_DEAL);
        //执行批量事件，单次执行消耗
        if(!userData.getLazyCDComponent().spendItem(RefGeneral.Ref().travel_cost_lazycd_id, realCount, context))
        {
        	_commiter.commitFailRes(TravelErr.TRAVEL_COST_FAIL.getCode());
        	return;
        }
        
        //一键处理流程
        ArrayList<Travel_EventResult> dealedResultList = new ArrayList<>();
        ArrayList<TravelCanDealEventInfo> canDealEventList = new ArrayList<>();
        userData.getTravelComponent().akeyEvent(realCount, dealedResultList, canDealEventList, context);
     
        _commiter.commitSucRes(US2GCWriter_008_TravelOp.make_002_RetAkeyTravel(dealedResultList));

        MJEventLog.logTravel(
                userData,
                dealedResultList.size(),  // 本次游历次数
                context.getContextId()  // 事件ID（从context获取）
        );
    }
}
