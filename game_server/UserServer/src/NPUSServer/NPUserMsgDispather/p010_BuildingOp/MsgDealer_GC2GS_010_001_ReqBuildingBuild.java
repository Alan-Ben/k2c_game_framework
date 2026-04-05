package  NPUSServer.NPUserMsgDispather.p010_BuildingOp;

import GC2GS.p010_BuildingOp.GC2GS_010_001_ReqBuildingBuild;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Building.RefBuilding;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
public class  MsgDealer_GC2GS_010_001_ReqBuildingBuild extends NPUserMsgDealer<GC2GS_010_001_ReqBuildingBuild>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_010_001_ReqBuildingBuild _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        //建筑已经存在
        if(userData.getBuildingComponent().hasBuilding(_msg.getBuildingId()))
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_EXISTED.getCode());
        	return;
        }
        
        //检查配置
        RefBuilding ref = RefBuilding.getMgr().get(_msg.getBuildingId());
        if(null == ref)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        //检查条件
        if(!NPPlayerConditionDealerMgr.IsEnable(ref.build_condition, userData, null))
        {
        	_commiter.commitFailRes(CommErr.CONDITION_NOT_ENABLE.getCode());
        	return;
        }
        
        //检查消耗
        if(!userData.hasItem(ref.build_cost))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        //开启消耗
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUILDING_BUILD);
        
        if(!userData.spendItem(ref.build_cost, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        
        //创建建筑
        BuildingInfo info = userData.getBuildingComponent().create(_msg.getBuildingId(), context);
        if(null == info)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_BUILD_FAIL.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_010_BuildingOp.make_001_RetBuildingBuild());
    }
}