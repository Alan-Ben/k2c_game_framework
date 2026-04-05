package NPHttpServer.Http.HttpContorller;

import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Http.HttpService.server.HttpMethod;
import NPCommon.Http.HttpService.server.HttpRequest;
import NPCommon.Http.HttpService.server.HttpResponse;
import NPCommon.Log.CommLog;
import NPHttpServer.HttpAiService.HttpAiService;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 接口测试
 * 
 * @author: mark
 */

public class HSAiChatNotifyController
{
    @RequestMapping(uri = "/aichat/notify", method = HttpMethod.POST)
    public void pushServerInfoChg(HttpRequest _request, final HttpResponse _response)
    {
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_request.getPostBody()).getAsJsonObject();

            long requestId = jsonObject.get("request_id").getAsLong();

            int code = jsonObject.get("code").getAsInt();
            if (code == 200)
            {
                JsonObject jsonObject1 = jsonObject.get("data").getAsJsonObject();
                HttpAiService.getInstance().dealResponse(requestId, 0, jsonObject1.get("content").getAsString());
            }
            else
            {
                int errCode = jsonObject.get("code").getAsInt();
                String errMsg = jsonObject.get("message").getAsString();
                HttpAiService.getInstance().dealResponse(requestId, errCode, "");

                CommLog.info("HSAiChatNotifyController pushServerInfoChg /aichat/notify error: requestId={}, errCode={}, errMsg={}", requestId, errCode, errMsg);
            }

            //成功处理逻辑
            _response.response(200, "Success");
        } catch (Exception e)
        {
            CommLog.error("HSAiChatNotifyController pushServerInfoChg /aichat/notify error Exception:{}", e);
        }
    }
}
