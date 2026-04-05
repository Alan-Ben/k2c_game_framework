package PayCenter.PayServer.GeneralListener.RequestDispather.p001_PayOp;

import Common.ServerObj.ServerObj_PayCallbackInfo;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import PayCenter.PayCallback.PayCallbackMgr;
import PayCenter.PayServer.GeneralListener.RequestDispather.PayServerRequestCommitter;
import PayCenter.PayServer.PayServer;
import ToPay_R.p001_PayOp.ToPay_R_001_001_GetPayCallbackList;
import ToPay_RB.p001_PayOp.ToPay_RB_001_001_GetPayCallbackList;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;

public class RequestDealer_ToPay_R_001_001_GetPayCallbackList extends NPRequestDealer<ToPay_R_001_001_GetPayCallbackList>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, ToPay_R_001_001_GetPayCallbackList _msg)
    {
        PayServerRequestCommitter receiver = (PayServerRequestCommitter) _receiver;
        PayServer payServer = receiver.getPayServer();

        ArrayList<ServerObj_PayCallbackInfo> callbackList
                = PayCallbackMgr.getInstance().makeCallbackListByUs(payServer.getConf().getPlatformId(), payServer.getConf().getPlatAreaId(), _msg.getUsId());

        _receiver.commitSucRes(new ToPay_RB_001_001_GetPayCallbackList(callbackList));

    }
}
