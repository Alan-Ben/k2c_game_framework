package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_008_ReqConfirmUpgradeBuildingLvl;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039008MarsBuildingConfirmUpgradeBO;

public class MsgDealer_GC2GS_039_008_ReqConfirmUpgradeBuildingLvl extends NPUserMsgDealer<GC2GS_039_008_ReqConfirmUpgradeBuildingLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_008_ReqConfirmUpgradeBuildingLvl _msg)
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
        
        if(!info.isUnlock())
        {
        	_commiter.commitFailRes(MarsErr.MARS_BUILDING_NOT_BUILD.getCode());
        	return;
        }

        int preLvl = info.getBuildingLvl();
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_UPGRADE_CONFIRM);
        Result result = info.doneUpgrade(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_008_RetConfirmUpgradeBuildingLvl());

        //日志数据
        Opt039008MarsBuildingConfirmUpgradeBO optBo = new Opt039008MarsBuildingConfirmUpgradeBO();
        optBo.setBuildingId(getUSServer().getBM(), _msg.getBuildingId());
        optBo.setPreLvl(getUSServer().getBM(), preLvl);
        optBo.setCurLvl(getUSServer().getBM(), info.getBuildingLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
