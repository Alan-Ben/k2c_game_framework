package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_011_ReqCancelBuildingUpgrade;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039011MarsBuildingCancelUpgradeBO;

public class MsgDealer_GC2GS_039_011_ReqCancelBuildingUpgrade extends NPUserMsgDealer<GC2GS_039_011_ReqCancelBuildingUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_011_ReqCancelBuildingUpgrade _msg)
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
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_UPGRADE_CANCEL);
        Result result = userData.getMarsBuildingComponent().getBuildingUpQueueMgr().cancelUpgrade(_msg.getBuildingId(), context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_011_RetCancelBuildingUpgrade());

        //日志数据
        Opt039011MarsBuildingCancelUpgradeBO optBo = new Opt039011MarsBuildingCancelUpgradeBO();
        optBo.setBuildingId(getUSServer().getBM(), _msg.getBuildingId());
        optBo.setBuildingLvl(getUSServer().getBM(), info.getBuildingLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
