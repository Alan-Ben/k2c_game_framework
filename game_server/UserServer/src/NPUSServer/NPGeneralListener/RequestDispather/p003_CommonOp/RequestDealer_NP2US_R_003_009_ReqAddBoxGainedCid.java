package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_003_CommonOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_009_ReqAddBoxGainedCid;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_009_RetAddBoxGainedCid;

public class RequestDealer_NP2US_R_003_009_ReqAddBoxGainedCid extends _ABasicGeneralRequestDealer<NP2US_R_003_009_ReqAddBoxGainedCid>
{
    public RequestDealer_NP2US_R_003_009_ReqAddBoxGainedCid(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_009_ReqAddBoxGainedCid _msg)
    {
        CommBoxInfo boxInfo = getUSServer().getCommBoxMgr().lookupBox(_msg.getInstanceId());
        if (null == boxInfo)
        {
            _committer.commitSucRes(new NP2US_RB_003_009_RetAddBoxGainedCid());
            return;
        }

        _committer.commitSucRes(NP2US_RB_Writer_003_CommonOp.make_009_RetAddBoxGainedCid(boxInfo.setGainedCid(_msg.getCid())
                , boxInfo.getRef().id, boxInfo.checkStatus(_msg.getCid()), boxInfo.getGainedCidList()));
    }
}
