package NPHttpServer.Http.HttpContorller;

import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Http.HttpService.server.HttpMethod;
import NPCommon.Http.HttpService.server.HttpRequest;
import NPCommon.Http.HttpService.server.HttpResponse;
import NPCommon.Log.CommLog;

/**
 * @description: 接口测试
 * 
 * @author: mark
 */

public class NPHSTestController
{
    @RequestMapping(uri = "/game/test", method = HttpMethod.GET)
    public void pushServerInfoChg(HttpRequest _request, final HttpResponse _response)
    {
        try
        {
            _response.response(200, "success");
        } 
        catch (Exception e)
        {
            CommLog.error("NPHSTestController /game/test error Exception:{}", e);
        }
    }
}
