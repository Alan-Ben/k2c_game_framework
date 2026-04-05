package NPUSServer.AiService;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_AiChatMessage;
import NP2HS_R.p003_AiChatOp.ToHS_R_003_001_ReqAiChatCompletion;
import NP2HS_RB.p003_AiChatOp.ToHS_RB_003_001_RetAiChatCompletion;
import NPCommon.Enum.EUsParam;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;

public class AiServiceFunc
{
    private NPUserServer _m_server;
    private Map<Long, AiRequestContext> _m_requestMap;
    // 记录每个玩家当前的请求数量
    private Map<Long, Integer> _m_requestCountMap;

    private static long REQUEST_TIMEOUT_MS; // 30秒超时
    private static int MAX_CONCURRENT_REQUESTS_PER_PLAYER; // 每个玩家最多同时请求n个

    private MutexAtom _m_mutex;

    public AiServiceFunc(NPUserServer _server)
    {
        _m_server = _server;
        _m_requestMap = new HashMap<>();
        _m_requestCountMap = new HashMap<>();
        _m_mutex = new MutexAtom();

        REQUEST_TIMEOUT_MS = 30000;
        MAX_CONCURRENT_REQUESTS_PER_PLAYER = -1;

        // 注册定时任务
        ALSynTaskManager.getInstance().regTask(this::cleanTimeoutRequests);
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 检查玩家是否可以发起新请求
     */
    private boolean canPlayerSendRequest(long cid)
    {
        Integer currentCount = _m_requestCountMap.get(cid);
        if (currentCount == null)
            currentCount = 0;

        return currentCount < MAX_CONCURRENT_REQUESTS_PER_PLAYER || MAX_CONCURRENT_REQUESTS_PER_PLAYER == -1;
    }

    /**
     * 增加玩家请求计数
     */
    private void increasePlayerRequestCount(long cid)
    {
        _m_requestCountMap.merge(cid, 1, Integer::sum);
    }

    /**
     * 减少玩家请求计数
     */
    private void decreasePlayerRequestCount(long cid)
    {
        _m_requestCountMap.computeIfPresent(cid, (key, count) ->
        {
            int newCount = count - 1;
            return newCount > 0 ? newCount : null; // 如果计数为0则移除记录
        });
    }

    /**
     * 发送AI请求
     * @param _userdata
     * @param _charCode
     * @param _messages
     * @param _callback
     * @return
     */
    public long sendAIRequest(NPUSUserData _userdata, String _charCode, List<Common_AiChatMessage> _messages, _ICallBackIntT<String> _callback)
    {
        long cid = _userdata.getCid();

        _lock();
        try
        {
            // 检查玩家请求限制
            if (!canPlayerSendRequest(cid))
            {
                USLog.warn(_m_server, "AiServiceFunc sendAIRequest, player request limit exceeded, cid:{} currentCount:{}",
                        cid, _m_requestCountMap.getOrDefault(cid, 0));

                // 直接回调错误
                ALSynTaskManager.getInstance().regTask(() -> _callback.onRunOver(CommErr.SYS_BUSY.getCode(), ""));

                return -1;
            }

            // 增加玩家请求计数
            increasePlayerRequestCount(cid);
        } finally
        {
            _unlock();
        }

        //生成请求ID
        long requestId = _userdata.getUSServer().getUSParams().incParam(EUsParam.AI_SERVICE_SERIAL);

        //生成请求上下文
        AiRequestContext context = new AiRequestContext(requestId, _userdata.getCid(), CommonFunc.getNowTimeMS(), _callback);

        _lock();
        try
        {
            _m_requestMap.put(requestId, context);
        } finally
        {
            _unlock();
        }

        //构造协议
        ToHS_R_003_001_ReqAiChatCompletion proto = new ToHS_R_003_001_ReqAiChatCompletion();
        proto.setUid(_userdata.getUid());
        proto.setCid(_userdata.getCid());
        proto.setRole(_userdata.getPlayerComponent().getName());
        proto.setLevel((int) _userdata.getParam(ENPPlayerParam.LEVEL));
        proto.setVipLevel((int) _userdata.getParam(ENPPlayerParam.VIP_LVL));
        proto.setUsRequestSerial(requestId);
        proto.setUsId(_userdata.getUSServer().getServerTypeId());
        proto.setCharCode(_charCode);
        proto.getMsgList().addAll(_messages);

        //发起请求
        _m_server.sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.HTTP.ordinal(), proto, new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new ToHS_RB_003_001_RetAiChatCompletion();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _retMsg)
            {
            }

            @Override
            public void dealFail(int _errCode)
            {
                handleAIResponse(requestId, _errCode, "");
            }
        });

        return requestId;
    }

    /**
     * 处理AI响应
     */
    public void handleAIResponse(long requestId, int _errCode, String response)
    {
        AiRequestContext context;

        _lock();
        try
        {
            context = _m_requestMap.remove(requestId);
        } finally
        {
            _unlock();
        }

        if (context == null)
        {
            USLog.warn(_m_server, "AiServiceFunc handleAIResponse, request not found, requestId:{} errCode:{} response:{}",
                    requestId, _errCode, response);
            return;
        }

        // 减少玩家请求计数
        _lock();
        try
        {
            decreasePlayerRequestCount(context.getCid());
        } finally
        {
            _unlock();
        }

        context.getCallback().onRunOver(_errCode, response);
    }

    /**
     * 定时清理超时请求
     */
    private void cleanTimeoutRequests()
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        List<Long> timeoutRequestIdList = new ArrayList<>();
        _lock();
        try
        {
            // 先收集超时的请求ID
            for (Entry<Long, AiRequestContext> entry : _m_requestMap.entrySet())
            {
                AiRequestContext context = entry.getValue();
                if (nowTimeMS - context.getCreateTimeMs() >= REQUEST_TIMEOUT_MS)
                {
                    timeoutRequestIdList.add(entry.getKey());
                }
            }

            // 再统一处理超时请求
            for (Long requestId : timeoutRequestIdList)
            {
                AiRequestContext context = _m_requestMap.remove(requestId);
                if (context != null)
                {
                    decreasePlayerRequestCount(context.getCid());
                    context.getCallback().onRunOver(CommErr.PROCESS_TIMEOUT.getCode(), "");
                    USLog.info(_m_server, "AiServiceFunc cleaned timeout request, requestId:{} cid:{}",
                            requestId, context.getCid());
                }
            }
        } finally
        {
            _unlock();
        }

        if (!timeoutRequestIdList.isEmpty())
        {
            USLog.info(_m_server, "AiServiceFunc cleaned {} timeout requests", timeoutRequestIdList.size());
        }

        // 重新注册定时任务
        ALSynTaskManager.getInstance().regTask(this::cleanTimeoutRequests, 5000);
    }
}
