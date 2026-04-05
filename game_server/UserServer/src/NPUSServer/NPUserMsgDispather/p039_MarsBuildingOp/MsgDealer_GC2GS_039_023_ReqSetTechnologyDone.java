package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_023_ReqSetTechnologyDone;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039023MarsTechSetlDoneBO;

public class MsgDealer_GC2GS_039_023_ReqSetTechnologyDone extends NPUserMsgDealer<GC2GS_039_023_ReqSetTechnologyDone>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_023_ReqSetTechnologyDone _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        MarsTechInfo info = userData.getMarsTechComponent().lookup(_msg.getTechnologyId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_TECH_NOT_FOUND.getCode());
        	return;
        }
        
        boolean isUpgrade = info.isUpgrading();
        int preLvl = info.getLvl();
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_TECH_SET_DONE);
        Result result = info.setDone(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_023_RetSetTechnologyDone());

        //日志数据
        Opt039023MarsTechSetlDoneBO optBo = new Opt039023MarsTechSetlDoneBO();
        optBo.setTechId(getUSServer().getBM(), _msg.getTechnologyId());
        optBo.setIsUpgrade(getUSServer().getBM(), isUpgrade);
        optBo.setPreLvl(getUSServer().getBM(), preLvl);
        optBo.setCurLvl(getUSServer().getBM(), info.getLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
