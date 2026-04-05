package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_021_ReqConfirmUpgradeTechnologyLvl;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039021MarsTechConfirmUpgradeBO;

public class MsgDealer_GC2GS_039_021_ReqConfirmUpgradeTechnologyLvl extends NPUserMsgDealer<GC2GS_039_021_ReqConfirmUpgradeTechnologyLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_021_ReqConfirmUpgradeTechnologyLvl _msg)
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
        
        int preLvl = info.getLvl();
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_TECH_UPGRADE_LVL_CONFIRM);
        Result result = info.doneUpgradeLvl(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	//推送最新科技数据，校准客户端数据
        	userData.sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_060_OnMarsTechnologyChg(info));
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_021_RetConfirmUpgradeTechnologyLvl());

        //日志数据
        Opt039021MarsTechConfirmUpgradeBO optBo = new Opt039021MarsTechConfirmUpgradeBO();
        optBo.setTechId(getUSServer().getBM(), _msg.getTechnologyId());
        optBo.setPreLvl(getUSServer().getBM(), preLvl);
        optBo.setCurLvl(getUSServer().getBM(), info.getLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
