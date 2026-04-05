package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_016_ReqNotifyRoleGameEvent;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_016_RetNotifyRoleGameEvent;

public class RequestDealer_NP2US_R_003_016_ReqNotifyRoleGameEvent extends _ABasicGeneralRequestDealer<NP2US_R_003_016_ReqNotifyRoleGameEvent>
{
    public RequestDealer_NP2US_R_003_016_ReqNotifyRoleGameEvent(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_016_ReqNotifyRoleGameEvent _msg)
    {
        if (_msg.getType().equals("survey"))
        {
            Result result = getUSServer().getQuestionnaireMgr().addRecord(_msg.getCid(), _msg.getActivityCode());
            if (!result.isSucc())
            {
                _committer.commitFailRes(result.getCode());
            }

            _committer.commitSucRes(new NP2US_RB_003_016_RetNotifyRoleGameEvent());
        } else
        {
            _committer.commitFailRes(HttpErr.NOTIFY_ROLE_GAME_EVENT_TYPE_NOT_FOUND.getCode());
        }
    }
}
