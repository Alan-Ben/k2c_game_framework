package WCGCommon.XYLog.Entity;

/**
 * 用户在线信息,间隔五分钟定时上报
 */
public class XYOnlineLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String svr;//区服标识
    public int ts;//统计时间，大于0的10位Unix时间戳
    public int cnt;//在线数(按账号统计),非负整数

    @Override
    public String getApi()
    {
        return "online";
    }
}
