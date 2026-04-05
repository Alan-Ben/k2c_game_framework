package  NPUSServer.NPUserMsgDispather.p008_TravelOp;

import GC2GS.p008_TravelOp.GC2GS_008_001_ReqStartTravel;
import MJLog.MJEventLog;
import NPCommon.ErrMain.TravelErr;
import NPCommon.Util.Pair.WCGPairLong;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_008_TravelOp;
public class  MsgDealer_GC2GS_008_001_ReqStartTravel extends NPUserMsgDealer<GC2GS_008_001_ReqStartTravel>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_008_001_ReqStartTravel _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查消耗
  		if(!userData.hasItem(ENPItemType.LAZY_CD, RefGeneral.Ref().travel_cost_lazycd_id, 1))
  		{
  			_commiter.commitFailRes(TravelErr.TRAVEL_EVENT_NOT_FOUND.getCode());
  			return;
  		}
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TRAVEL_START);
		//执行消耗
		if(!userData.spendItem(ENPItemType.LAZY_CD, RefGeneral.Ref().travel_cost_lazycd_id, 1, context))
		{
			_commiter.commitFailRes(TravelErr.TRAVEL_COST_FAIL.getCode());
			return;
		}
		
		/**
		 * GOB-1456 【游玩】支持配置前期一定会触发的事件
		 * https://www.teambition.com/task/67da8e79bbb9e767ba5c0655
		 */
		/**
		 * 优化
		 * GOB-4164 【优化-0】游历--前期游历触发的事件列表触发前先判断是否符合条件
		 * https://www.teambition.com/task/68873ba6122e42a7b9c7426b
		 * 
		 * 总结整理：
		 * 1. 每次先从前期事件列表中按顺序遍历，获取第一个可以触发的事件，如果没有事件，则随机挑选
		 * 2. 如果前期列表已经选完，则之后不再进行检查，直接进入随机挑选（无视后续的配置修改）
		 */
		WCGPairLong earlyEventPair = userData.getTravelComponent().popFirstUnFinishedEarlyEvent(context);

        TravelCanDealEventInfo info;
        if(null != earlyEventPair) //指定事件
		{
            info = userData.getTravelComponent().createEvent(earlyEventPair.first(), earlyEventPair.second(), context);
        }
		else //随机事件
		{
            info = userData.getTravelComponent().createRandEvent(context);
        }

        if(null == info)
        {
            _commiter.commitFailRes(TravelErr.TRAVEL_EVENT_CREATE_FAIL.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_008_TravelOp.make_001_RetStartTravel());

		MJEventLog.logTravel(
				userData,
				1,  // 本次游历次数
				context.getContextId()  // 事件ID（从context获取）
		);
    }
}
