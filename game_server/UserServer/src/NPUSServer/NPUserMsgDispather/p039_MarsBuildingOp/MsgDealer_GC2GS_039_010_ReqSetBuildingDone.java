package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_010_ReqSetBuildingDone;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039010MarsBuildingSetDoneBO;

public class MsgDealer_GC2GS_039_010_ReqSetBuildingDone extends NPUserMsgDealer<GC2GS_039_010_ReqSetBuildingDone>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_010_ReqSetBuildingDone _msg)
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

        int preLvl = info.getBuildingLvl();
        boolean isUpgrade = userData.getMarsBuildingComponent().getBuildingUpQueueMgr().isBuildingUp(_msg.getBuildingId());

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_SET_DONE);
        Result result = info.setBuildingDone(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_010_RetSetBuildingDone());

        //日志数据
        Opt039010MarsBuildingSetDoneBO optBo = new Opt039010MarsBuildingSetDoneBO();
        optBo.setBuildingId(getUSServer().getBM(), _msg.getBuildingId());
        optBo.setIsUpgrade(getUSServer().getBM(), isUpgrade);
        optBo.setPreLvl(getUSServer().getBM(), preLvl);
        optBo.setCurLvl(getUSServer().getBM(), info.getBuildingLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
