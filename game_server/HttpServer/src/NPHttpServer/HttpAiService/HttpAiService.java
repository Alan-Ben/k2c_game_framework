package NPHttpServer.HttpAiService;


import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_AiChatMessage;
import CommonEnum.EAiChatRoleType;
import NP2HS_R.p003_AiChatOp.ToHS_R_003_001_ReqAiChatCompletion;
import NP2US.p003_AiChatOp.ToUS_003_001_AiMsgAdd;
import NPCommon.Enum.EHsParam;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HttpErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.HSParams;
import NPHttpServer.Http.Core.NPHSHttpServiceCore;
import NPHttpServer.Http.Core._INPHttpCallBack;
import NPHttpServer.HttpServerConf;
import NPHttpServer.NPHttpServer;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import org.apache.http.Header;
import org.apache.http.message.BasicHeader;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;

public class HttpAiService
{
    private static HttpAiService _g_instance = new HttpAiService();

    public static HttpAiService getInstance()
    {
        return _g_instance;
    }

    //最大请求数
    private static final long MAX_REQUEST_COUNT = 10000;
    //25秒超时
    private static final long REQUEST_TIMEOUT_MS = 25000;
    //项目ID
    private static final int PROJECT_ID = 23;

    private Map<Long, HttpAiRequestContext> _m_requestMap;
    private MutexAtom _m_mutex;

