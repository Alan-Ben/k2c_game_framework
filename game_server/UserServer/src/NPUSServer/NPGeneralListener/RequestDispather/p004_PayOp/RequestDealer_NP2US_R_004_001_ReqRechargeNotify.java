package NPUSServer.NPGeneralListener.RequestDispather.p004_PayOp;

import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p004_PayOp.NP2US_R_004_001_ReqRechargeNotify;
import WCGCS2US_RB.p004_PayOp.NP2US_RB_004_001_RetRechargeNotify;

/**
 * 充值通知请求处理器
 * 处理PayCenter发送的充值成功通知
 */
public class RequestDealer_NP2US_R_004_001_ReqRechargeNotify extends _ABasicGeneralRequestDealer<NP2US_R_004_001_ReqRechargeNotify>
{
    public RequestDealer_NP2US_R_004_001_ReqRechargeNotify(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_004_001_ReqRechargeNotify _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAT_NOTIFY_ORDER_PAY);

        OfflineRewardFunc.onPlayerOrderPay(getUSServer(), _msg.getPayCallbackInfo(), context);

        _committer.commitSucRes(new NP2US_RB_004_001_RetRechargeNotify());
    }

}