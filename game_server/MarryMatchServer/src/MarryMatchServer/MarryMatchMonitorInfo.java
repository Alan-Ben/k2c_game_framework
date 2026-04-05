package MarryMatchServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description:  子嗣联姻服务器监控信息
 * @author: ricci
 * @date: 2023-03-20 11:48:44
 */
public class MarryMatchMonitorInfo extends _ANPServerBasicMonitorInfo
{
    @Override
    protected JsonElement __makeSubJson()
    {
        return new JsonObject();
    }
}
