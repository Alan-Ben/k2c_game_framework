package NPUSServer.DinnerMgr;

import ALBasicServer.ALTask._IALSynTask;
import Common.DinnerObj.Dinner_ResultGuestInfo;
import Common.DinnerObj.Dinner_StartLogIdx;
import Common.DinnerObj.Dinner_StartLogInfo;
import Common.MailObj.Mail_Data;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_DinnerResult;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_DINNER_SCORE_UP;
import NPUSServer.NPEvent.EventMgr.EventObj.NPGlobalUserEventObj;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerDinnerStartLogBO;
import USDB.Bo.PlayerOfflineRewardBO;

/**
 * 开宴玩家结算
 * @author mj
 *
 */
public class DinnerSettleOwnerTask implements _IALSynTask
{
	//US服务器
	private NPUserServer _m_usUSServer;
	//宴会数据
	private DinnerInfo _m_diDinner;
	//宴会结算数据
	private ServerObj_DinnerResult _m_drDinnerResult;
	//上下文对象
	private NPPlayerContext _m_ctxContext;
	
	public DinnerSettleOwnerTask(NPUserServer _usServer, DinnerInfo _dinner, ServerObj_DinnerResult _result, NPPlayerContext _context)
	{
		_m_usUSServer = _usServer;
		
		_m_diDinner = _dinner;
		
		_m_drDinnerResult = _result;
		
		_m_ctxContext = _context;
	}

	@Override
	public void run() 
	{
		//开宴日志数据，直接存bo数据
		Dinner_StartLogIdx startLogIdx = new Dinner_StartLogIdx();
		startLogIdx.setInstanceId(_m_drDinnerResult.getResult().getInstanceId());
		startLogIdx.setDinnerId(_m_drDinnerResult.getResult().getDinnerId());
		startLogIdx.setGainScore(_m_drDinnerResult.getResult().getGainScore());
		startLogIdx.setStartTs(_m_drDinnerResult.getStartTs());
		startLogIdx.setJoinerCount(_m_diDinner.getJoinerMgr().getJoinerCount());
		startLogIdx.setPermitType(_m_diDinner.getPermitType());
		startLogIdx.setPermitTypeId(_m_diDinner.getPermitTypeId());
		
		Dinner_StartLogInfo startLogInfo = new Dinner_StartLogInfo();
		for(int i = 0; i < _m_drDinnerResult.getResult().getGuestLog().size(); i++)
		{
			Dinner_ResultGuestInfo guest = _m_drDinnerResult.getResult().getGuestLog().get(i);
			if(null == guest)
				continue;
			
			startLogInfo.addGuestLog(guest);
		}
		
		//开宴结算数据
		NPUSUserData userData = _m_usUSServer.getUsUserMgr().lookupCacheUserData(_m_drDinnerResult.getOwnerCid());
		if(null != userData)
		{
			userData.safeCall(()->
			{
				//宴会结算奖励数据
				userData.getDinnerComponent().addOwnerReward(_m_drDinnerResult.getResult(), _m_ctxContext);
				//宴会开宴日志
				userData.getDinnerComponent().getStartLogList().addStartLog(startLogIdx, startLogInfo);
				
				//推送玩家结束数据
				userData.sendMsgToGC(US2GCWriter_019_DinnerOp.make_051_OnDinnerEnd(_m_drDinnerResult.getResult().getInstanceId()));
				
        		//触发玩家宴会积分增加事件
        		Event_P_DINNER_SCORE_UP scoreUpEvent = new Event_P_DINNER_SCORE_UP(_m_ctxContext, _m_drDinnerResult.getResult().getGainScore());
        		userData.onLogicEvent(scoreUpEvent);
			});
		}
		else
		{
			//开宴结算奖励
			PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
			bo.setRewardType(_m_usUSServer.getBM(), EOfflineRewardEnum.DINNER_OWNER_RESULT.ordinal());
			bo.setCid(_m_usUSServer.getBM(), _m_drDinnerResult.getOwnerCid());
			bo.setOfflineData(_m_usUSServer.getBM(), CommonFunc.ByteBfferToBytes(_m_drDinnerResult.getResult().makePackage()));
			bo.insert(_m_usUSServer.getBM());
			
			//开宴日志数据
			PlayerDinnerStartLogBO startLogBo = new PlayerDinnerStartLogBO();
			startLogBo.setCid(_m_usUSServer.getBM(), _m_drDinnerResult.getOwnerCid());
			startLogBo.setInstanceId(_m_usUSServer.getBM(), _m_drDinnerResult.getResult().getInstanceId());
			startLogBo.setLogIdx(_m_usUSServer.getBM(), CommonFunc.ByteBfferToBytes(startLogIdx.makePackage()));
			startLogBo.setLogInfo(_m_usUSServer.getBM(), CommonFunc.ByteBfferToBytes(startLogInfo.makePackage()));
			startLogBo.setStartTs(_m_usUSServer.getBM(), _m_drDinnerResult.getStartTs());
			startLogBo.insert(_m_usUSServer.getBM());

    		//触发玩家宴会积分增加事件
    		Event_P_DINNER_SCORE_UP scoreUpEvent = new Event_P_DINNER_SCORE_UP(_m_ctxContext, _m_drDinnerResult.getResult().getGainScore());
    		_m_usUSServer.getGlobalEventHandlerMgr().handle(scoreUpEvent, new NPGlobalUserEventObj(_m_drDinnerResult.getOwnerCid()));
		}

		//开宴玩家结算邮件
		if(RefGeneral.Ref().dinner_owner_settle_mail_id > 0)
		{
			Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().dinner_owner_settle_mail_id);
            mailData.setIsMustRead(false);
            mailData.getContentReplace().add(_m_diDinner.getRef().name);
            
			MailSystem.addMail(_m_usUSServer, _m_drDinnerResult.getOwnerCid(), mailData, _m_ctxContext);
		}
	}
}
