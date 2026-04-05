package NPLoginCheckServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description: US服务器监控信息
 * @author: ricci
 * @date: 2023-03-20 11:48:44
 */
public class NPLCSMonitorInfo extends _ANPServerBasicMonitorInfo
{
    @Override
    protected JsonElement __makeSubJson()
    {
        return new JsonObject();
    }
}
