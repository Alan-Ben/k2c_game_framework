package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_024_ReqResetCountdownEvent;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_024_ReqResetCountdownEvent extends NPUserMsgDealer<GC2GS_007_024_ReqResetCountdownEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_024_ReqResetCountdownEvent _msg)
    {
    	NPUSUserData userData = _commiter.getUserData();

		Result result = userData.getCountdownEventComponent().resetDuration();
		if (!result.isSucc())
		{
			_commiter.commitFailRes(result.getCode());
			return;
		}

		_commiter.commitSucRes(US2GCWriter_007_CommOp.make_024_RetResetCountdownEvent());
    }
}
