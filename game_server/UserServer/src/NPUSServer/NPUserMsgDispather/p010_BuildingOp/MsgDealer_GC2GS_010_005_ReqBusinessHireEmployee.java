package  NPUSServer.NPUserMsgDispather.p010_BuildingOp;

import CommonEnum.EBonusPropertyType;
import CommonEnum.ECurrency;
import GC2GS.p010_BuildingOp.GC2GS_010_005_ReqBusinessHireEmployee;
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
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
import USLOGDB.OptBo.Opt010005BuildingHireEmployeeBO;
public class  MsgDealer_GC2GS_010_005_ReqBusinessHireEmployee extends NPUserMsgDealer<GC2GS_010_005_ReqBusinessHireEmployee>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_010_005_ReqBusinessHireEmployee _msg)
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
        if(null == info.getBusiness())
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NO_TYPE.getCode());
        	return;
        }
        
        //检查员工最大数量
        if(!info.getBusiness().checkHireEmployeeCount())
        {
        	_commiter.commitFailRes(BuildingErr.BUSINESS_EMPLOYEE_FULL.getCode());
        	return;
        }

        int employeeCount = info.getBusiness().getEmployeeCount();
        int targetCount = employeeCount + 1;

        //计算消耗
        RefBusinessBuildingHireCost costRef = RefBusinessBuildingHireCost.getMgr().getHireCostMgr().getLevelData(targetCount);
        if(null == costRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUILDING_BUSINESS_HIRE);
        
        //计算消耗
        long costNum = costRef.calCostNum(targetCount, info.getBusiness().getRef().hire_cost_multiple);
        //计算减少的万分比
        long costReducePer = info.getBonusPropertyValue(EBonusPropertyType.BUILDING_EMPLOYEE_REDUCE_PER);
        long realCostNum = (long) Math.ceil(1.0 * costNum * (1 - costReducePer / 10000f));
        if(realCostNum > 0)
        {
        	if(!userData.hasItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), realCostNum))
            {
            	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            	return;
            }
            
            //消耗物品
            if(!userData.spendItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), realCostNum, context))
            {
            	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            	return;
            }
        }
        
        //增加雇佣人数
        info.getBusiness().setEmployeeCount(targetCount, context);
        
        _commiter.commitSucRes(US2GCWriter_010_BuildingOp.make_005_RetBusinessHireEmployee());

        userData.onLogicEvent(new Event_P_BUSSINESS_HIRE_EMPLOYEE(context, 1));

        //日志
        Opt010005BuildingHireEmployeeBO optBo = new Opt010005BuildingHireEmployeeBO();
        optBo.setBuildingId(getUSServer().getBM(), _msg.getBuildingId());
        optBo.setRealCostNum(getUSServer().getBM(), realCostNum);
        optBo.setCurHireCount(getUSServer().getBM(), targetCount);
        userData.logEvent(optBo, context);

        //MJ日志
        MJEventLog.logBuildUpdate(userData, _msg.getBuildingId(), info.getBusiness().getLvl(),
                1, employeeCount, targetCount);
    }
}