package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import Common.BuildingEnum.EBuildingFuncEnum;
import GC2GS.p004_PlayerOp.GC2GS_004_025_ReqHireEmployee;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefHire;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function._ABuildingFunc;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_025_ReqHireEmployee extends NPUserMsgDealer<GC2GS_004_025_ReqHireEmployee>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_025_ReqHireEmployee _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        //如果已经处理过了 直接返回成功
        if (userData.getRecordComponent().getRecordCount(ENPPlayerRecordParam.DEAL_HIRE_EMPLOYEE_PLOT) != 0)
        {
            //返回成功
            _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_025_RetHireEmployee(_msg.getNum()));
            return;
        }

        //检查传入的员工数量是否超过限制
        if (_msg.getNum() > RefHire.getMgr().getList().size())
        {
            _commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
            return;
        }

        //检查建筑是否存在
        BuildingInfo buildingInfo = userData.getBuildingComponent().lookupBuilding(_msg.getBuildingId());
        if (buildingInfo == null)
        {
            _commiter.commitFailRes(BuildingErr.BUILDING_NOT_EXIST.getCode());
            return;
        }

        _ABuildingFunc buildingFunc = buildingInfo.getBuildingFunc(EBuildingFuncEnum.BUSINESS);
        BuildingBusinessFunc businessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
        if (businessFunc == null)
        {
            _commiter.commitFailRes(BuildingErr.BUILDING_NOT_EXIST.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DEAL_HIRE_EMPLOYEE_PLOT);
        businessFunc.addEmployeeCount(_msg.getNum(), context);

        userData.getRecordComponent().setRecord(ENPPlayerRecordParam.DEAL_HIRE_EMPLOYEE_PLOT, 1, context);

        //返回成功
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_025_RetHireEmployee(_msg.getNum()));
    }
}
