package ShareCodeCenter.ShareCodeServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

public class ShareCodeMonitorInfo extends _ANPServerBasicMonitorInfo
{
    @Override
    protected JsonElement __makeSubJson()
    {
        return new JsonObject();
    }
}
