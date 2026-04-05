package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_013_ReqApplyToPlayer;
import NPCommon.ErrMain.ChildErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.Delegate.HandlerOne;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.MarryApplyInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014013AdultSendApplyBO;

public class  MsgDealer_GC2GS_014_013_ReqApplyToPlayer extends NPUserMsgDealer<GC2GS_014_013_ReqApplyToPlayer>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_013_ReqApplyToPlayer _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //不能对自身玩家发起请求
        if(_msg.getTargetCid() == userData.getCid())
        {
        	_commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
        	return;
        }
        
        //检查子嗣数据
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
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_SEND_PLAYER_APPLY);
        //构造请求数据
        MarryApplyInfo apply = adult.transToMarryApply(_msg.getTargetCid(), context);
        if(null == apply)
        {
        	_commiter.commitFailRes(CommErr.DATA_STATE_ERR.getCode());
        	return;
        }
        //发送联姻请求
        AdultMarrySystem.SendPlayerApply(apply, context, new HandlerOne<ResultOne<Boolean>>()
        {
            @Override
            public void handle(ResultOne<Boolean> _resultOne)
            {
                if (!_resultOne.isSucc())
                {
                    _commiter.commitFailRes(_resultOne.getCode());
                    return;
                }

                if (_resultOne.getData())
                {
                    adult.setApplyIdle(apply, context);
                }

                _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_013_RetApplyToPlayer(_resultOne.getData()));

                //日志
                Opt014013AdultSendApplyBO optBo = new Opt014013AdultSendApplyBO();
                optBo.setAdultId(getUSServer().getBM(), _msg.getAdultId());
                optBo.setTargetCid(getUSServer().getBM(), _msg.getTargetCid());
                userData.logEvent(optBo, context);
            }
        });
    }
}