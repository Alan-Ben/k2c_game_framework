package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import NPUSServer.RankPlayerDataMgr.RankFixedObjDataInfo;
import NPUSServer.RankPlayerDataMgr.RankFixedObjDataList;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_005_ReqRankFixedLikeScore;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_005_RetRankFixedLikeScore;

public class RequestDealer_NP2US_R_003_005_ReqRankFixedLikeScore extends _ABasicGeneralRequestDealer<NP2US_R_003_005_ReqRankFixedLikeScore>
{
    public RequestDealer_NP2US_R_003_005_ReqRankFixedLikeScore(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_005_ReqRankFixedLikeScore _msg)
    {
        RankFixedObjDataList rankFixedList = getUSServer().getRankFixedObjDataListMgr().lookupObj(_msg.getRankFixedId());
        if (rankFixedList == null)
        {
            _committer.commitSucRes(new NP2US_RB_003_005_RetRankFixedLikeScore(0));
            return;
        }

        RankFixedObjDataInfo dataInfo = rankFixedList.lookupPlayer(_msg.getKey());
        if (dataInfo == null)
        {
            _committer.commitSucRes(new NP2US_RB_003_005_RetRankFixedLikeScore(0));
            return;
        }

        _committer.commitSucRes(new NP2US_RB_003_005_RetRankFixedLikeScore(dataInfo.getLikeScore(_msg.getIsCross())));
    }
}