    public HttpAiService()
    {
        _m_requestMap = new HashMap<>();
        _m_mutex = new MutexAtom();

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
     * 处理AI请求
     * @param _usId
     * @param _usRequestId
     * @param _request
     */
    public void dealRequest(int _usId, long _usRequestId, ToHS_R_003_001_ReqAiChatCompletion _request)
    {
        //构造请求上下文
        HttpAiRequestContext context = new HttpAiRequestContext(_usId, _usRequestId, CommonFunc.getNowTimeMS());

        long requestId;
        _lock();
        try
        {
            //判断是否超过最大请求数
            if (_m_requestMap.size() >= MAX_REQUEST_COUNT)
            {
                dealResponse(context, CommErr.SYS_BUSY.getCode(), "");
                return;
            }

            requestId = HSParams.getInstance().incParam(EHsParam.AI_SERVICE_SERIAL);
            _m_requestMap.put(requestId, context);
        } finally
        {
            _unlock();
        }

        //发送请求
        sendRequestToAiPlatform(requestId, _request);
    }

    /**
     * 处理AI请求
     * @param _requestId
     * @param _request
     */
    private void sendRequestToAiPlatform(long _requestId, ToHS_R_003_001_ReqAiChatCompletion _request)
    {
        //获取uri
        String url = HttpServerConf.getInstance().getAiPlatUrl() + "/api/game/chat_completion/send";
        //生成header
        ArrayList<BasicHeader> headers = buildPlatFormUrlHeader();
        //生成基础param
        String paramList = buildPlatFormJsonBody(_requestId, _request);
        //发起http请求
        NPHSHttpServiceCore.getInstance().httpPostJson(url, headers, paramList, new _INPHttpCallBack()
        {
            @Override
            public void onSuc(Header[] headers, int code, String _responseStr)
            {
                if (_responseStr.isEmpty() || code != 200)
                {
                    CommLog.warn("HttpAiService sendRequestToAiPlatform response error: requestId {}, code {}, response {}", _requestId, code, _responseStr);
                    dealResponse(_requestId, HttpErr.HTTP_RESPONSE_ERROR.getCode(), "");
                    return;
                }

                //检查回包
                JsonObject obj = new JsonParser().parse(_responseStr).getAsJsonObject();
                if (!obj.has("code") || 200 != obj.get("code").getAsInt())
                {
                    CommLog.warn("HttpAiService sendRequestToAiPlatform response: requestId {}, response {}", _requestId, obj.toString());
                    dealResponse(_requestId, HttpErr.PLATFORM_RESPONSE_ERROR.getCode(), "");
                }
            }

            @Override
            public void onFail(Result _result)
            {
                dealResponse(_requestId, _result.getCode(), "");
            }
        });
    }

    /**
     * 创建一个header列表，请求后台通用的header参数加上自定义参数
     * @return ArrayList<BasicHeader>
     */
    public static ArrayList<BasicHeader> buildPlatFormUrlHeader()
    {
        ArrayList<BasicHeader> headers = new ArrayList<>();
        headers.add(new BasicHeader("Content-Type", "application/json"));
        headers.add(new BasicHeader("Authorization", "Bearer " + "3wa91llsmcewtmqilt2le2m8jjdz0f5q"));
        return headers;
    }

    /**
     * 创建JSON格式的请求体
     * @return String
     */
    public static String buildPlatFormJsonBody(long _requestId, ToHS_R_003_001_ReqAiChatCompletion _paramList)
    {
        JsonObject jsonObj = new JsonObject();
        jsonObj.addProperty("request_id", String.valueOf(_requestId));
        jsonObj.addProperty("project", PROJECT_ID);
        jsonObj.addProperty("region", HttpServerConf.getInstance().getRegionId());
        jsonObj.addProperty("char_code", _paramList.getCharCode());
        jsonObj.addProperty("uid", _paramList.getUid());
        jsonObj.addProperty("cid", String.valueOf(_paramList.getCid()));
        jsonObj.addProperty("role", _paramList.getRole());
        jsonObj.addProperty("level", _paramList.getLevel());
        jsonObj.addProperty("vip_level", _paramList.getVipLevel());
        JsonArray jsonArray = new JsonArray();
        for (int i = 0; i < _paramList.getMsgList().size(); i++)
        {
            Common_AiChatMessage msg = _paramList.getMsgList().get(i);

            JsonObject msgObj = new JsonObject();
            msgObj.addProperty("role", transRoleTypeToString(msg.getRoleType()));
            msgObj.addProperty("content", msg.getMsg());
            jsonArray.add(msgObj);
        }
        jsonObj.add("messages", jsonArray);
        jsonObj.addProperty("notify_url", HttpServerConf.getInstance().getAiPlatCallbackUrl());
        return jsonObj.toString();

    }

    /**
     * 将AI角色类型转换为字符串
     * @param roleType
     * @return
     */
    public static String transRoleTypeToString(EAiChatRoleType roleType)
    {
        switch (roleType)
        {
            case SYSTEM:
                return "assistant";
            case USER:
                return "user";
        }
        return "unknown";
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
            for (Entry<Long, HttpAiRequestContext> entry : _m_requestMap.entrySet())
            {
                HttpAiRequestContext context = entry.getValue();
                if (nowTimeMS - context.getCreateTimeMs() >= REQUEST_TIMEOUT_MS)
                {
                    timeoutRequestIdList.add(entry.getKey());
                }
            }

            // 再统一处理超时请求
            for (Long requestId : timeoutRequestIdList)
            {
                HttpAiRequestContext context = _m_requestMap.remove(requestId);
                if (context != null)
                {
                    CommLog.error("HttpAiService cleanTimeoutRequests: requestId {} timeout, usId {}, usRequestId {}",
                            requestId, context.getUsId(), context.getUsRequestId());
                    //回调通知超时
                    dealResponse(context, CommErr.PROCESS_TIMEOUT.getCode(), "");
                }
            }
        } finally
        {
            _unlock();
        }

        if (!timeoutRequestIdList.isEmpty())
        {
            CommLog.info("HttpAiService cleaned {} timeout requests", timeoutRequestIdList.size());
        }

        // 重新注册定时任务
        ALSynTaskManager.getInstance().regTask(this::cleanTimeoutRequests, 5000);
    }

    /**
     * 处理AI请求响应
     * @param _requestId
     * @param _errCode
     * @param _response
     */
    public void dealResponse(long _requestId, int _errCode, String _response)
    {
        HttpAiRequestContext context;

        _lock();
        try
        {
            context = _m_requestMap.get(_requestId);
            if (context == null)
            {
                CommLog.error("HttpAiService dealResponse: requestId {} not found", _requestId);
                return;
            }

            //从请求映射中移除
            _m_requestMap.remove(_requestId);
        } finally
        {
            _unlock();
        }

        dealResponse(context, _errCode, _response);
    }

    /**
     * 处理AI请求响应
     * @param _context
     * @param _errCode
     * @param _response
     */
    public void dealResponse(HttpAiRequestContext _context, int _errCode, String _response)
    {
        ToUS_003_001_AiMsgAdd proto = new ToUS_003_001_AiMsgAdd();
        proto.setUsRequestId(_context.getUsRequestId());
        proto.setErrCode(_errCode);
        proto.setResponse(_response);

        NPHttpServer.getInstance().sendMessageToBSServer(EServerType.USER.ordinal(), _context.getUsId(), proto);
    }


}
