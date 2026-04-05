package NPScheduleServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description: US服务器监控信息
 * @author: ricci
 * @date: 2023-03-20 11:48:44
 */
public class NPSSMonitorInfo extends _ANPServerBasicMonitorInfo
{
    @Override
    protected JsonElement __makeSubJson()
    {
        JsonObject jsObj = new JsonObject();
        jsObj.add("__ref__", getRefJson());


        return jsObj;
    }

    private JsonObject getRefJson()
    {
        JsonObject jsobj = new JsonObject();
        return jsobj;
    }
}
