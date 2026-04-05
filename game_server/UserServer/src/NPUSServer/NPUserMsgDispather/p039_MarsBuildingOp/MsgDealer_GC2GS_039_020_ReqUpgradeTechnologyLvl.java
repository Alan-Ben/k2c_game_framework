package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_020_ReqUpgradeTechnologyLvl;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039020MarsTechUpgradeBO;

public class MsgDealer_GC2GS_039_020_ReqUpgradeTechnologyLvl extends NPUserMsgDealer<GC2GS_039_020_ReqUpgradeTechnologyLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_020_ReqUpgradeTechnologyLvl _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查升级队列
        if(userData.getMarsTechComponent().getUpgradingSum() > 0)
        {
        	_commiter.commitFailRes(MarsErr.MARS_TECH_UPGRADING_LINES_FULL.getCode());
        	return;
        }
        
        //对应科技数据
        MarsTechInfo info = userData.getMarsTechComponent().lookup(_msg.getTechnologyId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_TECH_NOT_FOUND.getCode());
        	return;
        }
        
        //升级科技
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_TECH_UPGRADE_LVL);
        Result result = info.upgradeLvl(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_020_RetUpgradeTechnologyLvl());

        //日志数据
        Opt039020MarsTechUpgradeBO optBo = new Opt039020MarsTechUpgradeBO();
        optBo.setTechId(getUSServer().getBM(), _msg.getTechnologyId());
        optBo.setTechLvl(getUSServer().getBM(), info.getLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
