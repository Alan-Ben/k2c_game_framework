package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_014_ReqApplyToGroup;
import NPCommon.ErrMain.ChildErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.MatchAdultMgr.MatchAdultItem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014014AdultApplyGroupBO;

public class  MsgDealer_GC2GS_014_014_ReqApplyToGroup extends NPUserMsgDealer<GC2GS_014_014_ReqApplyToGroup>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_014_ReqApplyToGroup _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //参数检查
        if(_msg.getMinValue() < 0)
        {
        	_commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
        	return;
        }
      
        //检查对应未婚子嗣数据
        UnmarryAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupUnmarryAdult(_msg.getAdultId());
        if(null == adult)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_EXISTS.getCode());
        	return;
        }
        //检查子嗣状态
        if(!adult.isIdle())
        {
        	_commiter.commitFailRes(ChildErr.ADULT_NOT_IDLE.getCode());
        	return;
        }

        //进入全服联姻池状态
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_SEND_POOL_APPLY);
        MatchAdultItem matchItem = getUSServer().getMatchAdultPool().addItem(adult, _msg.getMinValue(), context);
        adult.setMatchItem(false, matchItem, context);
        
        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_014_RetApplyToGroup());

        //日志
        Opt014014AdultApplyGroupBO optBo = new Opt014014AdultApplyGroupBO();
        optBo.setAdultId(getUSServer().getBM(), adult.getAdultId());
        optBo.setBonus(getUSServer().getBM(), adult.getBonus());
        optBo.setMinBonus(getUSServer().getBM(), _msg.getMinValue());
        userData.logEvent(optBo, context);
    }
}