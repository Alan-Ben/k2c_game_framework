package  NPUSServer.NPUserMsgDispather.p010_BuildingOp;
import GC2GS.p010_BuildingOp.GC2GS_010_002_ReqFarmUpgradeLvl;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Building.RefFarmingBuilding;
import NPGameRes.Refs.Building.RefFarmingBuildingLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingFarmFunc;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
public class  MsgDealer_GC2GS_010_002_ReqFarmUpgradeLvl extends NPUserMsgDealer<GC2GS_010_002_ReqFarmUpgradeLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_010_002_ReqFarmUpgradeLvl _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        //获取建筑数据
        BuildingInfo info = userData.getBuildingComponent().lookupBuilding(_msg.getBuildingId());
        if(null == info)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NOT_EXIST.getCode());
        	return;
        }
        
        //检查是否农场建筑
        BuildingFarmFunc farmFunc = info.getFarm();
        if(null == farmFunc)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NO_TYPE.getCode());
        	return;
        }
        
        //检查满级情况
        RefFarmingBuilding refFarmingBuilding = farmFunc.getRef();
        if (refFarmingBuilding.building_level_max <= farmFunc.getLevel())
        {
            _commiter.commitFailRes(BuildingErr.BUILDING_LVL_FULL.getCode());
            return;
        }

        //获取当前等级配置
        RefFarmingBuildingLevel curLvlRef = farmFunc.getLvlRef();

        RefFarmingBuildingLevel nextLvlRef = farmFunc.getRef().getLevelMapMgr().getLevelData(farmFunc.getLevel() + 1);
        if(null == nextLvlRef)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_LVL_FULL.getCode());
        	return;
        }

        //检查消耗
        long costNum = curLvlRef.upgrade_cost_num + (long) curLvlRef.upgrade_cost_num_per_level * farmFunc.getLevelGap();
        if (!userData.hasItem(farmFunc.getRef().upgrade_cost_item, costNum))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }
        
        //升级操作
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUILDING_UPGRADE_LVL);
        //消耗物品
        if(!userData.spendItem(farmFunc.getRef().upgrade_cost_item, costNum, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        //设置等级
        farmFunc.setLvl(nextLvlRef, farmFunc.getLevel() + 1, context);
        
        _commiter.commitSucRes(US2GCWriter_010_BuildingOp.make_002_RetFarmUpgradeLvl());
    }
}