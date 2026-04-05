package WCGCommon.XYLog.Entity;

/**
 * 登录游戏服务器信息,用户登录服务器后上报
 */
public class XYLoginServerLog extends BaseXYLog
{
    public String chr; //注册渠道标识
    public String chl; //登录渠道标识
    public String svr; //区服标识
    public int ts; //登录服务器时间，大于0的10位Unix时间戳
    public String aid; //账号ID，渠道唯一
    public String dt; //设备类型，可选字段
    public String dno; //设备编号，可选字段

    @Override
    public String getApi()
    {
        return "loginserver";
    }
}
