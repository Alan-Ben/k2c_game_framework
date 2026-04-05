package NPUSServer.NPUserMsgDispather.p038_MarsOp;

import GC2GS.p038_MarsOp.GC2GS_038_002_ReqArriveMarsStage;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_038_MarsOp;
import USLOGDB.OptBo.Opt038002MarsRouteArriveBO;

public class MsgDealer_GC2GS_038_002_ReqArriveMarsStage extends NPUserMsgDealer<GC2GS_038_002_ReqArriveMarsStage>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_038_002_ReqArriveMarsStage _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //执行解锁操作
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_GO_ROUTE_DONE_STAGE);
        Result result = userData.getMarsGoRouteComponent().doneStage(_msg.getStage(), context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_038_MarsOp.make_002_RetArriveMarsStage(context));
        
        //日志数据
        Opt038002MarsRouteArriveBO optBo = new Opt038002MarsRouteArriveBO();
        optBo.setStage(getUSServer().getBM(), _msg.getStage());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
