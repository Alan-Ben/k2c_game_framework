package WCGCommon.XYLog.Entity;

/**
 * 创建角色信息,角色ID产生后上报
 */
public class XYRoleLog extends BaseXYLog
{
    public String chr; //注册渠道标识
    public String chl; //登录渠道标识
    public String svr; //区服标识
    public int ts; //创角时间，大于0的10位Unix时间戳
    public String rid; //角色ID，全服唯一，建议serverId+roleId
    public String aid; //账号ID，渠道唯一

    @Override
    public String getApi()
    {
        return "role";
    }
}
