package NPUSServer.NPGeneralListener.RequestDispather.p005_WebPayOp;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import Common.ServerObj.ServerObj_WebPayGoodsList;
import NP2US_R.p005_WebPayOp.NP2US_R_005_001_ReqGetWebPayGoodsList;
import NP2US_RB.p002_GSOp.NP2US_RB_002_001_RegUserGate;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPServerProtocolWriter.NP2US.Response.NP2US_RB_Writer_005_WebPayOp;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.NPUSUserMgr;
import NPUSServer.NPUSUserMgr.UserSafeCall._IUserLoadOverHandler;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 获取网页支付商品列表请求处理器
 * <p>
 * 处理来自HttpServer的商品列表查询请求
 */
public class RequestDealer_NP2US_R_005_001_ReqGetWebPayGoodsList extends _ABasicGeneralRequestDealer<NP2US_R_005_001_ReqGetWebPayGoodsList>
{
    public RequestDealer_NP2US_R_005_001_ReqGetWebPayGoodsList(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2US_R_005_001_ReqGetWebPayGoodsList _msg)
    {
        NPUSUserMgr usUserMgr = getUSServer().getUsUserMgr();

        // 直接从内存查找
        NPUSUserData userData = usUserMgr.lookupCacheUserData(_msg.getCid());
        if (userData != null)
        {
            _makeGoodsList(_commiter, userData, _msg, false, 0);
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
                        USLog.error(getUSServer(), "[WebPay] web pay make goods list failed, player cid:{} not found", _msg.getCid());
                        _commiter.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
                        return;
                    }

                    //异步线程初始化玩家数据
                    ALSynTaskManager.getInstance().regTask(() -> 
                    {
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
                                            USLog.error(getUSServer(), "[WebPay] web pay make goods list failed, player cid:{} data load failed", _msg.getCid());
                                            _commiter.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
                                            return;
                                        }

                                        long newSerialize = commitMsg.getNewSerialize();

                                        _makeGoodsList(_commiter, loadedUserData, _msg, true, newSerialize);
                                    }
                                });
                            }

                            @Override
                            public void commitFailRes(int _errCode)
                            {
                                USLog.error(getUSServer(), "[WebPay] web pay make goods list failed, player cid:{} data load failed", _msg.getCid());
                                _commiter.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
                            }
                        });
                    });
                }
            });
        }
    }

    /**
     * 生成商品列表
     * @param _commiter
     * @param _userData
     * @param _msg
     * @param _needOffload
     * @param newSerialize
     */
    private void _makeGoodsList(_IWCGBasicRequestCommiter _commiter, NPUSUserData _userData, NP2US_R_005_001_ReqGetWebPayGoodsList _msg,
                                boolean _needOffload, long newSerialize)
    {
        _userData.safeCall(new _IUserLoadOverHandler()
        {
            @Override
            public void onLoadOver()
            {
                ServerObj_WebPayGoodsList webPayGoodsList = _userData.getOrderComponent().getMgr().getWebPayGoodsList();

                _commiter.commitSucRes(NP2US_RB_Writer_005_WebPayOp.make_001_RetGetWebPayGoodsList(webPayGoodsList));

                // 如果是加载用户数据后触发的请求，则卸载用户数据
                if (_needOffload)
                {
                    getUSServer().getUsUserMgr().changeUserOffline(_userData.getCid(), newSerialize);
                }
            }
        });
    }
}
