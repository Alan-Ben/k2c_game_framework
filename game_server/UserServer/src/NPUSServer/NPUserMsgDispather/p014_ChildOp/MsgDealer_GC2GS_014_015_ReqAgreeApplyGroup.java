package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_015_ReqAgreeApplyGroup;
import NPCommon.ErrMain.ChildErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014015AdultAgreeGroupBO;

public class  MsgDealer_GC2GS_014_015_ReqAgreeApplyGroup extends NPUserMsgDealer<GC2GS_014_015_ReqAgreeApplyGroup>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_015_ReqAgreeApplyGroup _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //获取玩家匹配的子嗣数据
        UnmarryAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupUnmarryAdult(_msg.getAdultId());
        if(null == adult)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_EXISTS.getCode());
        	return;
        }
        
        //发起同意联姻池子嗣操作
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_AGREE_POOL_APPLY);
        userData.getChildComponent().getAdultMgr().agreeMatchAdult(_msg.getAdultId(), _msg.getApplyCid(), _msg.getApplyAdultId(), context, _errCode ->
        {
        	if(_errCode > 0)
        	{
        		_commiter.commitFailRes(_errCode);
        	}
        	else
        	{
        		_commiter.commitSucRes(US2GCWriter_014_ChildOp.make_015_RetAgreeApplyGroup());

                //日志
                Opt014015AdultAgreeGroupBO optBo = new Opt014015AdultAgreeGroupBO();
                optBo.setAdultId(getUSServer().getBM(), _msg.getAdultId());
                optBo.setApplyCid(getUSServer().getBM(), _msg.getApplyCid());
                optBo.setApplyAdultId(getUSServer().getBM(), _msg.getApplyAdultId());
                userData.logEvent(optBo, context);
        	}
        });
    }
}