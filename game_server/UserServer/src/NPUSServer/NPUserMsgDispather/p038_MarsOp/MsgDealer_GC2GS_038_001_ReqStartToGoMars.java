package NPUSServer.NPUserMsgDispather.p038_MarsOp;

import GC2GS.p038_MarsOp.GC2GS_038_001_ReqStartToGoMars;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_038_MarsOp;
import USLOGDB.OptBo.Opt038001MarsRouteStartBO;

public class MsgDealer_GC2GS_038_001_ReqStartToGoMars extends NPUserMsgDealer<GC2GS_038_001_ReqStartToGoMars>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_038_001_ReqStartToGoMars _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //执行解锁操作
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_GO_ROUTE_START_STAGE);
        Result result = userData.getMarsGoRouteComponent().startStage(context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_038_MarsOp.make_001_RetStartToGoMars());
        
        //日志数据
        Opt038001MarsRouteStartBO optBo = new Opt038001MarsRouteStartBO();
        _commiter.getUserData().logEvent(optBo, context);
    }
}
