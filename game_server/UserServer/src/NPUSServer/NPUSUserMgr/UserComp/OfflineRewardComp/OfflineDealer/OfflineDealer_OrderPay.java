package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.ServerObj.ServerObj_PayCallbackInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.NPUSUserMgr.UserComp.OrderComp.OrderComponent;
import NPUSServer.USLog;
import ToPay_R.p001_PayOp.ToPay_R_001_003_NotifyPayCallbackHadProcess;
import ToPay_RB.p001_PayOp.ToPay_RB_001_003_NotifyPayCallbackHadProcess;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 发送宴会主人结算数据
 * @author mj
 *
 */
public class OfflineDealer_OrderPay extends _AOfflineDataDealer
{
	@Override
	public EOfflineRewardEnum getEnum()
	{
		return EOfflineRewardEnum.ORDER_PAY;
	}

	@Override
	public boolean isValid()
	{
		return true;
	}

	@Override
	public boolean syncToClient()
	{
		return false;
	}

	@Override
	public void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
	{
		if(null == _info.getOfflineData())
		{
			return;
		}

		try
		{
			ServerObj_PayCallbackInfo obj = new ServerObj_PayCallbackInfo();
			obj.readPackage(_info.getOfflineData());

			OrderComponent orderComponent = _info.getUserData().getOrderComponent();

			//订单支付通知
			orderComponent.platNotifyPay(obj, true, _context);

            //通知支付服该回调数据已经被处理
			notifyPayCallbackHadProcess(_info.getUserData(), obj.getDbId(),
					orderComponent.isOrderDeliverySuccess(obj.getOrderId(), obj.getSdkOrderId()));
		}
		catch(Exception ex)
		{
			USLog.error(_info.getUserData().getUSServer(), "", ex);
		}
	}

    /**
     * 通知支付服该回调数据已经被处理
     */
    public static void notifyPayCallbackHadProcess(NPUSUserData _userData, long _callbackDbId, boolean _isDeliverySuccess)
    {
        _userData.getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.PAY.ordinal(),
                new ToPay_R_001_003_NotifyPayCallbackHadProcess(_callbackDbId, _isDeliverySuccess),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new ToPay_RB_001_003_NotifyPayCallbackHadProcess();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {

                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.warn(_userData.getUSServer(), "NotifyPayCallbackHadProcess failed, cid:{} callbackDbId:{} isDeliverySuccess:{}",
                                _userData.getCid(), _callbackDbId, _isDeliverySuccess);
                    }
                });
    }


}
