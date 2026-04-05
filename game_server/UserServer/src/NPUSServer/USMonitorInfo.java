package NPUSServer;

import NPCommon.MonitorInfo._ANPServerBasicMonitorInfo;
import NPEnum.EServerOnlineState;
import NPUSServer.CommonActivityMgr.Factory.CommonActivityFactory;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description: US服务器监控信息
 * @author: ricci
 * @date: 2023-03-20 11:48:44
 */
public class USMonitorInfo extends _ANPServerBasicMonitorInfo
{
    /**
     * 服务器在线状态
     */
    private EServerOnlineState onlineState;

    /**
     * 是否开启了GM命令
     */
    private boolean openGM;

    /**
     * 内存用户数量
     */
    private int cachedUserCount;

    /**
     * 在线用户数量
     */
    private int onlineUserCount;

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
    
    public EServerOnlineState getOnlineState()
    {
        return onlineState;
    }

    public void setOnlineState(EServerOnlineState onlineState)
    {
        this.onlineState = onlineState;
    }

    public boolean isOpenGM()
    {
        return openGM;
    }

    public void setOpenGM(boolean openGM)
    {
        this.openGM = openGM;
    }

    public int getCachedUserCount()
    {
        return cachedUserCount;
    }

    public void setCachedUserCount(int cachedUserCount)
    {
        this.cachedUserCount = cachedUserCount;
    }

    public int getOnlineUserCount()
    {
        return onlineUserCount;
    }

    public void setOnlineUserCount(int onlineUserCount)
    {
        this.onlineUserCount = onlineUserCount;
    }

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
    
    //平台相关数据
    private int _m_iPlatId;
    public void setPlatId(int _platId) {this._m_iPlatId = _platId;}
    public int getPlatId() {return this._m_iPlatId;}
    
    private int _m_iAreaId;
    public void setAreaId(int _areaId) {this._m_iAreaId = _areaId;}
    public int getAreaId() {return this._m_iAreaId;}
    
    //数据版本
    private String _m_sDBVersion;
    public void setDBVersion(String _dbVersion) {this._m_sDBVersion = _dbVersion;}
    public String getDBVersion() {return this._m_sDBVersion;}

    @Override
    protected JsonElement __makeSubJson()
    {
        JsonObject jsobj = new JsonObject();
        jsobj.addProperty("onlineState", getOnlineState().name());
        jsobj.addProperty("openGM", isOpenGM());
        jsobj.addProperty("activityCreatorCount", CommonActivityFactory.getInstance().getCreatorSize());
        jsobj.addProperty("cachedUserCount", getCachedUserCount());
        jsobj.addProperty("onlineUserCount", getOnlineUserCount());
        jsobj.addProperty("platId", getPlatId());
        jsobj.addProperty("areaId", getAreaId());
        jsobj.addProperty("dbVersion", getDBVersion());
        jsobj.add("__ref__", getRefJson());

        return jsobj;
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
