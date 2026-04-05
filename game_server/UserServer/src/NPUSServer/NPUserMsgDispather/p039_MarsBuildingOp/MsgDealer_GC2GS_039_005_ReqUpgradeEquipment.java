package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_005_ReqUpgradeEquipment;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingEquipmentInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039005MarsBuildingEquipUpgradeBO;

public class MsgDealer_GC2GS_039_005_ReqUpgradeEquipment extends NPUserMsgDealer<GC2GS_039_005_ReqUpgradeEquipment>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_005_ReqUpgradeEquipment _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        MarsBuildingInfo info = userData.getMarsBuildingComponent().lookupBuilding(_msg.getBuildingId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_BUILDING_NOT_FOUND.getCode());
        	return;
        }
        
        MarsBuildingEquipmentInfo equip = info.getBuildingEquipmentMgr().lookup(_msg.getEquipmentId());
        if(null == equip)
        {
        	_commiter.commitFailRes(MarsErr.MARS_BUILDING_EQUIPMENT_NOT_FOUND.getCode());
        	return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_EQUIPMENT_UPGRADE);
        Result result = equip.upgrade(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_005_RetUpgradeEquipment());

        //日志数据
        Opt039005MarsBuildingEquipUpgradeBO optBo = new Opt039005MarsBuildingEquipUpgradeBO();
        optBo.setBuildingId(getUSServer().getBM(), _msg.getBuildingId());
        optBo.setEquipId(getUSServer().getBM(), _msg.getEquipmentId());
        optBo.setEquipId(getUSServer().getBM(), equip.getLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
