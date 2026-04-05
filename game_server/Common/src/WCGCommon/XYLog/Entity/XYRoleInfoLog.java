package WCGCommon.XYLog.Entity;

/**
 * 角色详细信息,每隔一小时上报，所有在前一次上报后发生变化的角色信息
 */
public class XYRoleInfoLog extends BaseXYLog
{
    public String chr;//注册渠道标识
    public String svr;//区服标识
    public int tsr;//注册时间，大于0的10位Unix时间戳
    public String rid;//角色ID，全服唯一，建议serverId+roleId
    public String rn;//角色名
    public String aid;//账号ID，渠道唯一
    public int tsl;//最后登录时间，大于0的10位unix时间戳
    public int lv;//角色等级，非负整数
    public String ip;//15	IP地址，可选字段
    public int tpc;//累计购买一级代币数量，非负整数
    public int rc;//剩余一级代币数量，非负整数
    public int ts;//角色信息统计时间，大于0的10位Unix时间戳

    @Override
    public String getApi()
    {
        return "roleinfo";
    }
}
