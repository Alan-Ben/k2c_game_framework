package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_011_ReqAgreeToMeApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014011AdultAgreeApplyBO;

public class  MsgDealer_GC2GS_014_011_ReqAgreeToMeApply extends NPUserMsgDealer<GC2GS_014_011_ReqAgreeToMeApply>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_011_ReqAgreeToMeApply _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_AGREE_PLAYER_APPLY);
        
        userData.getChildComponent().getAdultMgr().agreeApplyAdult(_msg.getAdultId(), _msg.getApplyAdultId(), context
        		, _errCode -> 
        		{
        			if(_errCode > 0)
        			{
        				_commiter.commitFailRes(_errCode);
        			}
        			else
        			{
        				_commiter.commitSucRes(US2GCWriter_014_ChildOp.make_011_RetAgreeToMeApply());

                        //日志
                        Opt014011AdultAgreeApplyBO optBo = new Opt014011AdultAgreeApplyBO();
                        optBo.setAdultId(getUSServer().getBM(), _msg.getAdultId());
                        optBo.setApplyAdultId(getUSServer().getBM(), _msg.getApplyAdultId());
                        userData.logEvent(optBo, context);
        			}
        		});
    }
}