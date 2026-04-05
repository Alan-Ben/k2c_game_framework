package PayCenter.PayServer;

import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonObject;

/**
 * PayMonitorInfo - 支付服务器监控信息类
 * 
 * 主要功能：
 * 1. 收集和存储PayServer的运行状态信息
 * 2. 提供系统资源使用情况监控
 * 3. 生成JSON格式的监控报告
 * 4. 支持外部监控系统集成
 * 
 * 设计特点：
 * - 封装服务器关键监控指标
 * - JSON序列化支持
 * - 内存和系统信息统计
 * - 服务器标识信息管理
 */
public class PayMonitorInfo
{
    // 系统内存信息
    private long totalMemory;
    private long maxMemory;
    private long freeMemory;
    
    // 版本和时区信息
    private String version;
    private String timeZone;
    
    // 服务器类型和标识
    private EServerType serverType;
    private int serverTypeId;
    
    // 网络连接信息
    private String ip;
    private int innerPort;

    // Getter和Setter方法
    public long getTotalMemory() { return totalMemory; }
    public void setTotalMemory(long totalMemory) { this.totalMemory = totalMemory; }

    public long getMaxMemory() { return maxMemory; }
    public void setMaxMemory(long maxMemory) { this.maxMemory = maxMemory; }

    public long getFreeMemory() { return freeMemory; }
    public void setFreeMemory(long freeMemory) { this.freeMemory = freeMemory; }

    public String getVersion() { return version; }
    public void setVersion(String version) { this.version = version; }

    public String getTimeZone() { return timeZone; }
    public void setTimeZone(String timeZone) { this.timeZone = timeZone; }

    public EServerType getServerType() { return serverType; }
    public void setServerType(EServerType serverType) { this.serverType = serverType; }

    public int getServerTypeId() { return serverTypeId; }
    public void setServerTypeId(int serverTypeId) { this.serverTypeId = serverTypeId; }

    public String getIp() { return ip; }
    public void setIp(String ip) { this.ip = ip; }

    public int getInnerPort() { return innerPort; }
    public void setInnerPort(int innerPort) { this.innerPort = innerPort; }

    /**
     * 生成监控信息的JSON对象
     * 
     * 包含字段：
     * - totalMemory: 总内存
     * - maxMemory: 最大内存
     * - freeMemory: 空闲内存
     * - usedMemory: 已使用内存
     * - version: 版本信息
     * - timeZone: 时区信息
     * - serverType: 服务器类型
     * - serverTypeId: 服务器类型ID
     * - ip: IP地址
     * - innerPort: 内部端口
     * 
     * @return JSON格式的监控信息对象
     */
    public JsonObject makeJsonObj()
    {
        JsonObject jsonObj = new JsonObject();
        
        jsonObj.addProperty("totalMemory", totalMemory);
        jsonObj.addProperty("maxMemory", maxMemory);
        jsonObj.addProperty("freeMemory", freeMemory);
        jsonObj.addProperty("usedMemory", totalMemory - freeMemory);
        
        jsonObj.addProperty("version", version);
        jsonObj.addProperty("timeZone", timeZone);
        
        jsonObj.addProperty("serverType", serverType.toString());
        jsonObj.addProperty("serverTypeId", serverTypeId);
        
        jsonObj.addProperty("ip", ip);
        jsonObj.addProperty("innerPort", innerPort);
        
        return jsonObj;
    }
}