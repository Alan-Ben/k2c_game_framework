package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_022_ReqCancelTechnologyUpgrade;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039022MarsTechCancelUpgradeBO;

public class MsgDealer_GC2GS_039_022_ReqCancelTechnologyUpgrade extends NPUserMsgDealer<GC2GS_039_022_ReqCancelTechnologyUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_022_ReqCancelTechnologyUpgrade _msg)
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
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_TECH_UPGRADE_CANCEL);
        Result result = info.cancelUpgradeLvl(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_022_RetCancelTechnologyUpgrade());

        //日志数据
        Opt039022MarsTechCancelUpgradeBO optBo = new Opt039022MarsTechCancelUpgradeBO();
        optBo.setTechId(getUSServer().getBM(), _msg.getTechnologyId());
        optBo.setTechLvl(getUSServer().getBM(), info.getLvl());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
