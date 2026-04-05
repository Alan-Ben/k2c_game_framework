package NPBusServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import WCGBasicBusServer.WCGBasicBusServerConf;
import WCGBasicServer.WCGBasicServerConf;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description: BUS服务器监控信息
 * @author: ricci
 * @date: 2023-03-20 11:48:44
 */
public class NPBusMonitorInfo extends _ANPServerBasicMonitorInfo
{
    @Override
    protected JsonElement __makeSubJson()
    {
    	JsonObject jsObj = new JsonObject();
        jsObj.addProperty("max_handle_count", WCGBasicBusServerConf.getInstance().getMaxHandleCount());
        jsObj.addProperty("rec_buff_length", WCGBasicServerConf.getInstance().getClientRecBufferLen());
        
        return jsObj;
    }
}
