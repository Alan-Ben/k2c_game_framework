package NPLoginServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import WCGBasicServer.WCGBasicServerConf;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description: US服务器监控信息
 * @author: ricci
 * @date: 2023-03-20 11:48:44
 */
public class NPLSMonitorInfo extends _ANPServerBasicMonitorInfo
{
    @Override
    protected JsonElement __makeSubJson()
    {
        JsonObject jsObj = new JsonObject();
        jsObj.addProperty("areaTag", LoginServerConf.getInstance().getAreaTag());
        jsObj.addProperty("recBuff", WCGBasicServerConf.getInstance().getClientRecBufferLen());
        jsObj.addProperty("clientBuff", LoginServerConf.getInstance().getClientSocketCacheSize());
        return jsObj;
    }
}
