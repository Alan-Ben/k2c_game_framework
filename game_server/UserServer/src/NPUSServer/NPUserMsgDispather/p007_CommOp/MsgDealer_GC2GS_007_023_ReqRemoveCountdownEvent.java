package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_023_ReqRemoveCountdownEvent;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_023_ReqRemoveCountdownEvent extends NPUserMsgDealer<GC2GS_007_023_ReqRemoveCountdownEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_023_ReqRemoveCountdownEvent _msg)
    {
    	NPUSUserData userData = _commiter.getUserData();

		Result result = userData.getCountdownEventComponent().removeOngoingEvent(NPPlayerContext.createNew(ENPGameEvent.COUNT_DOWN_EVENT_DONE));
		if (!result.isSucc())
		{
			_commiter.commitFailRes(result.getCode());
			return;
		}

		_commiter.commitSucRes(US2GCWriter_007_CommOp.make_023_RetRemoveCountdownEvent());
    }
}
