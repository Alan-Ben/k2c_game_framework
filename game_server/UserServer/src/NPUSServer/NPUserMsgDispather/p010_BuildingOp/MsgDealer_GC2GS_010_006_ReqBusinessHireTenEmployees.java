package  NPUSServer.NPUserMsgDispather.p010_BuildingOp;

import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import GC2GS.p010_BuildingOp.GC2GS_010_006_ReqBusinessHireTenEmployees;
import MJLog.MJEventLog;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Building.RefBusinessBuildingHireCost;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_BUSSINESS_HIRE_EMPLOYEE;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
import NPUSServer.USLog;
public class  MsgDealer_GC2GS_010_006_ReqBusinessHireTenEmployees extends NPUserMsgDealer<GC2GS_010_006_ReqBusinessHireTenEmployees>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_010_006_ReqBusinessHireTenEmployees _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        //获取建筑数据
        BuildingInfo info = userData.getBuildingComponent().lookupBuilding(_msg.getBuildingId());
        if(null == info)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NOT_EXIST.getCode());
        	return;
        }
        
        //检查是否经营建筑
        BuildingBusinessFunc businessFunc = info.getBusiness();
        if(null == businessFunc)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NO_TYPE.getCode());
        	return;
        }

        int beforeEmployeeCount = businessFunc.getEmployeeCount();
        int curCostHireCount = beforeEmployeeCount;

        //计划招聘数量
        int planHireCount = 10;

        //计算实际招募的员工数量
        int canAllowHireCount = businessFunc.getMaxEmployeeCount() - curCostHireCount;
        int realHireCount = Math.min(canAllowHireCount, planHireCount);
        if(realHireCount <= 0)
        {
        	_commiter.commitFailRes(BuildingErr.BUSINESS_EMPLOYEE_FULL.getCode());
        	return;
        }

        //计算减少的万分比
        long costReducePer = info.getBonusPropertyValue(EBonusPropertyType.BUILDING_EMPLOYEE_REDUCE_PER);
        
        long curCostNum = 0;

        for(int i = 0; i < realHireCount; i++)
        {
        	curCostHireCount++;

            RefBusinessBuildingHireCost costRef = RefBusinessBuildingHireCost.getMgr().getHireCostMgr().getLevelData(curCostHireCount);
            if(null == costRef)
            {
                USLog.error(getUSServer(), "player:{} hireNum:{} get building business hire cost ref fail, not find ref.", userData.getCid(), curCostHireCount);
                _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
                return;
            }

            long costNum = costRef.calCostNum(curCostHireCount, businessFunc.getRef().hire_cost_multiple);

        	curCostNum += (long) Math.ceil(1.0 * costNum * (1 - costReducePer / 10000f));
        }
        
        if(!userData.hasItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), curCostNum))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        //雇佣人数
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUILDING_BUSINESS_HIRE);
        //消耗物品
        if(!userData.spendItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), curCostNum, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        //增加雇佣人数
        businessFunc.setEmployeeCount(curCostHireCount, context);
        
        _commiter.commitSucRes(US2GCWriter_010_BuildingOp.make_006_RetBusinessHireTenEmployees());

        userData.onLogicEvent(new Event_P_BUSSINESS_HIRE_EMPLOYEE(context, realHireCount));

        //MJ日志
        MJEventLog.logBuildUpdate(userData, _msg.getBuildingId(), businessFunc.getLvl(),
                1, beforeEmployeeCount, curCostHireCount);
    }
}