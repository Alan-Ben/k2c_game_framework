package NPCommon.MonitorInfo;

import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;

/**
 * @description: 服务器基础监控信息
 * @author: ricci
 * @date: 2023-03-20 11:14:32
 */
public abstract class _ANPServerBasicMonitorInfo
{
    /**
     * 总内存 方法返回Java虚拟机中的总内存量，即Java虚拟机当前可用的总内存大小。
     * 这包括了已经被分配和未被分配的内存
     */
    private long totalMemory;
    /**
     * 最大内存 方法返回Java虚拟机尝试使用的最大内存量，即Java虚拟机可以使用的最大内存大小。
     * 如果Java虚拟机尝试使用超出这个值的内存，会抛出OutOfMemoryError异常
     */
    private long maxMemory;

    /**
     * 方法返回Java虚拟机中的空闲内存量，即当前未被分配的内存大小
     */
    private long freeMemory;

    /**
     * 服务器版本号 来源于NPVersion
     */
    private String version;

    /**
     * 时区
     */
    private String timeZone;
    /**
     * 服务器类型
     */
    private EServerType serverType;
    /**
     * 服务器id
     */
    private int serverTypeId;

    /**
     * 服务器所在ip
     */
    private String ip;

    /**
     * 服务器内部通讯端口
     */
    private int innerPort;

    public long getTotalMemory()
    {
        return totalMemory;
    }

    public void setTotalMemory(long totalMemory)
    {
        this.totalMemory = totalMemory;
    }

    public long getMaxMemory()
    {
        return maxMemory;
    }

    public void setMaxMemory(long maxMemory)
    {
        this.maxMemory = maxMemory;
    }

    public long getFreeMemory()
    {
        return freeMemory;
    }

    public void setFreeMemory(long freeMemory)
    {
        this.freeMemory = freeMemory;
    }

    public String getVersion()
    {
        return version;
    }

    public void setVersion(String version)
    {
        this.version = version;
    }

    public String getTimeZone()
    {
        return timeZone;
    }

    public void setTimeZone(String timeZone)
    {
        this.timeZone = timeZone;
    }

    public EServerType getServerType()
    {
        return serverType;
    }

    public void setServerType(EServerType serverType)
    {
        this.serverType = serverType;
    }

    public String getServerTypeId()
    {
        if (getServerType() == EServerType.SINGLE)
        {
            ENPSingleServerType singleServerType = ENPSingleServerType.ENPSingleServerType_FromInt(serverTypeId);
            if (singleServerType == null)
            {
                return String.valueOf(serverTypeId);
            }
            return singleServerType.name();
        }
        return String.valueOf(serverTypeId);
    }

    public void setServerTypeId(int serverTypeId)
    {
        this.serverTypeId = serverTypeId;
    }

    public String getIp()
    {
        return ip;
    }

    public void setIp(String ip)
    {
        this.ip = ip;
    }

    public int getInnerPort()
    {
        return innerPort;
    }

    public void setInnerPort(int innerPort)
    {
        this.innerPort = innerPort;
    }

    /**
     * 构造json格式
     * @return
     */
    public JsonObject makeJsonObj()
    {
        JsonObject jsobj = new JsonObject();
        jsobj.addProperty("totalMemory", getTotalMemory());
        jsobj.addProperty("maxMemory", getMaxMemory());
        jsobj.addProperty("freeMemory", getFreeMemory());
        jsobj.addProperty("version", getVersion());
        jsobj.addProperty("timeZone", getTimeZone());
        jsobj.addProperty("serverType", getServerType() + "-" + getServerTypeId());
        jsobj.addProperty("ip", getIp());
        jsobj.addProperty("innerPort", getInnerPort());
        jsobj.add("__extMsg__", __makeSubJson());
        return jsobj;
    }

    protected abstract JsonElement __makeSubJson();
}
