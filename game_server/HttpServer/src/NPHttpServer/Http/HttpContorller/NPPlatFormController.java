package NPHttpServer.Http.HttpContorller;

import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Http.HttpService.server.HttpMethod;
import NPCommon.Http.HttpService.server.HttpRequest;
import NPCommon.Http.HttpService.server.HttpResponse;
import NPCommon.Log.CommLog;
import NPHttpServer.Http.HttpService.MsgDispather.NPHttpDealerDispatcher;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.HttpServerConf;
import WCGCommon.Security.MD5;
import org.apache.http.NameValuePair;
import org.apache.http.client.utils.URLEncodedUtils;

import java.util.Comparator;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;

import static org.apache.http.Consts.UTF_8;

/**
 * @description: 后台推送
 * @author: ricci
 * @date: 2023-03-24 14:21:04
 */

public class NPPlatFormController
{
    @RequestMapping(uri = "/game/platform", method = HttpMethod.POST)
    public void pushServerInfoChg(HttpRequest _request, final HttpResponse _response)
    {
        try
        {
            List<NameValuePair> parseList = URLEncodedUtils.parse(_request.getPostBody(), UTF_8);
            HashMap<String, String> keyValMap = new HashMap<>();
            for (NameValuePair nameValuePair : parseList)
            {
                keyValMap.put(nameValuePair.getName(), nameValuePair.getValue());
            }
            //签名验证
            if (!__signCheck(parseList))
            {
                _response.response(401, "Sign check fail!");
                CommLog.error("accept NPPlatFormController __signCheck fail /game/platform signCheck error", _request.getPostBody());
                return;
            }

            //主协议号
            String protocolNoStr = keyValMap.get("protocolNo");
            int protocolNo = Integer.parseInt(protocolNoStr);
            //次协议号
            String subProtocolNoStr = keyValMap.get("subProtocolNo");
            int subProtocolNo = Integer.parseInt(subProtocolNoStr);
            //平台请求序列号，用于后续回复响应确认
            String serial = keyValMap.get("serial");

            _ANPPlatFormHttpSubDealer<?> dealer = NPHttpDealerDispatcher.getInstance().lookupDealer(protocolNo, subProtocolNo);
            if (dealer == null)
            {
                _response.response(404, "Request not find dealer!");
                CommLog.error("NPPlatFormController  /game/platform request not find dealer:{}-{}", protocolNo, subProtocolNo);
                return;
            }
            //处理消息
            dealer.dealMsg(serial, keyValMap.get("data"), _response);

            CommLog.info("accept NPPlatFormController /game/platform" + protocolNo + "-" + subProtocolNo);
        } catch (Exception e)
        {
            CommLog.error("NPPlatFormController pushServerInfoChg /game/platform error Exception:{}", e);
        }
    }

    /**
     * 签名校验
     * @param _parseList 参数列表
     * @return boolean
     */
    private boolean __signCheck(List<NameValuePair> _parseList)
    {
        _parseList.sort(Comparator.comparing(NameValuePair::getName));
        String clientSign = "";
        StringBuilder sb = new StringBuilder();
        for (NameValuePair nameValuePair : _parseList)
        {
            //跳过签名字段
            if ("sign".equals(nameValuePair.getName()))
            {
                clientSign = nameValuePair.getValue();
                continue;
            }
            sb.append(nameValuePair.getName()).append("=").append(nameValuePair.getValue());
        }
        sb.append(HttpServerConf.getInstance().getPlatFromClientKey());
        String sign = MD5.md5(sb.toString()).toLowerCase(Locale.ROOT);

        return sign.equals(clientSign);
    }
}
