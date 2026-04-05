package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;
import GC2GS.p015_ConsortOp.GC2GS_015_011_ReqGetCgUnlockReward;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortCGMgr.ConsortCGInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import USLOGDB.OptBo.Opt015011ConsortGainCgRewardBO;
public class  MsgDealer_GC2GS_015_011_ReqGetCgUnlockReward extends NPUserMsgDealer<GC2GS_015_011_ReqGetCgUnlockReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_011_ReqGetCgUnlockReward _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ConsortCGInfo cg = userData.getConsortComponent().getCGMgr().lookup(_msg.getCgId());
        if(null == cg)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_CG_NOT_UNLOCK.getCode());
        	return;
        }
        
        if(null == cg.getRef())
        {
        	_commiter.commitFailRes(CommErr.DATA_STATE_ERR.getCode());
        	return;
        }
        
        if(cg.isRewarded())
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_CG_HAD_REWARDED.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_GET_CG_REWARD);
        //设置领取标志位
        cg.setRewarded(context);
        //领取奖励
        userData.gainItemList(cg.getRef().cg_unlock_item_list, context);
        //推送通用奖励弹框
        if(!context.getCollector().isEmpty())
        {
        	userData.sendMsgToGC(context.getCollector().toProto());
        }
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_011_RetGetCgUnlockReward());
        
        //日志
        Opt015011ConsortGainCgRewardBO optBo = new Opt015011ConsortGainCgRewardBO();
        optBo.setCgId(getUSServer().getBM(), _msg.getCgId());
        userData.logEvent(optBo, context);
    }
}