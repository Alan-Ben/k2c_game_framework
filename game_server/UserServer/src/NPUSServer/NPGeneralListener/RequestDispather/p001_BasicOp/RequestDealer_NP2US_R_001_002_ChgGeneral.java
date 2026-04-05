package NPUSServer.NPGeneralListener.RequestDispather.p001_BasicOp;

import NP2US_R.p001_BasicOp.NP2US_R_001_002_ChgGeneral;
import NPCommon.ErrMain.GameResErr;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_001_BasicOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_NP2US_R_001_002_ChgGeneral extends _ABasicGeneralRequestDealer<NP2US_R_001_002_ChgGeneral>
{
    public RequestDealer_NP2US_R_001_002_ChgGeneral(NPUserServer _server) {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_001_002_ChgGeneral _msg)
    {
        boolean bOK = RefGeneral.Ref().gmSetValue(_msg.getKey().trim(), _msg.getValue().trim());
        if (bOK)
        {
            RefGeneral.Ref().Assert();
        }

        if (bOK)
            _committer.commitSucRes(NP2US_RB_Writer_001_BasicOp.make_002_ChgGeneralRes(bOK));
        else
            _committer.commitFailRes(GameResErr.GAME_RES_GM_CHG_FAIL.getCode());
    }
}
