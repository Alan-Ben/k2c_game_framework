package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_016_ReqCancelApplyToPlayer;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014016AdultCancelApplyBO;

public class  MsgDealer_GC2GS_014_016_ReqCancelApplyToPlayer extends NPUserMsgDealer<GC2GS_014_016_ReqCancelApplyToPlayer>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_016_ReqCancelApplyToPlayer _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_CANCEL_PLAYER_APPLY);
        
        Result res = userData.getChildComponent().getAdultMgr().cancelApplyToPlayer(_msg.getAdultId(), context);
        if(!res.isSucc())
        {
        	_commiter.commitFailRes(res.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_016_RetCancelApplyToPlayer());

        //日志
        Opt014016AdultCancelApplyBO optBo = new Opt014016AdultCancelApplyBO();
        optBo.setAdultId(getUSServer().getBM(), _msg.getAdultId());
        userData.logEvent(optBo, context);
    }
}