package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import NPUSServer.RankingEvent.USRankingEventRecordCallbackMgr;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_007_ReqRankingEventTrigger;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_007_RetRankingEventTrigger;

public class RequestDealer_NP2US_R_003_007_ReqRankingEventTrigger extends _ABasicGeneralRequestDealer<NP2US_R_003_007_ReqRankingEventTrigger>
{
    public RequestDealer_NP2US_R_003_007_ReqRankingEventTrigger(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_007_ReqRankingEventTrigger _msg)
    {
        USRankingEventRecordCallbackMgr.getInstance().onRankingEvent(_msg.getDealSerial(), _msg.getCid(), _msg.getScoreSourceId(), _msg.getChgValue());
        _committer.commitSucRes(new NP2US_RB_003_007_RetRankingEventTrigger());
    }
}
