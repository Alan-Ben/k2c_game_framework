package NPGateServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import WCGBasicServer.WCGBasicServerConf;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description: US服务器监控信息
 * @author: ricci
 * @date: 2023-03-20 11:48:44
 */
public class NPGSMonitorInfo extends _ANPServerBasicMonitorInfo
{
    /**
     * 当前配表的版本号
     */
    private String refVersion;

    /**
     * 准备更新的配表版本号
     */
    private String readyRefVersion;

    /**
     * 是否已经准备服务器配表更新
     */
    private boolean isReadyRefReload;
    /**
     * 是否服务器配表正在准备
     */
    private boolean isReadyRefLoading;

    public String getRefVersion()
    {
        return refVersion;
    }

    public void setRefVersion(String refVersion)
    {
        this.refVersion = refVersion;
    }

    public String getReadyRefVersion()
    {
        return readyRefVersion;
    }

    public void setReadyRefVersion(String readyRefVersion)
    {
        this.readyRefVersion = readyRefVersion;
    }

    public boolean getIsReadyRefReload()
    {
        return isReadyRefReload;
    }

    public void setIsReadyRefReload(boolean isReadyRefReload)
    {
        this.isReadyRefReload = isReadyRefReload;
    }

    public boolean getIsReadyRefLoading()
    {
        return isReadyRefLoading;
    }

    public void setIsReadyRefLoading(boolean isReadyRefLoading)
    {
        this.isReadyRefLoading = isReadyRefLoading;
    }

    @Override
    protected JsonElement __makeSubJson()
    {
        JsonObject jsObj = new JsonObject();
        jsObj.add("__ref__", getRefJson());
        
        jsObj.addProperty("areaTag", GateServerConf.getInstance().getAreaTag());
        jsObj.addProperty("recBuff", WCGBasicServerConf.getInstance().getClientRecBufferLen());
        jsObj.addProperty("maxUserHandle", GateServerConf.getInstance().getUserMaxCount());
        jsObj.addProperty("clientBuff", GateServerConf.getInstance().getClientSocketCacheSize());

        return jsObj;
    }

    private JsonObject getRefJson()
    {
        JsonObject jsobj = new JsonObject();
        jsobj.addProperty("refVersion", getRefVersion());
        jsobj.addProperty("isReadyRefLoading", getIsReadyRefLoading());
        jsobj.addProperty("isReadyRefReload", getIsReadyRefReload());
        jsobj.addProperty("readyRefVersion", getReadyRefVersion());
        return jsobj;
    }
}
