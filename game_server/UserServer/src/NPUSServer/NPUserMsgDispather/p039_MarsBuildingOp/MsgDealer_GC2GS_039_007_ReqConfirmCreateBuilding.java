package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_007_ReqConfirmCreateBuilding;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039007MarsBuildingConfirmCreateBO;

public class MsgDealer_GC2GS_039_007_ReqConfirmCreateBuilding extends NPUserMsgDealer<GC2GS_039_007_ReqConfirmCreateBuilding>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_007_ReqConfirmCreateBuilding _msg)
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
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_CREATE_CONFIRM);
        Result result = info.doneUpgrade(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_007_RetConfirmCreateBuilding());

        //日志数据
        Opt039007MarsBuildingConfirmCreateBO optBo = new Opt039007MarsBuildingConfirmCreateBO();
        optBo.setBuildingId(getUSServer().getBM(), _msg.getBuildingId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
