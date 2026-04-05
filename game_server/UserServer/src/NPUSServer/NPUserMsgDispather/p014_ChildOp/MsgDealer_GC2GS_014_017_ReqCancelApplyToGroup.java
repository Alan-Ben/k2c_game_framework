package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_017_ReqCancelApplyToGroup;
import NPCommon.ErrMain.ChildErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014017AdultCancelGroupBO;

public class  MsgDealer_GC2GS_014_017_ReqCancelApplyToGroup extends NPUserMsgDealer<GC2GS_014_017_ReqCancelApplyToGroup>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_017_ReqCancelApplyToGroup _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查子嗣数据
        UnmarryAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupUnmarryAdult(_msg.getAdultId());
        if(null == adult)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_EXISTS.getCode());
        	return;
        }
        //检查子嗣状态
        if(!adult.isApplyServer())
        {
        	_commiter.commitFailRes(ChildErr.ADULT_NOT_IN_POOL.getCode());
        	return;
        }
        
        //移除联姻池数据
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_CANCEL_POOL_APPLY);
        getUSServer().getMatchAdultPool().removeItem(_msg.getAdultId(), context);
        //设置子嗣为空闲状态
        adult.setIdle(false, context);
        
        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_017_RetCancelApplyToGroup());

        //日志
        Opt014017AdultCancelGroupBO optBo = new Opt014017AdultCancelGroupBO();
        optBo.setAdultId(getUSServer().getBM(), _msg.getAdultId());
        userData.logEvent(optBo, context);
    }
}