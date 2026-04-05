package NPUSServer.NPUserMsgDispather.p038_MarsOp;

import GC2GS.p038_MarsOp.GC2GS_038_003_ReqSendDoneMarquee;
import NPCommon.ErrMain.MarsErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_038_MarsOp;

public class MsgDealer_GC2GS_038_003_ReqSendDoneMarquee extends NPUserMsgDealer<GC2GS_038_003_ReqSendDoneMarquee>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_038_003_ReqSendDoneMarquee _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        if(!userData.getMarsGoRouteComponent().isAllDone())
        {
        	_commiter.commitFailRes(MarsErr.MARS_GO_ROUTE_NOT_DONE.getCode());
        	return;
        }
        
        userData.getMarsGoRouteComponent().sendDoneMarquee();
        
        _commiter.commitSucRes(US2GCWriter_038_MarsOp.make_003_RetSendDoneMarquee());
    }
}
