package NPUSServer.SynTask;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALTask._IALSynTask;
import Common.ServerObj.ServerObj_PayCallbackInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import ToPay_R.p001_PayOp.ToPay_R_001_001_GetPayCallbackList;
import ToPay_R.p001_PayOp.ToPay_R_001_002_NotifyPayCallbackHadPush;
import ToPay_RB.p001_PayOp.ToPay_RB_001_001_GetPayCallbackList;
import ToPay_RB.p001_PayOp.ToPay_RB_001_002_NotifyPayCallbackHadPush;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;

/**
 * 用于获取指定服务器的支付回调列表
 */
public class USGetPayCallbackListTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public USGetPayCallbackListTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer()
    {
        return _m_server;
    }

    @Override
    public void run()
    {
        final USGetPayCallbackListTask self = this;

        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.PAY.ordinal(), new ToPay_R_001_001_GetPayCallbackList(getUSServer().getServerTypeId()), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new ToPay_RB_001_001_GetPayCallbackList();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        ToPay_RB_001_001_GetPayCallbackList ret = (ToPay_RB_001_001_GetPayCallbackList) _retProto;

                        if (ret.getPayCallbackList().isEmpty())
                            return;

                        ArrayList<Long> dbIdList = new ArrayList<>();

                        NPPlayerContext context = NPPlayerContext.createNew(NPEnum.ENPGameEvent.PLAT_NOTIFY_ORDER_PAY);
                        for (ServerObj_PayCallbackInfo callbackInfo : ret.getPayCallbackList())
                        {
                            OfflineRewardFunc.onPlayerOrderPay(getUSServer(), callbackInfo, context);
                            dbIdList.add(callbackInfo.getDbId());
                        }

                        notifyPayCallbackHadPush(dbIdList);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(_m_server, "NPUserServer USGetPayCallbackListTask deal fail err:{}", _errCode);
                    }
                });
    }

    /**
     * 通知支付服已拉取到回调列表
     */
    private void notifyPayCallbackHadPush(ArrayList<Long> _dbIdList)
    {
        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.PAY.ordinal(), new ToPay_R_001_002_NotifyPayCallbackHadPush(_dbIdList), new _IWCGCallbackDealer()
                {

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new ToPay_RB_001_002_NotifyPayCallbackHadPush();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {

                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(_m_server, "NPUserServer USGetPayCallbackListTask notifyPayCallbackHadPush deal fail, dbIdList:{} err:{}"
                                , CommonFunc.toStringList(_dbIdList), _errCode);
                    }
                });
    }
}
