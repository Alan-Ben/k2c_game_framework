package NPUSServer.NPUserMsgDispather.p014_ChildOp;

import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import CommonEnum.ESpecialItemType;
import GC2GS.p014_ChildOp.GC2GS_014_002_ReqTrainChild;
import GS2GC.p004_PlayerOp.GS2GC_004_056_OnGoldInfoChg;
import NPCommon.ErrMain.ChildErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_TRAIN_CHILD;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildSeatInfo;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Gold;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014002ChildTrainBO;

import java.util.ArrayList;

public class MsgDealer_GC2GS_014_002_ReqTrainChild extends NPUserMsgDealer<GC2GS_014_002_ReqTrainChild> {
	@Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_002_ReqTrainChild _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ChildInfo child = userData.getChildComponent().getChildMgr().lookupChild(_msg.getId());
        if(null == child)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_EXISTS.getCode());
			//重新推送正确数据
			_onCommitFail(userData, _msg);
        	return;
        }
        
        //子嗣尚未命名
        if(child.getName().isEmpty())
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NAME_NOT_SET.getCode());
			//重新推送正确数据
			_onCommitFail(userData, _msg);
        	return;
        }
        
        //检查子嗣等级
        if(child.getLvl() >= child.getMaxLvl())
        {
        	_commiter.commitFailRes(ChildErr.CHILD_LVL_FULL.getCode());
			//重新推送正确数据
			_onCommitFail(userData, _msg);
        	return;
        }
        
        //检查训练点位
        ChildSeatInfo seat = userData.getChildComponent().getChildMgr().lookupSeat(child.getSeatId());
        if(null == seat)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_SEAT_NOT_EXIST.getCode());
			//重新推送正确数据
			_onCommitFail(userData, _msg);
        	return;
        }
        
        //检查训练位脑力值
        int curEnergy = seat.calAndGetEnergy();
        if(curEnergy <= 0)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_SEAT_NOT_ENERGY.getCode());
			//重新推送正确数据
			_onCommitFail(userData, _msg);
        	return;
        }
        
        //原等级，用于确认是否达到训练等级
        int oriLvl = child.getLvl();
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHILD_TRAIN);
        
        //计算需要扣除的金币值
        long costValue = userData.getPlayerComponent().getCurLevel().child_educate_cost;
        if(costValue > 0)
        {
        	//扣除减少万分比
        	int costPer = (int) child.getBonusPropertyValue(EBonusPropertyType.CHILD_TRAIN_COST_PER);
        	//计算实际扣除数值
        	costValue = (long) (costValue * (10000 - costPer) / 10000d);
            //检查扣除消耗
        	if(costValue > 0)
        	{
        		if(!userData.hasItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), costValue))
        		{
        			_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        			//重新推送正确数据
        			_onCommitFail(userData, _msg);
        			return;
        		}
        		
        		if(!userData.spendItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), costValue, context))
        		{
        			_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        			//重新推送正确数据
        			_onCommitFail(userData, _msg);
        			return;
        		}
        	}
        }
    	//检查获取大臣经验
    	long gainValue = userData.getPlayerComponent().getCurLevel().child_educate_get_hero_exp;
    	if(gainValue > 0)
    	{
    		//大臣经验加成加成
    		int gainPer = (int) child.getBonusPropertyValue(EBonusPropertyType.CHILD_TRAIN_GAIN_PER) 
    				+ child.getInitStudyBonus();
    		//计算获取的大臣经验
			gainValue = (long) Math.ceil(gainValue * (10000 + gainPer) / 10000d);
        	if(gainValue > 0)
        	{
        		//卷王子嗣收益X2
        		if(child.isGiftde())
        		{
        			gainValue = gainValue * 2;
        		}
				gainValue += child.getBonusPropertyValue(EBonusPropertyType.CHILD_TRAIN_GAIN);
        		
        		userData.gainItem(ENPItemType.CURRENCY, ECurrency.HERO_EXP.ordinal(), gainValue, context);
        	}
    	}
        
        //消耗脑力值
        seat.descEnergy(1, context);
        //增加子嗣等级
        child.incrLvl(context);
        
        //训练等级/阶段增加上课收益
    	long finalAddValue = 0;
        if(null != child.getQualityRef())
        {
        	//检查当前等级是否可以增加上课收益
            long addValue = (long) Math.ceil(child.getBaseBonus() * (RefGeneral.Ref().child_add_per) / 10000d);
            ArrayList<Integer> trainAddBonusLvlList = child.getQualityRef().addBonusStepList;
            for(int i = 0; i < trainAddBonusLvlList.size(); i++)
            {
            	int curTrainAddBonusLvl = trainAddBonusLvlList.get(i);
            	if(curTrainAddBonusLvl > child.getLvl())
            		break;
            	
            	if(curTrainAddBonusLvl > oriLvl)
            	{
            		finalAddValue += addValue;
            	}
            }
            //检查当前阶段可以增加的收益
            if(child.getLvl() != child.getMaxLvl() && child.getQualityRef().step_lvl.contains(child.getLvl()))
            {
            	long addStepValue = (long) Math.ceil(child.getBaseBonus() * (RefGeneral.Ref().child_train_add_per / 10000d));
            	finalAddValue += addStepValue;
            }
            //最终实际增加的训练属性
            if(finalAddValue > 0)
    		{
            	//卷王子嗣，获得收益X2
            	if(child.isGiftde())
            	{
            		finalAddValue = finalAddValue * 2;
            	}
            	
            	child.incrTrainBonus(finalAddValue, context);
    		}
        }

        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_002_RetTrainChild(costValue, gainValue, finalAddValue));

        userData.onLogicEvent(new Event_P_TRAIN_CHILD(context, _msg.getId(), 1, finalAddValue, gainValue));

        //日志
        Opt014002ChildTrainBO optBo = new Opt014002ChildTrainBO();
        optBo.setChildId(getUSServer().getBM(), _msg.getId());
        optBo.setOriLvl(getUSServer().getBM(), oriLvl);
        optBo.setCurLvl(getUSServer().getBM(), child.getLvl());
        optBo.setCostSilver(getUSServer().getBM(), costValue);
        optBo.setAddHeroExp(getUSServer().getBM(), gainValue);
        optBo.setAddBonus(getUSServer().getBM(), finalAddValue);
        userData.logEvent(optBo, context);
    }

	/**
	 * 失败的时候再次重新推送正确数据
	 * 
	 * @param _userData
	 * @param _msg
	 */
	private void _onCommitFail(NPUSUserData _userData, GC2GS_014_002_ReqTrainChild _msg) {
		// 重新推送子嗣数据 - GS2GC_014_052_OnChildLvlChg
		ChildInfo child = _userData.getChildComponent().getChildMgr().lookupChild(_msg.getId());
		if (null != child) {
			_userData.sendMsgToGC(US2GCWriter_014_ChildOp.make_052_OnChildLvlChg(child));
		}
		// 重新推送训练点位 - GS2GC_014_053_OnSeatChg
		ChildSeatInfo seat = _userData.getChildComponent().getChildMgr().lookupSeat(child.getSeatId());
		if (null != seat) {
			_userData.sendMsgToGC(US2GCWriter_014_ChildOp.make_053_OnSeatChg(seat));
		}
		// 重新推送金币结算信息 - GS2GC_004_056_OnGoldInfoChg
		SpecialItemDealer_Gold goldDealer = _userData.getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
		if (goldDealer != null) {
			_userData.sendMsgToGC(new GS2GC_004_056_OnGoldInfoChg(goldDealer.toProto()));
		}
		// 重新推送Hero_Exp数据 - GS2GC_004_051_PushCurrencyInfo
		long heroExpCount = _userData.getCurrencyComponent().getItemCount(ECurrency.HERO_EXP.ordinal());
		_userData.sendMsgToGC(
				US2GCWriter_004_PlayerOp.make_051_PushCurrencyInfo(ECurrency.HERO_EXP.ordinal(), heroExpCount));
		// 重新推送所有子嗣bonus总和 - GS2GC_014_061_OnChildBonusSumChg
		long bonusSum = _userData.getChildComponent().getBonusSum();
		_userData.sendMsgToGC(US2GCWriter_014_ChildOp.make_061_OnChildBonusSumChg(bonusSum));
	}
}