package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicServer.ALBasicMutex.MutexAtom;
import AllRpcData.US_Service.Common.ExecServerGmCommand;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Promise.Promise;
import NPHttpServer.Http.Entity.NPEntityGMCommandServer;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormGMCommandServerDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import RPC._ARpcCallBack;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

public class NP2HS_003_002_PlatFromServerGMCommand extends _ANPPlatFormHttpSubDealer<NPEntityGMCommandServer>
{
    private static class ServerResult
    {
        public int serverId;
        public boolean isSuccess;
        public String msg;
    }

    @Override
    public int subOrder()
    {
        return 2;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityGMCommandServer> getDecoder()
    {
        return NPPlatFormGMCommandServerDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityGMCommandServer _decodeObj)
    {
        Promise promise = new Promise();
        //目标玩家列表
        Set<Integer> serverIdList = _decodeObj.getServerIdList();
        //结果列表
        List<ServerResult> resultList = new ArrayList<>();
        //结果列表锁
        MutexAtom locker = new MutexAtom();

        int promiseIndex = 0;
        for (Integer serverId : serverIdList)
        {
            //rpc对象
            ExecServerGmCommand rpc = new ExecServerGmCommand();
            rpc.req().setCommand(_decodeObj.getCommand());

            int finalPromiseIndex = promiseIndex;
            promise.then(promiseIndex, p ->
            {
                NPHttpServer.getInstance().rpc2us.requestTo(serverId, rpc, new _ARpcCallBack<ExecServerGmCommand>()
                {
                    @Override
                    public void call_back(int _errCode, ExecServerGmCommand _rpc)
                    {
                        ServerResult result = new ServerResult();
                        if (_errCode != Result.SUCC.getCode())
                        {
                            result.serverId = serverId;
                            result.isSuccess = false;
                            result.msg = "server error code:" + _errCode;
                        } else
                        {
                            result.serverId = serverId;
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
                                CommLog.info("NP2HS_003_002_PlatFromServerGMCommand deal proto to usId:{} success command:{}", serverId, _decodeObj.getCommand());
                            } else
                            {
                                CommLog.error("NP2HS_003_002_PlatFromServerGMCommand deal proto to usId:{} failed command:{}", serverId, _decodeObj.getCommand());
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
            for (ServerResult result : resultList)
            {
                sb.append(String.format("usTypeId:[%d] result:%s msg:%s\n", result.serverId, result.isSuccess, result.msg));
            }
            _commiter.commitSuc(sb.toString());
        });
    }
}
