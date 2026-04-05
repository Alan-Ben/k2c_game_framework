package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicServer.ALBasicMutex.MutexAtom;
import AllRpcData.US_Service.Common.ExecPlayerGmCommand;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Promise.Promise;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityGMCommandPlayer;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormGMCommandPlayerDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import RPC._ARpcCallBack;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

public class NP2HS_003_001_PlatFromPlayerGMCommand extends _ANPPlatFormHttpSubDealer<NPEntityGMCommandPlayer>
{
    private static class PlayerResult
    {
        public long cid;
        public boolean isSuccess;
        public String msg;
    }

    @Override
    public int subOrder()
    {
        return 1;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityGMCommandPlayer> getDecoder()
    {
        return NPPlatFormGMCommandPlayerDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityGMCommandPlayer _decodeObj)
    {
        Promise promise = new Promise();
        //目标玩家列表
        Set<Long> cidList = _decodeObj.getCidSet();
        //结果列表
        List<PlayerResult> resultList = new ArrayList<PlayerResult>();
        //结果列表锁
        MutexAtom locker = new MutexAtom();

        int promiseIndex = 0;
        for (Long cid : cidList)
        {
            //目标服务器id
            int usId = CommonFunc.parseServerTypeIdFromCid(cid);

            //rpc对象
            ExecPlayerGmCommand rpc = new ExecPlayerGmCommand();
            rpc.req().setCid(cid);
            rpc.req().setCommand(_decodeObj.getCommand());

            int finalPromiseIndex = promiseIndex;
            promise.then(promiseIndex, p ->
            {
                NPHttpServer.getInstance().rpc2us.requestTo(usId, rpc, new _ARpcCallBack<ExecPlayerGmCommand>()
                {
                    @Override
                    public void call_back(int _errCode, ExecPlayerGmCommand _rpc)
                    {
                        PlayerResult result = new PlayerResult();
                        if (_errCode != Result.SUCC.getCode())
                        {
                            result.cid = cid;
                            result.isSuccess = false;
                            result.msg = "server error code:" + _errCode;
                        } else
                        {
                            result.cid = cid;
                            result.isSuccess = _rpc.retObj().getIsSucc();
                            result.msg = _rpc.retObj().getResult();
                        }

                        locker.lock();
                        try
                        {
                            resultList.add(result);

                            //输出日志
                            if (_rpc.retObj().getIsSucc())
                            {
                                CommLog.info("NP2HS_003_001_PlatFromPlayerGMCommand deal proto to cid:{} usId:{} command:{} success", cid, usId, _decodeObj.getCommand());
                            } else
                            {
                                CommLog.error("NP2HS_003_001_PlatFromPlayerGMCommand deal proto to cid:{} usId:{} command:{} failed", cid, usId, _decodeObj.getCommand());
                            }
                        } finally
                        {
                            locker.unlock();
                        }

                        promise.commit(finalPromiseIndex);
                    }
                });
            });

            promiseIndex++;
        }

        promise.over((p) ->
        {
            StringBuilder sb = new StringBuilder();
            for (PlayerResult result : resultList)
            {
                sb.append(String.format("cid:[%d] result:%s msg:%s\n", result.cid, result.isSuccess, result.msg));
            }
            _commiter.commitSuc(sb.toString());
        });
    }
}
