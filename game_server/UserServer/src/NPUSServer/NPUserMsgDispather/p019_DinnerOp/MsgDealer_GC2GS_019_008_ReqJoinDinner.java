package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import CommonEnum.ECurrency;
import GC2GS.p019_DinnerOp.GC2GS_019_008_ReqJoinDinner;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.DinnerErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Dinner.RefDinnerJoinCost;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_DINNER_SCORE_UP;
import NPUSServer.Common.Event.Events.Event_P_JOIN_DINNER;
import NPUSServer.NPUSUserMgr.GameSystem.DinnerSystem.DinnerSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp.PlayerFixedCD;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
import NPUSServer.USLog;
import USLOGDB.OptBo.Opt019008DinnerJoinBO;
public class  MsgDealer_GC2GS_019_008_ReqJoinDinner extends NPUserMsgDealer<GC2GS_019_008_ReqJoinDinner>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_008_ReqJoinDinner _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查配置数据
        RefDinnerJoinCost costRef = RefDinnerJoinCost.getMgr().get(_msg.getCostId());
        if(null == costRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }

        //检查道具
        if(!userData.hasItem(costRef.cost_item))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        //检测每日最大赴宴次数
        PlayerFixedCD fixedCd = userData.getFixedCdComponent().lookupByRefId(costRef.fixed_cd_id);
        if(costRef.fixed_cd_id > 0)
        {
        	if(null == fixedCd || fixedCd.getCount() < 1)
        	{
        		_commiter.commitFailRes(DinnerErr.DINNER_JOIN_CD_NOT_ENOUGH.getCode());
        		return;
        	}
        }
        
        /********** 加入宴会流程
         * 1. 先获取宴会数据
         * 2. 检查完宴会数据，发起加入宴会
         * 3. 加入宴会成功，获取对应的宴会结算
         *************************/
        DinnerSystem.GetDinnerInfo(userData, _msg.getInstanceId(), (_getDinnerErr, _dinnerInfo) -> 
        {
        	if(_getDinnerErr > 0)
        	{
        		_commiter.commitFailRes(_getDinnerErr);
        		return;
        	}
        	
        	//不能加入自己的举办的宴会
        	if(_dinnerInfo.getIdx().getOwnerCid() == userData.getCid())
        	{
        		_commiter.commitFailRes(DinnerErr.DINNER_IS_OWN.getCode());
        		return;
        	}
        	
        	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DINNER_JOIN);

        	//消耗道具
        	if(!userData.spendItem(costRef.cost_item, context))
        	{
        		_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        		return;
        	}
            //最大赴宴次数计数
        	if(null != fixedCd)
        	{
        		fixedCd.descCount(1, context);
        	}
        	
        	//计算玩家赴宴获取的宴会币
            long gainCoin = DinnerSystem.CalJoinerCoin(userData, costRef.join_gain_coin);
            //计算玩家赴宴获取的宴会积分
            long gainScore = DinnerSystem.CalJoinerScore(userData, costRef.join_gain_score);
            
        	//发起赴宴操作
        	DinnerSystem.SendJoinDinner(userData, _msg.getInstanceId(), _msg.getCostId(), gainCoin, gainScore, context, _errCode -> 
        	{
        		if(_errCode > 0)
        		{
        			NPPlayerContext returnContext = NPPlayerContext.createNew(ENPGameEvent.DINNER_RETURN);
        			returnContext.setGuid(context.getGuid());
        			//最大赴宴次数计数返回补偿
                	if(null != fixedCd)
                	{
                		fixedCd.addCountExt(1);
                	}
                	//之前消耗道具进行返回补偿
                	userData.gainItem(costRef.cost_item, returnContext);
        			
        			_commiter.commitFailRes(_errCode);
        			return;
        		}
        		
        		//获取赴宴币
        		userData.gainItem(ENPItemType.CURRENCY, ECurrency.DINNER_COIN.ordinal(), gainCoin, context);
        		
        		//回包处理
        		_commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_008_RetJoinDinner(context));
        		
        		//增加赴宴记录计数
        		userData.getRecordComponent().addRecord(ENPPlayerRecordParam.JOIN_DINNER_COUNT, 1, context);
        		//触发事件
        		Event_P_JOIN_DINNER event = new Event_P_JOIN_DINNER(context, _dinnerInfo.getIdx().getDinnerId());
        		userData.onLogicEvent(event);
        		
        		//触发玩家宴会积分增加事件
        		Event_P_DINNER_SCORE_UP scoreUpEvent = new Event_P_DINNER_SCORE_UP(context, gainScore);
        		userData.onLogicEvent(scoreUpEvent);
        		
        		//增加玩家交互记录
        		userData.getDinnerComponent().addLastEachLog(true, _dinnerInfo.getIdx().getOwnerCid(), context);
        		//对方玩家增加交互记录
        		ALSynTaskManager.getInstance().regTask(()->
        		{
        			DinnerSystem.SendAddBeJoinedCount(userData, _dinnerInfo.getIdx().getOwnerCid(), _sendBeJoinedResultErrCode -> 
        			{
        				if(_sendBeJoinedResultErrCode > 0)
        				{
        					USLog.error(getUSServer(), "player:{} ownerPlayer:{} SendAddBeJoinedCount fail, errCode:{}."
        							, userData.getCid(), _dinnerInfo.getIdx().getOwnerCid(), _sendBeJoinedResultErrCode);
        				}
        			} , context);
        		});

				MJEventLog.logParty(userData, _dinnerInfo.getIdx().getDinnerId(), 1);
				
				//玩家赴宴记录
				Opt019008DinnerJoinBO optBo = new Opt019008DinnerJoinBO();
		        optBo.setInstanceId(userData.getUSServer().getBM(), _msg.getInstanceId());
		        optBo.setCostId(userData.getUSServer().getBM(), _msg.getCostId());
		        optBo.setGainCoin(userData.getUSServer().getBM(), gainCoin);
		        optBo.setGainScore(userData.getUSServer().getBM(), gainScore);
		        _commiter.getUserData().logEvent(optBo, context);
        	});
        });
    }
}