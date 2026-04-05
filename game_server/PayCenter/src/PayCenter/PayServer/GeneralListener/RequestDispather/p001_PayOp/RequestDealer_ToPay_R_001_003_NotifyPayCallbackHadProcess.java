package PayCenter.PayServer.GeneralListener.RequestDispather.p001_PayOp;

import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import PayCenter.PayCallback.PayCallbackMgr;
import ToPay_R.p001_PayOp.ToPay_R_001_003_NotifyPayCallbackHadProcess;
import ToPay_RB.p001_PayOp.ToPay_RB_001_003_NotifyPayCallbackHadProcess;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_ToPay_R_001_003_NotifyPayCallbackHadProcess extends NPRequestDealer<ToPay_R_001_003_NotifyPayCallbackHadProcess>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, ToPay_R_001_003_NotifyPayCallbackHadProcess _msg)
    {
        PayCallbackMgr.getInstance().notifyHadProcessedData(_msg.getDbId(), _msg.getIsDeliverySuccess());

        _receiver.commitSucRes(new ToPay_RB_001_003_NotifyPayCallbackHadProcess());
    }
}
