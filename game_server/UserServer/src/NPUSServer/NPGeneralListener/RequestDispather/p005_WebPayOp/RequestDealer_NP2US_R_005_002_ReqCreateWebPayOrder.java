package NPUSServer.NPGeneralListener.RequestDispather.p005_WebPayOp;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import Common.ServerObj.ServerObj_WebPayOrderList;
import NP2US_R.p005_WebPayOp.NP2US_R_005_002_ReqCreateWebPayOrder;
import NP2US_RB.p002_GSOp.NP2US_RB_002_001_RegUserGate;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ENPGameEvent;
import NPServerProtocolWriter.NP2US.Response.NP2US_RB_Writer_005_WebPayOp;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.NPUSUserMgr;
import NPUSServer.NPUSUserMgr.UserSafeCall._IUserLoadOverHandler;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;
import java.util.List;

/**
 * 创建网页支付订单请求处理器
 * <p>
 * 处理来自HttpServer的订单创建请求，支持批量创建
 */
public class RequestDealer_NP2US_R_005_002_ReqCreateWebPayOrder extends _ABasicGeneralRequestDealer<NP2US_R_005_002_ReqCreateWebPayOrder>
{
    public RequestDealer_NP2US_R_005_002_ReqCreateWebPayOrder(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2US_R_005_002_ReqCreateWebPayOrder _msg)
    {
        NPUSUserMgr usUserMgr = getUSServer().getUsUserMgr();

        // 直接从内存查找
        NPUSUserData userData = usUserMgr.lookupCacheUserData(_msg.getCid());
        if (userData != null)
        {
            _createOrder(_commiter, userData, _msg, false, 0);
        } else
        {
            // 查找玩家的UID
            getUSServer().getUserIdxMgr().lookupCidLinkedUid(_msg.getCid(), new _ICallBackResultT<String>()
            {
                @Override
                public void onRunOver(Result _result, String _uid)
                {
                    if (!_result.isSucc())
                    {
                        USLog.error(getUSServer(), "[WebPay] create web pay order failed, player cid:{} not found", _msg.getCid());
                        // 使用commitSucRes返回带错误码的协议
                        _commiter.commitSucRes(
                                NP2US_RB_Writer_005_WebPayOp.make_002_RetCreateWebPayOrderFailed(
                                        CommErr.PLAYER_NOT_FOUND.getCode(), null));
                        return;
                    }

                    usUserMgr.initUserData(_msg.getCid(), _uid, "", new _IWCGBasicRequestCommiter()
                    {
                        @Override
                        public _IALProtocolReceiver getRequestDealer()
                        {
                            return null;
                        }

                        @Override
                        public void commitSucRes(_IALProtocolStructure _proto)
                        {
                            ALSynTaskManager.getInstance().regTask(new _IALSynTask()
                            {
                                @Override
                                public void run()
                                {
                                    NP2US_RB_002_001_RegUserGate commitMsg = (NP2US_RB_002_001_RegUserGate) _proto;

                                    NPUSUserData loadedUserData = usUserMgr.lookupCacheUserData(_msg.getCid());
                                    if (loadedUserData == null)
                                    {
                                        USLog.error(getUSServer(), "[WebPay] create web pay order failed, player cid:{} data load failed", _msg.getCid());
                                        // 使用commitSucRes返回带错误码的协议
                                        _commiter.commitSucRes(
                                                NP2US_RB_Writer_005_WebPayOp.make_002_RetCreateWebPayOrderFailed(
                                                        CommErr.PLAYER_NOT_FOUND.getCode(), null));
                                        return;
                                    }

                                    long newSerialize = commitMsg.getNewSerialize();

                                    _createOrder(_commiter, loadedUserData, _msg, true, newSerialize);
                                }
                            });
                        }

                        @Override
                        public void commitFailRes(int _errCode)
                        {
                            USLog.error(getUSServer(), "[WebPay] create web pay order failed, player cid:{} data load failed", _msg.getCid());
                            // 使用commitSucRes返回带错误码的协议
                            _commiter.commitSucRes(
                                    NP2US_RB_Writer_005_WebPayOp.make_002_RetCreateWebPayOrderFailed(
                                            CommErr.PLAYER_NOT_FOUND.getCode(), null));
                        }
                    });
                }
            });
        }
    }

    /**
     * 创建订单
     * @param _commiter
     * @param _userData
     * @param _msg
     * @param _needOffload
     * @param _newSerialize
     */
    private void _createOrder(_IWCGBasicRequestCommiter _commiter, NPUSUserData _userData, NP2US_R_005_002_ReqCreateWebPayOrder _msg,
                              boolean _needOffload, long _newSerialize)
    {
        _userData.safeCall(new _IUserLoadOverHandler()
        {
            @Override
            public void onLoadOver()
            {
                NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.WEB_PAY_BATCH_CREATE_ORDER);

                List<Long> failedGoodsIds = new ArrayList<>();

                ResultOne<ServerObj_WebPayOrderList> webOrderBatch =
                        _userData.getOrderComponent().createWebOrderBatch(_msg.getGoodsIds(), failedGoodsIds, context);

                if (!webOrderBatch.getResult().isSucc())
                {
                    // 使用commitSucRes返回带错误码和失败商品ID的协议
                    _commiter.commitSucRes(
                            NP2US_RB_Writer_005_WebPayOp.make_002_RetCreateWebPayOrderFailed(
                                    webOrderBatch.getResult().getCode(), failedGoodsIds));
                    return;
                }

                _commiter.commitSucRes(
                        NP2US_RB_Writer_005_WebPayOp.make_002_RetCreateWebPayOrder(
                                _userData.getUid(), webOrderBatch.getData()));

                // 如果是加载用户数据后触发的请求，则卸载用户数据
                if (_needOffload)
                {
                    getUSServer().getUsUserMgr().changeUserOffline(_userData.getCid(), _newSerialize);
                }
            }
        });
    }
}
