package  NPUSServer.NPUserMsgDispather.p010_BuildingOp;

import GC2GS.p010_BuildingOp.GC2GS_010_004_ReqBusinessUpgradeLvl;
import MJLog.MJEventLog;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Building.RefBusinessBuildingLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_BUSINESS_UPGRADE;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
public class  MsgDealer_GC2GS_010_004_ReqBusinessUpgradeLvl extends NPUserMsgDealer<GC2GS_010_004_ReqBusinessUpgradeLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_010_004_ReqBusinessUpgradeLvl _msg)
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

        //检查满级情况
        int nextLvl = info.getBusiness().getLvl() + 1;
        RefBusinessBuildingLevel nextLvlRef = info.getBusiness().getRef().getLevelMapMgr().getLevelData(nextLvl);
        if(null == nextLvlRef)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_LVL_FULL.getCode());
        	return;
        }

        RefBusinessBuildingLevel curLvlRef = info.getBusiness().getLvlRef();
        //检查消耗
        if(!userData.hasItem(curLvlRef.upgrade_cost_item))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }

        //升级操作
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUILDING_UPGRADE_LVL);

        int beforeLevel = info.getBusiness().getLvl();

        //消耗物品
        if(!userData.spendItem(curLvlRef.upgrade_cost_item, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        //设置等级
        info.getBusiness().setLvl(nextLvlRef, context);

        _commiter.commitSucRes(US2GCWriter_010_BuildingOp.make_004_RetBusinessUpgradeLvl());

        userData.onLogicEvent(new Event_P_BUSINESS_UPGRADE(context, _msg.getBuildingId()));

        //MJ日志
        MJEventLog.logBuildUpdate(userData, _msg.getBuildingId(), beforeLevel,
                2, beforeLevel, nextLvl);
    }
}