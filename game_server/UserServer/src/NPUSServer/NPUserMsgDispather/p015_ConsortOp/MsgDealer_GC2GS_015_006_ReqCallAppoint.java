package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_006_ReqCallAppoint;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Common.RefTimePrice;
import NPGameRes.Refs.Consort.RefConsortTravel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GAIN_CHILD;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.ChildSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTravel.ConsortTravelInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import USLOGDB.OptBo.Opt015006ConsortAppointBO;

import java.util.List;
public class  MsgDealer_GC2GS_015_006_ReqCallAppoint extends NPUserMsgDealer<GC2GS_015_006_ReqCallAppoint>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_006_ReqCallAppoint _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ConsortInfo consort = userData.getConsortComponent().lookup(_msg.getConsortId());
        if(null == consort)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        //检查出游配置
        RefConsortTravel travelRef = RefConsortTravel.getMgr().get(_msg.getConsortTravelId());
        if(null == travelRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_APPOINT_TRAVEL);
        //计算并累计消耗
        NPItemCollector costItemCollector = new NPItemCollector(context.getContextId());
        
        int calTravelCount = 0;
        //计算金币消耗
        if(travelRef.time_price_id > 0)
        {
        	//刷新出游次数数据
        	userData.getConsortComponent().getTravelMgr().refreshGemCallCount(context);
        	//获取对应的出游数据
        	ConsortTravelInfo travel = userData.getConsortComponent().getTravelMgr().lookup(_msg.getConsortTravelId());
        	if(null == travel)
        	{
            	_commiter.commitFailRes(CommErr.DATA_STATE_ERR.getCode());
            	return;
            }
        	//消耗计数表
        	int nextTravelCount = travel.getTravelCount() + 1;
        	calTravelCount = nextTravelCount;
        	RefTimePrice costTimePriceRef = RefTimePrice.getMgr().getPrice(travelRef.time_price_id, nextTravelCount);
        	if(null == costTimePriceRef)
        	{
            	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            	return;
            }
        	//计算消耗
        	long costValue = NPPlayerVariableDeal.getInstance().CalculateVariableResult(userData, costTimePriceRef.cost_item_formula, null);
        	//累计消耗
        	costItemCollector.addItem(costTimePriceRef.item, costValue);
        }
        //计算物品消耗
        if(null != travelRef.travel_cost_item)
        {
        	costItemCollector.addItem(travelRef.travel_cost_item);
        }
        //检查物品
        List<NPCommonCostItem> finalCostItemList = costItemCollector.getAllItemList();
    	if(!userData.hasCostItemList(finalCostItemList))
    	{
    		_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
    	}
    	//消耗物品
    	if(!userData.spendItem(finalCostItemList, context))
    	{
    		_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
    	}
    	
        //增加出游计数
    	userData.getConsortComponent().getTravelMgr().checkAndIncr(_msg.getConsortTravelId(), context);
    	
        //计算并获取的加护点
        long addCharmPer = consort.getCharmPointPer();
        long charmPointExtraAdd = consort.getCharmPointExtraAdd();
        long addCharmPoint = (long) Math.ceil(1.0 * consort.getCharm() * (1 + addCharmPer / 10000f) * travelRef.bless_point_multiple) + charmPointExtraAdd;
        consort.incrCharmPoint(addCharmPoint, context);
        //增加加护力
        consort.incrCharm(travelRef.add_bless, context);
        
        //子嗣数据
        long childId = 0;
        ChildInfo child = ChildSystem.createChild(consort, travelRef.isGiftde(), null, context);
		if(null != child)
		{
			childId = child.getChildId();
		}
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_006_RetCallAppoint(addCharmPoint, childId));

		userData.getRecordComponent().addRecord(ENPPlayerRecordParam.CONSORT_CALL_TIMES, 1, context);
        
        //获得子嗣事件
        Event_P_GAIN_CHILD evt = new Event_P_GAIN_CHILD(context, 1);
        userData.onLogicEvent(evt);

        //日志
        Opt015006ConsortAppointBO optBo = new Opt015006ConsortAppointBO();
        optBo.setConsortId(getUSServer().getBM(), consort.getConsortId());
        optBo.setConsortTravelId(getUSServer().getBM(), _msg.getConsortTravelId());
        optBo.setTravelCount(getUSServer().getBM(), calTravelCount);
        optBo.setAddCharmPoint(getUSServer().getBM(), addCharmPoint);
        optBo.setChildId(getUSServer().getBM(), childId);
        userData.logEvent(optBo, context);
    }
}