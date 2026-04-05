package PayCenter.PayServer.GeneralListener.RequestDispather.p001_PayOp;

import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import PayCenter.PayCallback.PayCallbackMgr;
import ToPay_R.p001_PayOp.ToPay_R_001_002_NotifyPayCallbackHadPush;
import ToPay_RB.p001_PayOp.ToPay_RB_001_002_NotifyPayCallbackHadPush;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class RequestDealer_ToPay_R_001_002_NotifyPayCallbackHadPush extends NPRequestDealer<ToPay_R_001_002_NotifyPayCallbackHadPush>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, ToPay_R_001_002_NotifyPayCallbackHadPush _msg)
    {
        PayCallbackMgr.getInstance().notifyHadGetData(_msg.getDbIdList());

        _receiver.commitSucRes(new ToPay_RB_001_002_NotifyPayCallbackHadPush());
    }
}
